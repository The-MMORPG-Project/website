namespace Valk.Networking
{
    public class Exit : Commands
    {
        public override void Run(string[] args)
        {
            Program.Server.Stop();
            System.Environment.Exit(0);
        }
    }
}