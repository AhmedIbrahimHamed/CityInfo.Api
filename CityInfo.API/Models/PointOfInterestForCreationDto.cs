using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models {
    public class PointOfInterestForCreationDto {
        [Required(ErrorMessage = "you should provide a valid name.")]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
