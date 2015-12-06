using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Models
{
    public class ChirpRepository : IChirpRepository
    {
        private ChirpContext m_context;
        private ILogger<ChirpRepository> m_logger;

        public ChirpRepository(ChirpContext a_context, ILogger<ChirpRepository> a_logger)
        {
            m_context = a_context;
            m_logger = a_logger;
        }

        public void AddMessage(ChirpMessage newMessage)
        {
            m_context.Add(newMessage);
        }

        public IEnumerable<ChirpMessage> GetAllMessages()
        {
            try
            {
                return m_context.ChirpMessages.OrderBy(t => t.PostTime).ToList();
            }
            catch (Exception ex)
            {
                m_logger.LogError("Could not get trips from database", ex);
                return null;
            }
            
        }

        public IEnumerable<ChirpMessage> GetAllMessagesByUserId(string a_userId)
        {
            try
            {
                return m_context.ChirpMessages.Where(t => (t.User.Id == a_userId)).OrderBy(t => t.PostTime).ToList();
            }
            catch (Exception ex)
            {
                m_logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public bool SaveAll()
        {
            return (m_context.SaveChanges() > 0);
        }
    }
}
