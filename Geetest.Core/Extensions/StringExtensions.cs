using System;
using System.Security.Cryptography;
using System.Text;

namespace Geetest.Core
{
    public static class StringExtensions
    {
        public static string Md5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            }
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}