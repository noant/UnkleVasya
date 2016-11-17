using HierarchicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VK_UnkleVasya
{
    [Serializable]
    public class TagsContainer
    {
        static TagsContainer()
        {
            HierarchicalObjectCrutch.Register(typeof(TagsContainer));
            HierarchicalObjectCrutch.Register(typeof(TagSynonyms));

            Current = HierarchicalObject.FromFile(StringConstants.TagSynonymsFileName)[0];
        }

        public TagSynonyms[] _tagsSynonyms;
        public TagSynonyms[] TagsSynonyms
        {
            get
            {
                return _tagsSynonyms;
            }
            set
            {
                _tagsSynonyms = value;
                Dictionary = _tagsSynonyms.ToDictionary(x =>
                    x.Tag.ToLower(),
                    x => x.Synonyms.Select(z => z.ToLower()).ToArray()
                );
            }
        }

        [XmlIgnore]
        public Dictionary<string, string[]> Dictionary { get; private set; }

        public static TagsContainer Current { get; private set; }
    }
}
