using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Infor.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base("name=connStr")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}