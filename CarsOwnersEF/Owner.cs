using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsOwnersEF
{
    public class Owner
    {
        [Key]
        [Required]
        public int OwnerId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [NotMapped]
        public int CarsNumber { get => Car.Count; }

        public byte[] Image { get; set; }
        public ICollection<Car> Car { get; set; }
        
    }
}
