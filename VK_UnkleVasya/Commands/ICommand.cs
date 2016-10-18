using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public interface ICommand
    {
        bool IsIt(string query);
        void Execute(VkApi vk, Message message, string sourceQuery);
        string ExtractQuery(string query);
    }
}
