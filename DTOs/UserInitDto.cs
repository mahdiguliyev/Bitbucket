using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.DTOs
{
    public class UserInitDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
