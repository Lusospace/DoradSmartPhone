using Newtonsoft.Json.Linq;
using System.Text.Json;

public static class GoogleMapsGeocoding
{
    private const string GoogleMapsGeocodingApiBaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
    private const string GooglePlacesApiBaseUrl = "https://maps.googleapis.com/maps/api/staticmap?";
    private const string GoogleMapsApiKey = "AIzaSyDaW6mCTLhn8sDi0yfbpjDqtBLkudQSRKA"; // --> is also in AndroidManifest
    private const string size = "500x500";
    private const string zoom = "15";

    /// <summary>
    /// Receives the latitude and longitude, then go to the google maps api in the internet to grab the full address based on the provided coordinates.
    /// We use to get the starting and finishing address of the routes. It will return the image of the map later.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns>The Address</returns>
    /// <exception cref="Exception"></exception>
    public static async Task<string> GetAddressName(double latitude, double longitude)
    {
        try
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
                        var photo = await GetPlacePhotoUrl(latitude, longitude); //--> Need to send the image to the exercise list after
                        return result.results[0].formatted_address;
                    }
                }
                else
                {
                    // Handle API error
                    Console.WriteLine("Geocoding API request not found.");
                }
            }

            return null;
        }
        catch(Exception ex)
        {
            throw new Exception("Google API error {}", ex);
        }
        
    }

    /// <summary>
    /// Receive the coordinates and uses google maps api to get a static image of the google maps location.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    public static async Task<Stream> GetPlacePhotoUrl(double latitude, double longitude)
    {
        string url = $"{GooglePlacesApiBaseUrl}center={latitude},{longitude}&zoom={zoom}&size={size}&key={GoogleMapsApiKey}";

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Stream imageStream = await response.Content.ReadAsStreamAsync();
                return imageStream;
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
/// <summary>
/// Class to handle the google maps api response
/// </summary>
public class GoogleMapsGeocodingResponse
{
    public string status { get; set; }    
    public List<GoogleMapsGeocodingResult> results { get; set; }
}
/// <summary>
/// Class to handle with the result, in our case only the address
/// </summary>
public class GoogleMapsGeocodingResult
{    
    public string formatted_address { get; set; }
}
