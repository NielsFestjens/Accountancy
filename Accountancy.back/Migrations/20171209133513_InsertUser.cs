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
                    Password = "n25Pg9qjde1ABd1MRr3bPK7aLsfC4xRpnwa5ha0QMvHdXggkqUaVoAfaI976smRHlDgLe2ZXcgv7eWk0bK0mzIjymtuZrN6E",
                    PasswordSalt = "88F29ADB99ACDE84".HexToByteArray()
                });
            }
        }
    }
}