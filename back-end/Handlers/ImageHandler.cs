using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
namespace back_end.Handlers
{
    public static class ImageHandler
    {
        private static string _imagePath= "https://kananmafiapictures.alwaysdata.net/pictures/";
        public static byte[] ImageToByteArray(string filename)
        {
            
            return  File.ReadAllBytes(_imagePath+filename);
        }
    }
}
