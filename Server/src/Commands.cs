namespace Valk.Networking {
    public abstract class Commands {
        public virtual void Run(string[] args){
            Logger.Log("Unimplemented command.");
        }
    }
}