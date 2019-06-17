using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using ManagerLogbook.Web.Services.Contracts;

namespace ManagerLogbook.Web.Services
{
    public class ImageOptimizer : IImageOptimizer
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ImageOptimizer(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public void DeleteOldImage(string imageName)
        {
            var imageToDelete = new FileInfo(imageName);
            imageToDelete.Delete();
        }

        public string OptimizeImage(IFormFile inputImage, int endHeight, int endWidth)
        {
            double aspectRatioCoeff = endHeight / endWidth;
            var imageName = Guid.NewGuid() + ".jpg";
            var inputFileName = Path.Combine(hostingEnvironment.WebRootPath, "images/") + imageName;

            using (var initialImageStream = new MemoryStream())
            {
                inputImage.CopyTo(initialImageStream);

                //Compress
                initialImageStream.Position = 0;
                var optimizer = new ImageMagick.ImageOptimizer();
                optimizer.LosslessCompress(initialImageStream);
                initialImageStream.Position = 0;

                //Resize
                using (MagickImage image = new MagickImage(initialImageStream))
                {
                    double tempWidth = 0;
                    double tempHeight = 0;

                    double imgHeight = image.Height;
                    double imgWidth = image.Width;
                    double aspectRatio = imgHeight / imgWidth;

                    var scale = Math.Max(endWidth / imgWidth, endHeight / imgHeight);

                    tempWidth = imgWidth * scale;
                    tempHeight = imgHeight * scale;

                    int resultedSize = (int)Math.Max(tempWidth, tempHeight);

                    MagickGeometry size = new MagickGeometry(resultedSize)
                    {
                        IgnoreAspectRatio = false
                    };

                    image.Resize(size);
                    var resizedImageByteArr = image.ToByteArray();

                    using (var finalImageStream = new MemoryStream(resizedImageByteArr))
                    {
                        //Crop
                        var cropRect = new Rectangle(0, 0, endWidth, endHeight);
                        using (Bitmap src = new Bitmap(finalImageStream))
                        {
                            using (Bitmap target = new Bitmap(cropRect.Width, cropRect.Height))
                            {
                                var result = new Rectangle(0, 0, target.Width, target.Height);
                                Graphics g = Graphics.FromImage(target);
                                g.DrawImage(src, result, cropRect, GraphicsUnit.Pixel);

                                target.Save(inputFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }
                }
            }
            return imageName;
        }
    }
}
