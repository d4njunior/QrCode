using Microsoft.AspNetCore.Mvc;
using QrCode.ViewModels;
using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace QrCode.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index(string url)    
        {
            var viewModel = new IndexViewModel();
            if (string.IsNullOrEmpty(url))
            {
                return View(viewModel);
            }
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                ViewData["url"] = url;
                ViewData["Msg"] = "The url is not valid.";
                return View(viewModel);
            }
            var image = GenerateByteArray(url);
            viewModel.ImageB64 = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(image));
            ViewData["url"] = url;
            return View(viewModel);
        }

        public static Bitmap GenerateImage(string url)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(10);
            return qrCodeImage;
        }

        public static byte[] GenerateByteArray(string url)
        {
            var image = GenerateImage(url);
            return ImageToByte(image);
        }

        private static byte[] ImageToByte(Image img)
        {
            using var stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }

    }
}
