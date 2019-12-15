using RiskProfileAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiskProfileAPI.Models
{
    public class User
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Age { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Dependents { get; set; }

        [Required]
        public House House { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Income { get; set; }

        [Required]
        public MaritalStatus Marital_Status { get; set; }

        [Required]
        public List<bool> Risk_Questions { get; set; }

        [Required]
        public Vehicle Vehicle { get; set; }

        public int BaseScore { get; set; }
    }
}