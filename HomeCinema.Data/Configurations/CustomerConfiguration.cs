﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeCinema.Entities;

namespace HomeCinema.Data
{
    public class CustomerConfiguration : EntityBaseConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            Property(u => u.LastName).IsRequired().HasMaxLength(100);
            Property(u => u.IdentityCard).IsRequired().HasMaxLength(50);
            Property(u => u.UniqueKey).IsRequired();
            Property(u => u.Mobile).IsRequired().HasMaxLength(50);
            Property(u => u.Email).IsRequired().HasMaxLength(200);
            Property(u => u.DateOfBirth).IsRequired();
            Property(u => u.RegistrationDate).IsRequired();
        }
    }
}
