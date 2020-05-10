using System;
using System.Collections.Generic;

using ENet;

namespace Valk.Networking
{
    class ClientAddEventArgs : EventArgs 
    {
        public List<Client> Clients { get; set; }
        public Event netEvent {get; }
    }

    class Clients
    {
        public event EventHandler<ClientAddEventArgs> Add;

        public virtual void OnAdd(ClientAddEventArgs e) 
        {
            Add?.Invoke(this, e);
        }

        private List<Client> clients;
        public int Count { get; set; }

        public Clients() 
        {
            clients = new List<Client>();
        }

        /*public void Add(Event netEvent) 
        {
            clients.Add(new Client(netEvent.Peer));
            Count = clients.Count;
        }

        public void Remove(Event netEvent)
        {
            clients.Remove(clients.Find(x => x.ID.Equals(netEvent.Peer.ID)));
            Count = clients.Count;
        }*/
    }
}