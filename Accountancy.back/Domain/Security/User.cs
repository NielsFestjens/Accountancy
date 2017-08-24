using Accountancy.Infrastructure.Database;

namespace Accountancy.Domain.Security
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}