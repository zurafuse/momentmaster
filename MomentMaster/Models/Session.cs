using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomentMaster.Models
{
    public class Session
    {
        public Session(string key, string user, DateTime created)
        {
            this.Key = key;
            this.User = user;
            this.Created = created;
        }
        public string User { get; set; }
        public DateTime Created { get; set; }
        public string Key { get; set; }
    }
}
