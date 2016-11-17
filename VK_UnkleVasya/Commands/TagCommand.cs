using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public class TagCommand : Command
    {
        public override void Execute(VkApi vk, Message message, string sourceQuery)
        {
            var targetTag = GetTargetTag(sourceQuery);
            var tagAlbums = EroRepository.TagsAlbums[targetTag];
            var dialog = DialogSettings.GetSession(vk, message);
            var result = Utils.GetWhile(() => VkUtils.GetNextPictureAndMessageForDialog_Ad(vk, dialog, tagAlbums[Utils.GetNextRandom(0, tagAlbums.Count - 1)]), (res) => res.Key != null, 10);
            VkNet.VkUtils.SendImage(vk, message, result.Key, result.Value);
        }

        public override string ExtractQuery(string query)
        {
            return string.Empty;
        }

        public override bool IsIt(string query)
        {
            return !string.IsNullOrEmpty(GetTargetTag(query));
        }

        public string GetTargetTag(string query)
        {
            query = query.ToLower().Trim();
            return EroRepository.TagsAlbums.Keys.FirstOrDefault(x => query.Contains(x) || x.Contains(query));
        }
    }
}
