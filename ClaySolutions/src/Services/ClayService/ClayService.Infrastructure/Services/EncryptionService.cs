using ClayService.Application.Contracts.Infrastructure;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ClayService.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string pass)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(pass);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}
