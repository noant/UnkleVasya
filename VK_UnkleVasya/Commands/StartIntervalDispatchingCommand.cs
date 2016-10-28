using VkNet;
using VkNet.Model;
using System.Linq;

namespace VK_UnkleVasya.Commands
{
    public class StartIntervalDispatchingCommand : Command
    {
        public override void Execute(VkApi vk, Message message, string sourceQuery)
        {
            var setIntervalCommand = CommandUtils.AllCommands.Single(x => x is SetIntervalValueCommand);
            var extractedQuery = this.ExtractQuery(sourceQuery);
            if (setIntervalCommand.IsIt(extractedQuery))
                setIntervalCommand.Execute(vk, message, extractedQuery);
            else
            {
                var session = DialogSettings.GetSession(vk, message);
                session.IntervalDispatchingValue = 1;
                session.StartIntervalDispatching();
            }
        }
    }
}
