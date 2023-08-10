using SkiaSharp;

namespace DoradSmartphone.Helpers
{
    public static class PhotoPickerHelper
    {
        public static async Task<byte[]> PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    using (var stream = await photo.OpenReadAsync())
                    {
                        // Load the image using SkiaSharp
                        using (var originalImage = SKBitmap.Decode(stream))
                        {
                            // Calculate the scaling factors for resizing
                            float scaleX = (float)Constants.XPosition / originalImage.Width;
                            float scaleY = (float)Constants.YPosition / originalImage.Height;

                            // Calculate the final dimensions for resizing
                            int newWidth = (int)(originalImage.Width * scaleX);
                            int newHeight = (int)(originalImage.Height * scaleY);

                            // Create a new SKBitmap with the target dimensions
                            using (var resizedImage = originalImage.Resize(new SKImageInfo(newWidth, newHeight), SKBitmapResizeMethod.Lanczos3))
                            {
                                // Convert the resized image to a byte array (PNG format)
                                using (var ms = new MemoryStream())
                                {
                                    resizedImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(ms);
                                    return ms.ToArray();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Toaster.MakeToast("Error when picking image. " + ex);
                throw new Exception("Error picking image.");
            }
            return null;
        }
    }
}
