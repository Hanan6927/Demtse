﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demጽ.Models.Users
{
    public class UserCreationDto
    {
        [Required]
        public String Password { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public String UserName { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public String ProfilePicture { get; set; }
    }
}
