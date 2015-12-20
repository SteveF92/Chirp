using System.Collections.Generic;

namespace Chirp.Models
{
    public interface IChirpRepository
    {
        IEnumerable<ChirpPost> GetAllPosts();
        IEnumerable<ChirpPost> GetAllPostsByUserId(string a_userId);
        void AddPost(ChirpPost newMessage);
        bool SaveAll();
    }
}