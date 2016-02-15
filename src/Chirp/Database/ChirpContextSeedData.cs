using Chirp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Database
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

                var userRes = await m_userManager.CreateAsync(newUser, "qwertyui");
            }

            m_context.SaveChanges();

            if (!m_context.ChirpPosts.Any())
            {
                //Add new data
                var chirpPost = new ChirpPost()
                {
                    Message = "First message! ChirpChirp!",
                    PostTime = DateTimeOffset.UtcNow,
                    User = await m_userManager.FindByEmailAsync("chase.huxley@chirp.com")
                };

                m_context.ChirpPosts.Add(chirpPost);

                m_context.SaveChanges();
            }
        }
    }

}