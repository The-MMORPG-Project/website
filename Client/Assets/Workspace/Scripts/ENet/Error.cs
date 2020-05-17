namespace Valk.Networking
{
    class Error
    {
        public static string ReadError(ErrorType type)
        {
            var message = "";
            if (type == ErrorType.AccountCreateNameAlreadyRegistered)
                message = "Account name already registered.";
            if (type == ErrorType.AccountLoginDoesNotExist)
                message = "Account login does not exist.";
            if (type == ErrorType.AccountLoginWrongPassword)
                message = "Wrong password.";
            return message;
        }
    }
}