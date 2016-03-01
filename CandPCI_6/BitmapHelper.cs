using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_6
{
    public class BitmapHelper
    {
        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат byte[,,].
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Байтовый массив с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe static byte[, ,] BitmapToByteRgbQ(Bitmap bmp)
        {
            int width  = bmp.Width,
                height = bmp.Height;
            var result = new byte[3, height, width];
            var bd = bmp.LockBits(new Rectangle(0, 0, width, height), 
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* _curpos;
                fixed (byte* _result = result)
                {
                    byte* _red   = _result, 
                          _green = _result + 1, 
                          _blue  = _result + 2;
                    for (var h = 0; h < height; h++)
                    {
                        _curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (var w = 0; w < width; w++)
                        {
                            *_blue = *(_curpos++); 
                             _blue += 3;

                            *_green = *(_curpos++); 
                             _green += 3;

                            *_red = *(_curpos++); 
                             _red += 3;
                        } 
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return result;
        }

        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат byte[,,].
        /// При этом третий индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), первый -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Байтовый массив с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe static byte[, ,] BitmapToByteRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            //byte[, ,] result = new byte[3, height, width];
            var result = new byte[height, width, 3];
            var bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (var h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (var w = 0; w < width; w++)
                    {
                        result[h, w, 2] = *(curpos++);
                        result[h, w, 1] = *(curpos++);
                        result[h, w, 0] = *(curpos++);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return result;
        }

        public unsafe static byte[] BitmapToByteRgbMarshal(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            var result = new byte[3 * height * width];
            var bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                var lineSize = width * 3;
                for (var h = 0; h < height; h++)
                {
                    var pos = h * lineSize;
                    var curpos = (IntPtr)((byte*)bd.Scan0) + h * bd.Stride;
                    Marshal.Copy(curpos, result, pos, lineSize);
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return result;
        }

        public unsafe static void ByteToBitmapRgbMarshal(Bitmap bmp, byte[] result)
        {
            int width = bmp.Width,
                height = bmp.Height;
            //var result = new byte[3 * height * width];
            var bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                var lineSize = width * 3;
                for (var h = 0; h < height; h++)
                {
                    var pos = h * lineSize;
                    var curpos = (IntPtr)((byte*)bd.Scan0) + h * bd.Stride;
                    Marshal.Copy(result, pos, curpos, lineSize);
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
        }

        /// <summary>
        /// Метод загружает растровые изображения без блокирования файла
        /// (как это делает конструктор Bitmap(fileName)).
        /// </summary>
        /// <param name="fileName">Имя файла для загрузки.</param>
        /// <returns>Экземпляр Bitmap.</returns>
        public static Bitmap LoadBitmap(string fileName)
        {
            //using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    return new Bitmap(fs);
            //}

            var firstBmp = new Bitmap(fileName);
            var secondBmp = new Bitmap(firstBmp.Width, firstBmp.Height, firstBmp.PixelFormat);
            var gr = Graphics.FromImage(secondBmp);
            gr.DrawImage(firstBmp, 0, 0);
            gr.Dispose();
            firstBmp.Dispose();
            return secondBmp;
  
        }
    }
}
