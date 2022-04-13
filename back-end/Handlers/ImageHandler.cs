using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Net;

namespace back_end.Handlers
{
    public static class ImageHandler
    {
        private static string _imagePath= "https://kananmafiapictures.alwaysdata.net/pictures/";
        public static byte[] ImageToByteArray(string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(_imagePath + filename);
            }
        }
    }
}
