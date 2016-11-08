using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;

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
                var result = Utils.GetWhile(() => VkUtils.GetNextPictureAndMessageForDialog_Ad(vk, dialog), (res) => res.Key != null, 10);
                VkNet.VkUtils.SendImage(vk, message, result.Key, result.Value);
            }
            else command.Execute(vk, message, query);
        }
    }
}
