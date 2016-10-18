using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public abstract class Command
    {
        public virtual bool IsIt(string query)
        {
            return CommandUtils.IsQueryStartWithAny(CommandUtils.GetCommandData(this.GetType()), query);
        }
        public virtual string ExtractQuery(string query)
        {
            return CommandUtils.ExtractQueryStandart(CommandUtils.GetCommandData(this.GetType()), query);
        }
        public abstract void Execute(VkApi vk, Message message, string sourceQuery);
    }
}
