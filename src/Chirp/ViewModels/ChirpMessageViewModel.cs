using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.ViewModels
{
    public class ChirpMessageViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Message { get; set; }
        public DateTime PostTime { get; set; }
        public ChirpUserViewModel User { get; set; }
    }
}
