using labo.signalr.api.Controllers;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace labo.signalr.api.MatchHub
{
    public class MatchHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            await Clients.Caller.SendAsync("TaskList");
        }

        public async Task AjouterUneTache(string value)
        {
            
            await Clients.Caller.SendAsync("TaskList");
        }
    }
}
