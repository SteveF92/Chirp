using Microsoft.AspNet.SignalR;

namespace Chirp.Hubs
{
    public class ChirpPostHub : Hub
    {
        public void RefreshChirps()
        {
            Clients.All.refreshChirps();
        }
    }
}
