using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VK_UnkleVasya.Commands
{
    public static class CommandUtils
    {
        private static readonly Dictionary<Type, string[]> CommandsData = new Dictionary<Type, string[]>();

        private static string[] LoadCommandData(Type type)
        {
            var filename = StringConstants.CommandsFolder + type.Name + ".xml";
            return Utils.LoadStringData(filename);
        }

        public static string[] GetCommandData(Type type)
        {
            if (!CommandsData.ContainsKey(type))
                CommandsData.Add(type, LoadCommandData(type));
            return CommandsData[type];
        }

        public static string ExtractQueryStandart(string[] allKeys, string query)
        {
            return (query = query.ToLower().Trim()).Substring(allKeys.Single(key => query.StartsWith(key)).Length).Trim();
        }

        public static bool IsQueryStartWithAny(string[] allKeys, string query)
        {
            query = query.Trim().ToLower();
            return !string.IsNullOrEmpty(query) && allKeys.Any(x => query.StartsWith(x));
        }

        private static Command[] GetAllCommands()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(
                x => x.BaseType.Equals(typeof(Command)) &&
                     !x.GetConstructors().Any(z => z.GetParameters().Any()))
                .Select(x => (Command)x.GetConstructor(new Type[0]).Invoke(new object[0]))
                .ToArray();
        }

        private static Command[] _commands;
        public static Command[] AllCommands
        {
            get
            {
                if (_commands == null)
                    _commands = GetAllCommands();
                return _commands;
            }
        }

        private static Command _startCommand;
        public static Command StartCommand
        {
            get
            {
                if (_startCommand == null)
                    _startCommand = AllCommands.Single(x => x is StartCommand);
                return _startCommand;
            }
        }
    }
}
