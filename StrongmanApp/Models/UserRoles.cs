using System;
using System.Collections.Generic;
namespace StrongmanApp.Models
{
   public partial class UserRoles { 
        //public string UserId { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId {  get; set; } 
        public string UserName { get; set; }
        public string RoleName { get; set; }

       
    }
}
