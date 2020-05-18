using System;

namespace Valk.Networking {
    static class Commands {
        private static string[] commands = {
            "help",
            "exit"
        };

        public static void Help(string[] args){
            Logger.Log($"Commands:\n - {String.Join("\n - ", commands)}");
        }

        public static void Exit(string[] args){
            var server = Program.Server;

            Logger.Log("Exiting..");
            server.Stop();
            Environment.Exit(0);
        }
    }
}