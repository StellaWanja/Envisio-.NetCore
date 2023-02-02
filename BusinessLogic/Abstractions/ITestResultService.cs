using NewEnvisioBackend.Models;
using NewEnvisioBackend.Data;
using NewEnvisioBackend.Data.DTOs;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewEnvisioBackend.Data.DTOs.PatientDTOs;
using Microsoft.EntityFrameworkCore;

namespace NewEnvisioBackend.BusinessLogic
{
    public interface ITestResultService
    {
        Task<TestResult> CreateTestResult(string patientId, string result);
        Task<List<TestResult>> GetAllTestResultsBelongingToAPatient(string patientId);
        Task<TestResult> GetFromApi();
    }
}