using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskLabBackend.Models;

namespace TaskLabBackend.Dto
{
    public class AddMemberDto
    {
        
        public int Id { get; set; }

        public string Name { get; set; }

       
        public string Role { get; set; }

       
        public string Email { get; set; }
  
        public decimal ExperienceInYears { get; set; }
    }
}
