using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace VK_UnkleVasya.Commands
{
    public class SetIntervalValueCommand : Command
    {
        public override bool IsIt(string query)
        {
            return ExtractValue(query) > 0;
        }

        public override void Execute(VkApi vk, Message message, string sourceQuery)
        {
            var targetQuery = sourceQuery.Substring(0, sourceQuery.Length - ExtractQuery(sourceQuery).Length);

            var value = ExtractValue(targetQuery);

            if (value < 1)
                VkNet.VkUtils.SendMessage(vk, message, StringConstants.Dialog_IntervalCannotBeLessThan1);
            else
            {
                var session = DialogSettings.GetSession(vk, message);
                session.IntervalDispatchingValue = value;
                session.StartIntervalDispatching();
            }
        }

        public decimal ExtractValue(string targetQuery)
        {
            return (Extract(targetQuery) ?? new ExtractionResult() { Value = 0 }).Value;
        }

        public override string ExtractQuery(string query)
        {
            var extracted = Extract(query);
            if (extracted == null) return query;
            return query.Substring(extracted.Query.Length);
        }

        public ExtractionResult Extract(string query)
        {
            query = query.ToLower();
            var splitter = "*".ToCharArray();
            foreach (var commandTemplate in CommandData)
            {
                var templates = commandTemplate.Split(splitter);
                if (query.StartsWith(templates[0]))
                {
                    var betweenValue = Utils.GetValueBetween(query, templates[0], templates[1]);
                    var value = Utils.ConvertToDecimal(betweenValue);
                    if (value != null)
                        return new ExtractionResult()
                        {
                            QueryPart1 = templates[0],
                            QueryPart2 = templates[1],
                            Value = value.Value
                        };
                }
            }
            return null;
        }

        public class ExtractionResult
        {
            public decimal Value { get; set; }
            public string QueryPart1 { get; set; }
            public string QueryPart2 { get; set; }
            public string Query
            {
                get
                {
                    return QueryPart1 + Value.ToString() + QueryPart2;
                }
            }
        }
    }
}
