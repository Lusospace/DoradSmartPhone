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
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
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
