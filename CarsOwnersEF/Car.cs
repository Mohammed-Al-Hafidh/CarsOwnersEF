using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsOwnersEF
{
    public class Car
    {
        [Key]
        [Required]
        public int CarId { get; set; }
        [StringLength(50)]
        public string MakeModel { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
