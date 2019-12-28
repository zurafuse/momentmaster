using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MomentMaster.Models
{
    public class Hours
    {
        public Hours() {
            this.User = new User();
        }

        public double GetHours()
        {
            try
            {
                TimeSpan timeGap = EndTime - StartTime;
                return timeGap.TotalHours;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return 0;
            }
        }
        public User User;

        [Key]
        public int Hour_Id { get; set; }
        [Column(TypeName = "int")]
        public int Project_Id { get; set; }
        [Column(TypeName = "int")]
        public int User_Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }

    }
}
