using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Models
{
    public class ChirpMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime PostTime { get; set; }
        public ChirpUser User { get; set; }
    }
}
