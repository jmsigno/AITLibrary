using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AITLibrary.Models
{
    public class UserDTO
    {
        public UserDTO() { }

        public UserDTO(string username, string pw, int level, string email)
        {
            this.UserName = username;
            this.Password = pw;
            this.UserLevel = level;
            this.UserEmail = email;
        }

        public int UID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserLevel { get; set; }
        public string UserEmail { get; set; }
    }
}