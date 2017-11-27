namespace PhotoShare.Client.Core
{
    using PhotoShare.Client.Core.Commands;
    using System;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            string command = commandParameters[0];
            string result = string.Empty;

            switch (command.ToLower())
            {
                case "registeruser":
                    result = RegisterUserCommand.Execute(commandParameters);
                    break;
                case "addtown":
                    result = AddTownCommand.Execute(commandParameters);
                    break;
                case "modifyuser":
                    result = ModifyUserCommand.Execute(commandParameters);
                    break;
                case "deleteuser":
                    result = DeleteUser.Execute(commandParameters);
                    break;
                case "addtag":
                    result = AddTagCommand.Execute(commandParameters);
                    break;
                case "createalbum":
                    result = CreateAlbumCommand.Execute(commandParameters);
                    break;
                case "addfriend":
                    result = AddFriendCommand.Execute(commandParameters);
                    break;
                case "acceptfriend":
                    result = AcceptFriendCommand.Execute(commandParameters);
                    break;
                case "printfriendslist":
                    result = PrintFriendsListCommand.Execute(commandParameters);
                    break;
                case "addtagto":
                    result = AddTagToCommand.Execute(commandParameters);
                    break;
                case "sharealbum":
                    result = ShareAlbumCommand.Execute(commandParameters);
                    break;
                case "uploadpicture":
                    result = UploadPictureCommand.Execute(commandParameters);
                    break;
                case "login":
                    result = LoginCommand.Execute(commandParameters);
                    break;
                case "logout":
                    result = LogoutCommand.Execute(commandParameters);
                    break;
                case "exit":
                    ExitCommand.Execute();
                    break;
                default:
                    throw new InvalidOperationException($"Command {command} not valid!");
            }

            return result;
        }
    }
}
