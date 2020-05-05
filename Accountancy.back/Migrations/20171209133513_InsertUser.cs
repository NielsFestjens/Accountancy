using Accountancy.Domain.Auth;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Util;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accountancy.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20171209133513_InsertUser")]
    public class InsertUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = DatabaseContextFacory.Create())
            {
                context.Add(new User
                {
                    Username = "w",
                    Password = "Kouq+NI8EjQ6RAdu+CrL53BG1XEGLBJTRyyx9/CFQIIFSvCDPkYm9+fcKV3nl4t3/X5yZpb15UsCGmcPQ4fDqVs+MTgqjpJA",
                    PasswordSalt = "5B3E31382A8E9240".HexToByteArray()
                });

                context.SaveChanges();
            }
        }
    }
}