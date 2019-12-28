using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MomentMaster.Models
{
    public class TimeObject
    {
        private bool tasksListed;
        public List<Comment> Comments { get; set; }
        public List<Hours> Hours { get; set; }
        public List<TimeObject> SubTasks;
        public List<int> projectIdList;
        public User User { get; set; }
        public double Spent;
        public double RemainingHours;

        public TimeObject()
        {
            this.tasksListed = false;
            this.Parent_Id = -1;
            this.Tier = 1;
            this.User_Id = 0;
            this.Spent = 0;
            this.RemainingHours = 0;
            this.SubTasks = new List<TimeObject>();
            this.Comments = new List<Comment>();
            this.Hours = new List<Hours>();
            this.projectIdList = new List<int>();
            this.UpdateTier();
        }

        public double GetRemainingHours()
        {
            try
            {
                return this.HoursBudgeted - this.Spent;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return 0;
            }
        }

        public void PopulateHours(List<User> userList, List<Hours> hourList, List<TimeObject> projectList, int thisProject_id)
        {
            try
            {
                bool isDuplicate = false;
                if (this.tasksListed == false)
                {
                    for (var i = 0; i < hourList.Count; i++)
                    {
                        if (hourList[i].Project_Id == this.Project_Id)
                        {
                            for (var j = 0; j < userList.Count; j++)
                            {
                                if (hourList[i].User_Id == userList[j].User_Id)
                                {
                                    hourList[i].User = userList[j];
                                }
                            }
                        }
                    }
                    this.tasksListed = true;
                }
                for (var i = 0; i < this.projectIdList.Count; i++)
                {
                    if (this.projectIdList[i] == thisProject_id)
                    {
                        isDuplicate = true;
                    }
                }
                if (isDuplicate == false)
                {
                    for (var i = 0; i < hourList.Count; i++)
                    {
                        if (hourList[i].Project_Id == thisProject_id)
                        {
                            this.Spent += hourList[i].GetHours();
                            this.projectIdList.Add(thisProject_id);
                        }
                    }
                }
                for (var i = 0; i < projectList.Count(); i++)
                {
                    if (projectList[i].Parent_Id == thisProject_id)
                    {
                        this.PopulateHours(userList, hourList, projectList, projectList[i].Project_Id);
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


        public void UpdateTier() {
            try
            {
                this.Tier = this.Parent_Id == -1 ? 1 : (this.Type == "Time Period" ? 2 : 3);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void AddSubTask(TimeObject subTask)
        {
            try
            {
                this.SubTasks.Add(subTask);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void SetUser(User user)
        {
            try
            {
                this.User = user;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        [Key]
        public int Project_Id { get; set; }
        [Column(TypeName = "int")]
        public int Parent_Id { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Type { get; set; }
        [Column(TypeName = "int")]
        public int Tier { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }
        [Column(TypeName = "int")]
        public int User_Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "int")]
        public int OriginalEstimate { get; set; }
        [Column(TypeName = "int")]
        public int HoursBudgeted { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }


    }
}
