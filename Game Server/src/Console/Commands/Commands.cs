namespace Valk.Networking {
    public abstract class Commands {
        public virtual void Run(string[] args){
            Console.Log("Unimplemented command.");
        }
    }
}