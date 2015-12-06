using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Models
{
    public class ChirpContextSeedData
    {
        private ChirpContext m_context;
        private UserManager<ChirpUser> m_userManager;

        public ChirpContextSeedData(ChirpContext a_context, UserManager<ChirpUser> a_userManager)
        {
            m_context = a_context;
            m_userManager = a_userManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            var userFound = await m_userManager.FindByEmailAsync("chase.huxley@chirp.com");
            if (userFound == null)
            {
                var newUser = new ChirpUser()
                {
                    UserName = "chasehuxley",
                    Email = "chase.huxley@chirp.com"
                };

                var userRes = await m_userManager.CreateAsync(newUser, "Myp@55word");
            }

            if (!m_context.ChirpMessages.Any())
            {
                //Add new data
                var chirpMessage = new ChirpMessage()
                {
                    Message = "First message! ChirpChirp!",
                    PostTime = DateTime.UtcNow,
                    User = await m_userManager.FindByEmailAsync("chase.huxley@chirp.com")
                };

                m_context.ChirpMessages.Add(chirpMessage);

                m_context.SaveChanges();
            }
        }
    }

}