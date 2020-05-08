class Packet
{
    public enum Type
    {
        HeartBeat = 0,
        ClientCreateAccount = 1,
        ServerCreateAccountDenied = 2,
        ServerCreateAccountAccepted = 3,
        ClientLoginAccount = 4,
        ServerLoginDenied = 5,
        ServerLoginAccepted = 6,
        ClientPositionUpdate = 7,
        ServerPositionUpdate = 8
    }
}