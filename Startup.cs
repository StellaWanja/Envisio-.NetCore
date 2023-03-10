using NewEnvisioBackend.BusinessLogic;
using NewEnvisioBackend.Data;
using NewEnvisioBackend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NewEnvisioBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ITestResultService, TestResultService>();
            services.AddScoped<ITestResultRepository, TestResultRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddHttpClient();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //time for token validity
            services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));

            services.Configure<IdentityOptions>(options =>
            {
                //password requirements
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
            });

            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllers();
            //for JWT
            services.AddAuthentication(options =>
            {
                //set default
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {//set up token validation parameters
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //validate issuer of JWT
                        ValidateAudience = true, //validate audience of API
                        ValidateLifetime = true, //check if token is expired or not
                        ValidateIssuerSigningKey = true, //validate issuer signing key
                        ValidIssuer = Configuration["JWTSettings:Issuer"], //set up issuer from appsettings.json
                        ValidAudience = Configuration["JWTSettings:Audience"], //set up audience from appsettings.json
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSettings:SecretKey"]))
                    };
                    options.SaveToken = true;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EnvisioAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager, AppDbContext context)
        {
            //Enable CORS
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.Use(async (ctx, next) =>
            {
                // ctx.Response.Headers.Add("Header-Name", "Header-Value");
                //Hackers iframe your website to trick users into clicking unintended links. The X-Frame-Options tell any client that framing isn't allowed
                ctx.Response.Headers.Add("X-Frame-Options", "DENY");
                //MIME-type sniffing is an attack where a hacker tries to exploit missing metadata on served files. 
                ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                //The X-Xss-Protection header will cause most modern browsers to stop loading the page when a cross-site scripting attack is identified
                ctx.Response.Headers.Add("X-Xss-Protection", "1; mode=block");

                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Envisio.API v1"));

            Seeder.Seed(roleManager, userManager, context).Wait();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
