using System;
using System.Threading.Tasks;
using Android.Widget;
using FFImageLoading;
using Plugin.Media;

namespace Watsbook_Android.Others
{
    public class ViewsHelper
    {
        public static void GetImageFromUrl(string url, ImageView imageView)
        {
            if (url != "")
            {
                ImageService.Instance.LoadUrl(url)
                    .Retry(3, 200)
                    .DownSample(400, 400)
                    .Into(imageView);
            }
        }

        public static async Task<byte[]> SelectPhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                throw new Exception("Dodawanie zdjęć nie jest wspierane.");
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 20
            });
            if (file == null)
            {
                throw new Exception("Plik nie istnieje.");
            }

            return System.IO.File.ReadAllBytes(file.Path);
        }
    }
}
