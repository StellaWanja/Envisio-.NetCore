using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NewEnvisioBackend.Data.DTOs.TestResultDTOs
{
    public class AddTestResultRequest
    {
        public string PatientId { get; set; }
        public string Result { get; set; }
    }
}
