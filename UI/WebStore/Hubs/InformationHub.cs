using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Hubs
{
    public class InformationHub : Hub
    {
        public async Task Send(string Message) => await Clients.All.SendAsync("Send", Message);
    }
}
