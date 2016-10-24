using System.Linq;
using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public class StartCommand : Command
    {
        public override void Execute(VkApi vk, Message message, string sourceQuery)
        {
            var query = ExtractQuery(sourceQuery);
            var command = CommandUtils.AllCommands.FirstOrDefault(x => x.IsIt(query));
            if (command == null)
            {
                var dialog = DialogSettings.GetSession(vk, message);
                var photoAndText = VkUtils.GetNextPictureAndMessageForDialog_Ad(vk, dialog);
                VkUtils.SendImage(vk, message, photoAndText.Key, photoAndText.Value);
            }
            else command.Execute(vk, message, query);
        }
    }
}
