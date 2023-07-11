using DoradSmartphone.DTO;
using System.Text;

namespace DoradSmartphone.Helpers
{
    public static class ConvertToJsonAndBytes
    {
        public static byte[] Convert(GlassDTO glassDTO)
        {

            string json = System.Text.Json.JsonSerializer.Serialize(glassDTO);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            return byteArray;
        }
    }
}
