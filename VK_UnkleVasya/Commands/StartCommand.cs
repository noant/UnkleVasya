using System.Linq;
using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    class StartCommand : Command
    {
        public override void Execute(VkApi vk, Message message, string sourceQuery)
        {
            var query = ExtractQuery(sourceQuery);
            var command = CommandUtils.AllCommands.FirstOrDefault(x => x.IsIt(query));
            if (command == null)
                VkUtils.SendImage(vk, message, VkUtils.GetNextPicture(vk), RandomMessages.GetNext());
            else command.Execute(vk, message, query);
        }
    }
}
