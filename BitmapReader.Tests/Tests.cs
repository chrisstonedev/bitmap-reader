using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace BitmapReader.Tests
{
    public class Tests
    {
        [Test]
        public void HasIncorrectHeader()
        {
            Assert.Throws<FileLoadException>(() => _ = new Bitmap("test_file.bin"));
        }

        [TestCase("10x10_24bpp_all_white.bmp")]
        [TestCase("10x10_24bpp_one_red_pixel.bmp")]
        [TestCase("10x10_256clr_all_white.bmp")]
        [TestCase("10x10_256clr_one_red_pixel.bmp")]
        [TestCase("10x10_16clr_all_white.bmp")]
        [TestCase("10x10_16clr_one_red_pixel.bmp")]
        [TestCase("10x10_mono_all_white.bmp")]
        [TestCase("10x10_mono_one_red_pixel.bmp")]
        public void HasCorrectValuesForAllFormats(string filename)
        {
            var bitmap = new Bitmap(filename);
            Assert.That(bitmap.Reserved, Is.EqualTo(0));
            Assert.That(bitmap.HeaderSize, Is.EqualTo(40));
            Assert.That(bitmap.ImageWidth, Is.EqualTo(10));
            Assert.That(bitmap.ImageHeight, Is.EqualTo(10));
            Assert.That(bitmap.NumberOfColorPlanes, Is.EqualTo(1));
            Assert.That(bitmap.Compression, Is.EqualTo(0));
            Assert.That(bitmap.HorizontalPixelsPerMeter, Is.EqualTo(3780));
            Assert.That(bitmap.VerticalPixelsPerMeter, Is.EqualTo(3780));
            Assert.That(bitmap.NumberOfColorsUsed, Is.EqualTo(0));
            Assert.That(bitmap.NumberOfImportantColors, Is.EqualTo(0));
        }

        [TestCase("10x10_24bpp_all_white.bmp")]
        [TestCase("10x10_24bpp_one_red_pixel.bmp")]
        public void HasCorrectValuesFor24BitsPerPixel(string filename)
        {
            var bitmap = new Bitmap(filename);
            Assert.That(bitmap.FileSizeInBytes, Is.EqualTo(374));
            Assert.That(bitmap.Offset, Is.EqualTo(54));
            Assert.That(bitmap.BitsPerPixel, Is.EqualTo(BitsPerPixel.TwentyFour));
            Assert.That(bitmap.ImageSizeInBytes, Is.EqualTo(0));
            Assert.That(bitmap.RowSize, Is.EqualTo(32));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 0), Is.EqualTo(54));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 1), Is.EqualTo(57));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 2), Is.EqualTo(60));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 0), Is.EqualTo(86));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 1), Is.EqualTo(89));
        }

        [TestCase("10x10_256clr_all_white.bmp")]
        [TestCase("10x10_256clr_one_red_pixel.bmp")]
        public void HasCorrectValuesFor256Colors(string filename)
        {
            var bitmap = new Bitmap(filename);
            Assert.That(bitmap.FileSizeInBytes, Is.EqualTo(1198));
            Assert.That(bitmap.Offset, Is.EqualTo(1078));
            Assert.That(bitmap.BitsPerPixel, Is.EqualTo(BitsPerPixel.Eight));
            Assert.That(bitmap.ImageSizeInBytes, Is.EqualTo(120));
            Assert.That(bitmap.RowSize, Is.EqualTo(12));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 0), Is.EqualTo(1078));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 1), Is.EqualTo(1079));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 2), Is.EqualTo(1080));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 0), Is.EqualTo(1090));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 1), Is.EqualTo(1091));
        }

        [TestCase("10x10_16clr_all_white.bmp")]
        [TestCase("10x10_16clr_one_red_pixel.bmp")]
        public void HasCorrectValuesFor16Colors(string filename)
        {
            var bitmap = new Bitmap(filename);
            Assert.That(bitmap.FileSizeInBytes, Is.EqualTo(198));
            Assert.That(bitmap.Offset, Is.EqualTo(118));
            Assert.That(bitmap.BitsPerPixel, Is.EqualTo(BitsPerPixel.Four));
            Assert.That(bitmap.ImageSizeInBytes, Is.EqualTo(80));
            Assert.That(bitmap.RowSize, Is.EqualTo(8));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 0), Is.EqualTo(118));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 1), Is.EqualTo(118));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 2), Is.EqualTo(119));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 3), Is.EqualTo(119));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 4), Is.EqualTo(120));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 0), Is.EqualTo(126));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 1), Is.EqualTo(126));
        }

        [TestCase("10x10_mono_all_white.bmp")]
        [TestCase("10x10_mono_one_red_pixel.bmp")]
        public void HasCorrectValuesForMonochrome(string filename)
        {
            var bitmap = new Bitmap(filename);
            Assert.That(bitmap.FileSizeInBytes, Is.EqualTo(102));
            Assert.That(bitmap.Offset, Is.EqualTo(62));
            Assert.That(bitmap.BitsPerPixel, Is.EqualTo(BitsPerPixel.One));
            Assert.That(bitmap.ImageSizeInBytes, Is.EqualTo(40));
            Assert.That(bitmap.RowSize, Is.EqualTo(4));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 0), Is.EqualTo(62));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 7), Is.EqualTo(62));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 8), Is.EqualTo(63));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 1, 9), Is.EqualTo(63));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 0), Is.EqualTo(66));
            Assert.That(bitmap.GetOffset(bitmap.ImageHeight - 2, 1), Is.EqualTo(66));
        }

        [TestCase("10x10_24bpp_all_white.bmp")]
        [TestCase("10x10_24bpp_one_red_pixel.bmp")]
        [TestCase("10x10_256clr_all_white.bmp")]
        [TestCase("10x10_256clr_one_red_pixel.bmp")]
        [TestCase("10x10_16clr_all_white.bmp")]
        [TestCase("10x10_16clr_one_red_pixel.bmp")]
        [TestCase("10x10_mono_all_white.bmp")]
        [TestCase("10x10_mono_one_red_pixel.bmp")]
        public void ReadFirstPixelOfBottomRow(string filename)
        {
            var bitmap = new Bitmap(filename);
            var color = bitmap.GetPixelColor(bitmap.ImageHeight - 1, 0);
            Assert.That(Color.FromArgb(color.ToArgb()), Is.EqualTo(Color.FromArgb(255, 255, 255)));
        }

        [Test]
        public void RoundUpToNearestFourTest()
        {
            Assert.That(Bitmap.RoundUpToNearestFour(4), Is.EqualTo(4));
            Assert.That(Bitmap.RoundUpToNearestFour(5), Is.EqualTo(8));
            Assert.That(Bitmap.RoundUpToNearestFour(6), Is.EqualTo(8));
            Assert.That(Bitmap.RoundUpToNearestFour(7), Is.EqualTo(8));
            Assert.That(Bitmap.RoundUpToNearestFour(8), Is.EqualTo(8));
            Assert.That(Bitmap.RoundUpToNearestFour(9), Is.EqualTo(12));
        }

        [TestCase("10x10_24bpp_one_red_pixel.bmp")]
        [TestCase("10x10_256clr_one_red_pixel.bmp")]
        [TestCase("10x10_16clr_one_red_pixel.bmp")]
        public void ReadRedPixelInFirstRow(string filename)
        {
            var bitmap = new Bitmap(filename);
            var color = bitmap.GetPixelColor(0, 0);
            Assert.That(color, Is.EqualTo(Color.FromArgb(255, 0, 0)));
        }

        [Test]
        public void ReadBlackPixelInMonochromeImage()
        {
            var bitmap = new Bitmap("10x10_mono_one_red_pixel.bmp");
            var color = bitmap.GetPixelColor(0, 0);
            Assert.That(Color.FromArgb(color.ToArgb()), Is.EqualTo(Color.FromArgb(0, 0, 0)));
        }
    }
}