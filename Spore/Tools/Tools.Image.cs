using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Spore
{
    public partial class Tools
    {

        /// <summary>
        /// 获取文件数组扩展名称带.
        /// </summary>
        /// <param name="photodata"></param>
        /// <returns></returns>
        public static string GetImageSuffixWithDot(byte[] photodata)
        {
            return "." + GetImageSuffixWithoutDot(photodata);
        }

        /// <summary>
        /// 获取文件数组扩展名称不带.
        /// </summary>
        /// <param name="photodata"></param>
        /// <returns></returns>
        public static string GetImageSuffixWithoutDot(byte[] photodata)
        {
            System.Drawing.Image photoimg = System.Drawing.Image.FromStream(new MemoryStream(photodata));
            if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                return "bmp";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
            {
                return "emf";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
            {
                return "exif";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                return "gif";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
            {
                return "icon";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                return "jpg";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
            {
                return "bmp";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                return "png";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
            {
                return "tiff";
            }
            else if (photoimg.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Wmf))
            {
                return "wmf";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据图片mime类型字符串获取图片格式
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormatFromMimeType(String mimeType)
        {
            switch (mimeType)
            {
                case MIMEs.JPEG:
                case MIMEs.PJPEG:
                    return ImageFormat.Jpeg;

                case MIMEs.GIF:
                    return ImageFormat.Gif;

                case MIMEs.BMP:
                    return ImageFormat.Bmp;

                case MIMEs.TIFF:
                    return ImageFormat.Tiff;

                case MIMEs.PNG:
                    return ImageFormat.Png;

                default:
                    throw new ArgumentException("Unsupported  MIME type '" + mimeType + "'", "mimeType");
            }
        }

        /// <summary>
        /// 根据mime类型获取编码器
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static ImageCodecInfo FindCodecForType(String mimeType)
        {
            ImageCodecInfo[] imgEncoders = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < imgEncoders.GetLength(0); i++)
            {
                if (imgEncoders[i].MimeType == mimeType)
                {
                    //Found it
                    return imgEncoders[i];
                }
            }

            //No encoders match
            return null;
        }

        /// <summary>
        /// 根据mime类型获取图片format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String MimeTypeFromImageFormat(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Jpeg))
            {
                return MIMEs.JPEG;
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                return MIMEs.GIF;
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                return MIMEs.BMP;
            }
            else if (format.Equals(ImageFormat.Tiff))
            {
                return MIMEs.TIFF;
            }
            else if (format.Equals(ImageFormat.Png))
            {
                return MIMEs.PNG;
            }
            else
            {
                throw new ArgumentException("Unsupported  image format '" + format + "'", "format");
            }
        }

        /// <summary>
        /// 转换为jpeg格式并设置质量
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static Bitmap ConvertToJPEG(Bitmap bitmap, int quality)
        {

            var imgStream = Tools.ConvertToJPEGStream(bitmap, quality);

            Bitmap destBitmap = new Bitmap(imgStream);

            return destBitmap;

        }

        /// <summary>
        /// 获取转换后的图片流
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static MemoryStream ConvertToJPEGStream(Bitmap bitmap, int quality)
        {
            //设置转换参数
            EncoderParameters destEncParams = new EncoderParameters(1);
            //设置图片质量参数
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            destEncParams.Param[0] = qualityParam;

            //新图片流
            System.IO.MemoryStream imgStream = new System.IO.MemoryStream();

            ImageCodecInfo destCodec = FindCodecForType(MimeTypeFromImageFormat(ImageFormat.Jpeg));

            bitmap.Save(imgStream, destCodec, destEncParams);

            return imgStream;
        }


    }
}
