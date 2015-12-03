using System;
using System.Linq;

namespace Chirp.Models
{
    public class ChirpContextSeedData
    {
        private ChirpContext m_context;

        public ChirpContextSeedData(ChirpContext a_context)
        {
            m_context = a_context;
        }

        public void EnsureSeedData()
        {
            if (!m_context.ChirpMessages.Any())
            {
                //Add new data
                var chirpMessage = new ChirpMessage()
                {
                    Message = "First message! ChirpChirp!",
                    PostTime = DateTime.UtcNow,
                    UserId = 12
                };

                m_context.ChirpMessages.Add(chirpMessage);

                m_context.SaveChanges();
            }
        }
    }

}