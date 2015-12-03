using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Models
{
    public class ChirpRepository : IChirpRepository
    {
        private ChirpContext m_context;

        public ChirpRepository(ChirpContext a_context)
        {
            m_context = a_context;
        }

        public IEnumerable<ChirpMessage> GetAllMessages()
        {
            return m_context.ChirpMessages.OrderBy(t => t.PostTime).ToList();
        }

        public IEnumerable<ChirpMessage> GetAllMessagesByUserId(int a_userId)
        {
            return m_context.ChirpMessages.Where(t => (t.UserId == a_userId)).OrderBy(t => t.PostTime).ToList();
        }
    }
}
