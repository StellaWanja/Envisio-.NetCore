using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NewEnvisioBackend.Models;

namespace NewEnvisioBackend.Data
{
    public interface ITestResultRepository
    {
        //add result
        Task<TestResult> AddTestResult(TestResult testResult);
        //get all results of a specific patient
        Task<List<TestResult>> GetAllTestResultsPerPatient(string patientId);
    }
}