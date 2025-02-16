using labo.signalr.api.Controllers;
using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace labo.signalr.api.MatchHub
{
    public class MatchHub : Hub
    {

        public static int connectionCount = 0;
        private readonly ApplicationDbContext _context;

        public MatchHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            connectionCount++;
            await Clients.Caller.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());
            await Clients.All.SendAsync("UserCount", connectionCount);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
            // TODO: Ajouter votre logique
            connectionCount--;
            await Clients.All.SendAsync("UserCount");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AjouterUneTache(string value)
        {
            UselessTask uselessTask = new UselessTask()
            {
                Completed = false,
                Text = value
            };
            await _context.UselessTasks.AddAsync(uselessTask);
            await _context.SaveChangesAsync();


            await Clients.All.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());
        }

        public async Task Complete(int id)
        {
            UselessTask? task = await _context.UselessTasks.FindAsync(id);
            if (task != null) 
            {
                task.Completed = true;
                _context.Update(task);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());
            }

        }
    }
}
