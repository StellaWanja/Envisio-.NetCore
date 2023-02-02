using System;
using System.Collections.Generic;
using System.Text;

namespace NewEnvisioBackend.Data.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HospitalName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
