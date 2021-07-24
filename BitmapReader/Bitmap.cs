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

            if (bytes[0] != 'B' || bytes[1] != 'M')
                throw new FileLoadException();
            _bytes = bytes;
        }

        public int FileSizeInBytes => BitConverter.ToInt32(_bytes, 2);
        public int Reserved => BitConverter.ToInt32(_bytes, 6);
        public int Offset => BitConverter.ToInt32(_bytes, 10);
        public int HeaderSize => BitConverter.ToInt32(_bytes, 14);
        public int ImageWidth => BitConverter.ToInt32(_bytes, 18);
        public int ImageHeight => BitConverter.ToInt32(_bytes, 22);
        public int NumberOfColorPlanes => BitConverter.ToInt16(_bytes, 26);
        public BitsPerPixel BitsPerPixel => (BitsPerPixel) BitConverter.ToInt16(_bytes, 28);
        public int Compression => BitConverter.ToInt32(_bytes, 30);
        public int ImageSizeInBytes => BitConverter.ToInt32(_bytes, 34);
        public int HorizontalPixelsPerMeter => BitConverter.ToInt32(_bytes, 38);
        public int VerticalPixelsPerMeter => BitConverter.ToInt32(_bytes, 42);
        public int NumberOfColorsUsed => BitConverter.ToInt32(_bytes, 46);
        public int NumberOfImportantColors => BitConverter.ToInt32(_bytes, 50);

        public int RowSize => RoundUpToNearestFour((int) (ImageWidth * (int) BitsPerPixel / 8.0));

        public static int RoundUpToNearestFour(int value)
        {
            return (value + 3) & -4;
        }

        private int GetColorByteLocation(int column, int pixelByteLocation)
        {
            if (BitsPerPixel == BitsPerPixel.TwentyFour)
            {
                return pixelByteLocation;
            }

            var colorNumber = BitsPerPixel switch
            {
                BitsPerPixel.One => (_bytes[pixelByteLocation] >> (7 - column % 8)) & 0x1,
                BitsPerPixel.Four => (_bytes[pixelByteLocation] >> (1 - column % 2) * 4) & 0xF,
                BitsPerPixel.Eight => _bytes[pixelByteLocation],
                _ => throw new InvalidOperationException()
            };

            return 54 + colorNumber * 4;
        }

        public int GetOffset(int row, int column)
        {
            row = ImageHeight - 1 - row;
            return BitsPerPixel switch
            {
                BitsPerPixel.One => Offset + row * RowSize + column / 8,
                BitsPerPixel.Four => Offset + row * RowSize + column / 2,
                BitsPerPixel.Eight => Offset + row * RowSize + column * 1,
                BitsPerPixel.TwentyFour => Offset + row * RowSize + column * 3,
                _ => throw new Exception()
            };
        }

        public Color GetPixelColor(int row, int column)
        {
            var pixelByteLocation = GetOffset(row, column);

            var colorByteLocation = GetColorByteLocation(column, pixelByteLocation);

            var blue = _bytes[colorByteLocation];
            var green = _bytes[colorByteLocation + 1];
            var red = _bytes[colorByteLocation + 2];
            return Color.FromArgb(red, green, blue);
        }
    }

    public enum BitsPerPixel
    {
        One = 1,
        Four = 4,
        Eight = 8,
        TwentyFour = 24
    }
}