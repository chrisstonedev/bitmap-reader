using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace BitmapReader.Tests
{
    public class Tests
    {
        [Test]
        public void CanReadBytesFromAFile()
        {
            var bytes = File.ReadAllBytes("test_file.bin");
            var expected = new byte[] {0, 1, 2, 3, 4, 5};
            Assert.That(bytes, Is.EqualTo(expected));
        }

        [Test]
        public void HasIncorrectHeader()
        {
            Assert.Throws<IncorrectHeaderException>(() => _ = new Bitmap("test_file.bin"));
        }

        [TestCase("10x10_24bpp_all_white.bmp")]
        [TestCase("10x10_24bpp_one_red_pixel.bmp")]
        public void HasCorrectValues(string filename)
        {
            var bitmap = new Bitmap(filename);
            Assert.That(bitmap.Offset, Is.EqualTo(54));
            Assert.That(bitmap.HeaderSize, Is.EqualTo(40));
            Assert.That(bitmap.ImageHeight, Is.EqualTo(10));
            Assert.That(bitmap.ImageWidth, Is.EqualTo(10));
            Assert.That(bitmap.BitsPerPixel, Is.EqualTo(24));
            Assert.That(bitmap.RowSize, Is.EqualTo(32));
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

        [TestCase("10x10_24bpp_all_white.bmp")]
        [TestCase("10x10_24bpp_one_red_pixel.bmp")]
        public void ReadFirstPixelOfLastRow(string filename)
        {
            var bitmap = new Bitmap(filename);
            var color = bitmap.GetPixel(bitmap.ImageHeight - 1, 0);
            Assert.That(color, Is.EqualTo(Color.FromArgb(255, 255, 255)));
        }

        [Test]
        public void ReadFirstPixelOfFirstRow()
        {
            var bitmap = new Bitmap("10x10_24bpp_one_red_pixel.bmp");
            var color = bitmap.GetPixel(0, 0);
            Assert.That(color, Is.EqualTo(Color.FromArgb(255, 0, 0)));
        }
    }
}