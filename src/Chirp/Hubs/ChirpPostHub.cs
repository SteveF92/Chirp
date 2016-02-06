using Microsoft.AspNet.SignalR;

namespace Chirp.Hubs
{
    public class ChirpPostHub : Hub
    {
        public void RefreshChirps(string receivedString)
        {
            var responseString = string.Empty;

            responseString = "";
            Clients.All.refreshChirps(responseString);
        }
    }
}
