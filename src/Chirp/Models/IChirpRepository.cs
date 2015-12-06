using System.Collections.Generic;

namespace Chirp.Models
{
    public interface IChirpRepository
    {
        IEnumerable<ChirpMessage> GetAllMessages();
        IEnumerable<ChirpMessage> GetAllMessagesByUserId(string a_userId);
        void AddMessage(ChirpMessage newMessage);
        bool SaveAll();
    }
}