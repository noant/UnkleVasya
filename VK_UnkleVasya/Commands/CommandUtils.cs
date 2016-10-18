using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VK_UnkleVasya.Commands
{
    public static class CommandUtils
    {
        private static readonly Dictionary<Type,string[]> CommandsData = new Dictionary<Type, string[]>();

        private static string[] LoadCommandData<T>() where T : ICommand
        {
            var filename = StringConstants.CommandsFolder + typeof(T).Name + ".xml";
            return Utils.LoadStringData(filename);
        }

        public static string[] GetCommandData<T>() where T : ICommand
        {
            if (!CommandsData.ContainsKey(typeof (T)))
                CommandsData.Add(typeof (T), LoadCommandData<T>());
            return CommandsData[typeof (T)];
        }

        public static string ExtractQueryStandart(string[] allKeys, string query)
        {
            return (query = query.ToLower().Trim()).Substring(allKeys.Single(key => query.StartsWith(key)).Length - 1).Trim();
        }

        public static bool IsQueryStartWithAny(string[] allKeys, string query)
        {
            query = query.Trim().ToLower();
            return !string.IsNullOrEmpty(query) && allKeys.Any(x => query.StartsWith(x));
        }
        
        private static ICommand[] GetAllCommands()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(
                x => x.GetInterfaces().Contains(typeof (ICommand)) &&
                     !x.IsAbstract &&
                     !x.GetConstructors().Any(z => z.GetParameters().Any()))
                .Select(x => (ICommand) x.GetConstructor(null).Invoke(null))
                .ToArray();
        }

        private static ICommand[] _commands;
        public static ICommand[] AllCommands
        {
            get
            {
                if (_commands == null)
                    _commands = GetAllCommands();
                return _commands;
            }
        }
    }
}
