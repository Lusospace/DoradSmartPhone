using System.Text.Json;

public static class GoogleMapsGeocoding
{
    private const string GoogleMapsGeocodingApiBaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
    private const string GoogleMapsApiKey = "AIzaSyDaW6mCTLhn8sDi0yfbpjDqtBLkudQSRKA"; // --> is also in AndroidManifest

    public static async Task<string> GetAddressName(double latitude, double longitude)
    {
        string url = $"{GoogleMapsGeocodingApiBaseUrl}?latlng={latitude},{longitude}&key={GoogleMapsApiKey}";

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GoogleMapsGeocodingResponse>(responseContent);
                if (result.status == "OK" && result.results.Count > 0)
                {
                    return result.results[0].formatted_address;
                }
            }
            else
            {
                // Handle API error
                Console.WriteLine("Geocoding API request failed.");
            }
        }

        return null;
    }
}

public class GoogleMapsGeocodingResponse
{
    public string status { get; set; }
    public List<GoogleMapsGeocodingResult> results { get; set; }
}

public class GoogleMapsGeocodingResult
{
    public string formatted_address { get; set; }
}
