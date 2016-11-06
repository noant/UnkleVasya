using HierarchicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;

namespace VkCaptchaSolver
{
    public class UsersSolvers
    {
        public static void InitializeUsersIds(VkApi vk)
        {
            var hObject = HierarchicalObject.FromFile(Constants.UsersToSendFilePath);
            var usersIds = new List<long>();

            for (int i = 0; hObject[i] is string; i++)
            {
                var userId = VkUtils.GetUserIdByUriName(vk, (string)hObject[i]);
                usersIds.Add(userId.Value);
            }

            Current = new UsersSolvers()
            {
                UsersIds = usersIds.ToArray()
            };
        }
        public static UsersSolvers Current { get; private set; }

        private Random _rand = new Random();
        public long[] UsersIds { get; private set; }

        public long GetRandUserId()
        {
            return UsersIds[_rand.Next(0, UsersIds.Length)];
        }
    }
}
