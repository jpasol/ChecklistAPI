using System;
using System.ComponentModel.DataAnnotations;

namespace ChecklistAPI
{
    public class Issues
    {
        [Key]
        [Required]
        public int Key { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
