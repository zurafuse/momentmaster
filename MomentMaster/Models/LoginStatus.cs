using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomentMaster.Models
{
    public static class LoginStatus
    {
        public static List<Session> Keys;

        static LoginStatus() {
            Keys = new List<Session>();
        }

        public static bool UserValidated(string thisSession)
        {
            bool isAuthorized = false;
            foreach (Session key in Keys)
            {
                if (key.Key == thisSession)
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
                if (Keys[i].Key == thisSession)
                {
                    keyToRemove = i;
                }
            }
            if (keyToRemove >= 0)
            {
                Keys.RemoveAt(keyToRemove);
            }
            ExpireSessions(thisSession);
        }
        public static void ExpireSessions(string thisSession)
        {
            int keyToRemove = -1;
            for (var i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].Created >= DateTime.Now.AddMinutes(100))
                {
                    keyToRemove = i;
                }
            }
            if (keyToRemove >= 0)
            {
                Keys.RemoveAt(keyToRemove);
                ExpireSessions(thisSession);
            }
        }
    }
}
