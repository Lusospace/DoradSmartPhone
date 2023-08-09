using System.Text;
using System.Text.Json;

namespace DoradSmartphone.Helpers
{
    public static class ConvertToJsonAndBytes
    {
        private static readonly byte[] Header = Encoding.UTF8.GetBytes("DoradHeader");
        private static readonly byte[] Footer = Encoding.UTF8.GetBytes("DoradFooter");

        /// <summary>
        /// Receive a T object that can be anything, serialize to string json, encode to bytes, then I add the header at the begining and the footer in the ending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Convert<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            // Calculate the total size of the data with headers and footers
            int totalSize = Header.Length + jsonData.Length + Footer.Length;

            // Create a new byte array with everybody
            byte[] completeData = new byte[totalSize];

            // Copy the header at the beginning
            Buffer.BlockCopy(Header, 0, completeData, 0, Header.Length);

            // Copy the serialized data after the header
            Buffer.BlockCopy(jsonData, 0, completeData, Header.Length, jsonData.Length);

            // Copy the footer at the end
            Buffer.BlockCopy(Footer, 0, completeData, Header.Length + jsonData.Length, Footer.Length);

            return completeData;
        }
    }
}
