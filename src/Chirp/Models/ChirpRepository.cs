using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;


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

        public void AddPost(ChirpPost newPost)
        {
            m_context.Add(newPost);
        }

        public IEnumerable<ChirpPost> GetAllPosts()
        {
            try
            {
                return m_context.ChirpPosts.Include(t => t.User).OrderBy(t => t.PostTime).ToList();
            }
            catch (Exception ex)
            {
                m_logger.LogError("Could not get trips from database", ex);
                return null;
            }
            
        }

        public IEnumerable<ChirpPost> GetAllPostsByUserId(string a_userId)
        {
            try
            {
                return m_context.ChirpPosts.Where(t => (t.User.Id == a_userId)).OrderBy(t => t.PostTime).ToList();
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
