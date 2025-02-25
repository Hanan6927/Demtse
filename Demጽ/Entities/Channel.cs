﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demጽ.Entities
{
    public class Channel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public String Id { get; set; }
        [Required]
        [MaxLength(30)]
        public String Name { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        public String ProfilePicture { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual User Owner { get; set; }
        [Required]
        public String UserId { get; set; }
        //[Required]
        //public String PictureName { get; set; }
        public virtual ICollection<Audio> Audios { get; set; } = new List<Audio>();
        public virtual ICollection<Subscribe> Subscribtion { get; set; } = new List<Subscribe>();


    }
}
