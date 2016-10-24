using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public abstract class Command
    {
        public virtual string[] CommandData
        {
            get
            {
                return CommandUtils.GetCommandData(this.GetType());
            }
        }
        public virtual bool IsIt(string query)
        {
            return CommandUtils.IsQueryStartWithAny(CommandData, query);
        }
        public virtual string ExtractQuery(string query)
        {
            return CommandUtils.ExtractQueryStandart(CommandData, query);
        }
        public abstract void Execute(VkApi vk, Message message, string sourceQuery);
    }
}
