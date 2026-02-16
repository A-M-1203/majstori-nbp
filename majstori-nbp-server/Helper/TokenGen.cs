using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace majstori_nbp_server.Helper
{
    public static class TokenGen
    {
        public static string NewToken(int bytes = 32)
        {
            var data = RandomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(data)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
    }
}