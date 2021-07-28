using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infor.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
    }
}