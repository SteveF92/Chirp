using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Chirp.Models
{
    public class ChirpUser : IdentityUser
    {
        public List<ChirpMessage> ChirpMessages;
    }
}