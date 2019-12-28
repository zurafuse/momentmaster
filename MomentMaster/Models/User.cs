using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomentMaster.Models
{
    public class User
    {
        public User()
        {
            this.Hours = 0;
        }

        public double Hours;

        [Key]
        public int User_Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string UserName { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string Password { get; set; }
        [Column(TypeName = "nvarchar(60)")]
        public string Department { get; set; }

    }
}
