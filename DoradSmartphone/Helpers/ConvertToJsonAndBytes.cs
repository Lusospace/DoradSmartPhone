using System.Text;
using System.Text.Json;

namespace DoradSmartphone.Helpers
{
    public static class ConvertToJsonAndBytes
    {
        public static byte[] Convert<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            return byteArray;
        }
    }
}
