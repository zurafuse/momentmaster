using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MomentMaster.Models
{
    public class Comment
    {

        [Key]
        public int Comment_Id { get; set; }
        [Column(TypeName = "int")]
        public int Project_Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Content { get; set; }
    }
}
