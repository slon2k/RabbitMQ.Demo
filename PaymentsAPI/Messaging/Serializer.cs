using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentsAPI.Messaging
{
    public static class Serializer
    {
        public static byte[] Serialize(this object obj)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
        }

        public static T Deserialize<T>(this byte[] bytes) where T : class
        {
            if (bytes.Length == 0)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }
    }
}
