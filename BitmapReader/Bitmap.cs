using System;
using System.Drawing;
using System.IO;

namespace BitmapReader
{
    public class Bitmap
    {
        private readonly byte[] _bytes;

        public Bitmap(string filename)
        {
            var bytes = File.ReadAllBytes(filename);

            if (bytes[0] != 66 || bytes[1] != 77)
                throw new IncorrectHeaderException();
            _bytes = bytes;
        }

        public int FileSize => BitConverter.ToInt32(_bytes, 2);
        public int Offset => BitConverter.ToInt32(_bytes, 10);
        public int HeaderSize => BitConverter.ToInt32(_bytes, 14);
        public int ImageWidth => BitConverter.ToInt32(_bytes, 18);
        public int ImageHeight => BitConverter.ToInt32(_bytes, 22);
        public int NumberOfColorPlanes => BitConverter.ToInt16(_bytes, 26);
        public int BitsPerPixel => BitConverter.ToInt16(_bytes, 28);
        public int Compression => BitConverter.ToInt32(_bytes, 30);
        public int ImageSize => BitConverter.ToInt32(_bytes, 34);

        public int RowSize => RoundUpToNearestFour(ImageWidth * 3);

        public static int RoundUpToNearestFour(int value)
        {
            return (value + 3) & -4;
        }

        public Color GetPixel(int row, int column)
        {
            row = ImageHeight - 1 - row;
            var startOfPixel = Offset + row * RowSize + column * 3;
            int blue = _bytes[startOfPixel];
            int green = _bytes[startOfPixel + 1];
            int red = _bytes[startOfPixel + 2];
            return Color.FromArgb(red, green, blue);
        }
    }

    public class IncorrectHeaderException : Exception
    {
    }
}