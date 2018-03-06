using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EquitiesIntegration.Models
{
    public class Citizenship
    {
        [Key]
        public int CitizenshipId { get; set; }

        [DisplayName("Citizenship")]
        public string Name { get; set; }
    }
}