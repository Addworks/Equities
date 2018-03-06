using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EquitiesIntegration.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string IDNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public int CitizenshipId { get; set; }

        public virtual Citizenship Citizenship { get; set; }
    }
}