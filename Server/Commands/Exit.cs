using System;

namespace Valk.Networking {
    public class Exit : Commands {
        public override void Run(string[] args){
            var server = Program.Server;

            Logger.Log("Exiting..");
            server.Stop();
            Environment.Exit(0);
        }
    }
}