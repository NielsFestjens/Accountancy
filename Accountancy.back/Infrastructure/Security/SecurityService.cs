using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Accountancy.Domain.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Accountancy.Infrastructure.Security
{
    public interface ISecurityService
    {
        byte[] GetSalt();
        string CalculateHash(string plainText, byte[] saltBytes);
        Task SignIn(HttpContext httpContext, User user);
        int GetUserId(ClaimsPrincipal user);
    }

    public class SecurityService : ISecurityService
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly HashAlgorithm _hashAlgorithm;

        public SecurityService(RandomNumberGenerator randomNumberGenerator, HashAlgorithm hashAlgorithm)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _hashAlgorithm = hashAlgorithm;
        }

        public byte[] GetSalt()
        {
            var saltBytes = new byte[8];
            _randomNumberGenerator.GetBytes(saltBytes);
            return saltBytes;
        }

        public string CalculateHash(string plainText, byte[] saltBytes)
        {
            var inputBytes = Combine(Encoding.UTF8.GetBytes(plainText), saltBytes);
            var hashedBytes = _hashAlgorithm.ComputeHash(inputBytes);
            var hashWithSaltBytes = Combine(hashedBytes, saltBytes);
            return Convert.ToBase64String(hashWithSaltBytes);
        }

        public async Task SignIn(HttpContext httpContext, User user)
        {
            var identity = new ClaimsIdentity("Custom");
            identity.AddClaim(new Claim("Id", user.Id.ToString()));
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync("DatScheme", principal);
        }

        public int GetUserId(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.Single(x => x.Type == "Id").Value);
        }

        private static T[] Combine<T>(IReadOnlyList<T> a, IReadOnlyList<T> b)
        {
            var output = new T[a.Count + b.Count];

            for (var i = 0; i < a.Count; i++)
                output[i] = a[i];

            for (var i = 0; i < b.Count; i++)
                output[a.Count + i] = b[i];

            return output;
        }
    }
}