using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomentMaster.Models
{
    public static class LoginStatus
    {
        public static List<string> Keys;

        static LoginStatus() {
            Keys = new List<string>();
        }

        public static bool UserValidated(string thisSession)
        {
            bool isAuthorized = false;
            foreach (string key in Keys)
            {
                if (key == thisSession)
                {
                    isAuthorized = true;
                    break;
                }
            }
            return isAuthorized;
        }
        public static void LogOff(string thisSession)
        {
            int keyToRemove = -1;
            for (var i = 0; i < Keys.Count; i++)
            {
                if (Keys[i] == thisSession)
                {
                    keyToRemove = i;
                }
            }
            if (keyToRemove >= 0)
            {
                Keys.RemoveAt(keyToRemove);
            }
        }
    }
}
