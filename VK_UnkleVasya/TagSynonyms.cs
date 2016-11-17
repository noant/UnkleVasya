using System;

namespace VK_UnkleVasya
{
    [Serializable]
    public class TagSynonyms
    {
        public string Tag { get; set; }
        public string[] Synonyms { get; set; }
    }
}