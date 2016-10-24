using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public class StopIntervalDispatchingCommand : Command
    {
        public override void Execute(VkApi vk, Message message, string sourceQuery)
        {
            DialogSettings.GetSession(vk, message).StopIntervalDispatching();
        }
    }
}
