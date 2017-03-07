using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeCinema.Entities;

namespace HomeCinema.Data
{
    public class UserRoleConfiguration : EntityBaseConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            Property(r => r.UserId).IsRequired();
            Property(r => r.RoleId).IsRequired();
        }
    }
}
