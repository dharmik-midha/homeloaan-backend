using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DBContext
{
    public class DatabaseContext : IdentityDbContext<IdentityUser>
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Advisor> Advisor { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<Collateral> Collateral { get; set; }
        public DbSet<LoanCollateral> LoanCollateral { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<LoanState> LoanState { get; set; }




    }
}
