using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.DTOs
{
    public class UserAddDto
    {
        [DisplayName("E-Mail Address")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(100, ErrorMessage = "{0} cannot pass {1} characters.")]
        [MinLength(10, ErrorMessage = "{0} cannot be less than {1} characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Name")]
        [MaxLength(30, ErrorMessage = "{0} cannot pass {1} characters.")]
        [MinLength(3, ErrorMessage = "{0} cannot be less than {1} characters.")]
        public string Name { get; set; }
        [DisplayName("Surname")]
        [MaxLength(30, ErrorMessage = "{0} cannot pass {1} characters.")]
        [MinLength(3, ErrorMessage = "{0} cannot be less than {1} characters.")]
        public string Surname { get; set; }
    }
}
