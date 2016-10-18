using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    class StartCommand : ICommand
    {
        public bool IsIt(string query)
        {
            return CommandUtils.IsQueryStartWithAny(CommandUtils.GetCommandData<StartCommand>(), query);
        }

        public void Execute(VkApi vk, Message message, string sourceQuery)
        {
            var query = ExtractQuery(sourceQuery);
            var command = CommandUtils.AllCommands.FirstOrDefault(x => x.IsIt(query));
            if (command == null)
                VkUtils.SendImage(vk, message, VkUtils.GetNextPicture(vk), RandomMessages.GetNext());
            else command.Execute(vk, message, query);
        }

        public string ExtractQuery(string query)
        {
            return CommandUtils.ExtractQueryStandart(CommandUtils.GetCommandData<StartCommand>(), query);
        }
    }
}
