﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demጽ.Entities
{
    public class Subscribe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public String Id { get; set; }
        [Required]
        public bool Nofication { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public String UserId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Channel Channel { get; set; }
        public String ChannelId { get; set; }

    }
}
