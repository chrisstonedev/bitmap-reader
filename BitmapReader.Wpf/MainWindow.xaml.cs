using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace BitmapReader.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PaintBitmap(Bitmap bitmap)
        {
            // Create the Grid.
            // Create columns.
            BitmapDisplayGrid.Children.Clear();
            BitmapDisplayGrid.ColumnDefinitions.Clear();
            for (var i = 0; i < bitmap.ImageWidth; i++)
            {
                BitmapDisplayGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1)});
            }

            // Create rows.
            BitmapDisplayGrid.RowDefinitions.Clear();
            for (var i = 0; i < bitmap.ImageHeight; i++)
            {
                BitmapDisplayGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1)});
            }

            // Add first column header.
            for (var row = 0; row < bitmap.ImageHeight; row++)
            {
                for (var column = 0; column < bitmap.ImageWidth; column++)
                {
                    var color = bitmap.GetPixel(row, column);
                    AddTextBlockToGrid(color.R, color.G, color.B, row, column);
                }
            }
        }

        private void AddTextBlockToGrid(byte red, byte green, byte blue, int row, int column)
        {
            var textBlock = new TextBlock
            {
                Background = new SolidColorBrush(Color.FromArgb(255, red, green, blue)),
            };
            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            BitmapDisplayGrid.Children.Add(textBlock);
        }

        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result != true) return;

            var bitmap = new Bitmap(openFileDialog.FileName);
            PaintBitmap(bitmap);
            PrintBitmapInformation(bitmap);
        }

        private void PrintBitmapInformation(Bitmap bitmap)
        {
            FileSize.Text = bitmap.FileSize.ToString();
            Offset.Text = bitmap.Offset.ToString();
            HeaderSize.Text = bitmap.HeaderSize.ToString();
            ImageWidth.Text = bitmap.ImageWidth.ToString();
            ImageHeight.Text = bitmap.ImageHeight.ToString();
            NumberOfColorPlanes.Text = bitmap.NumberOfColorPlanes.ToString();
            BitsPerPixel.Text = bitmap.BitsPerPixel.ToString();
            Compression.Text = bitmap.Compression.ToString();
            ImageSize.Text = bitmap.ImageSize.ToString();
        }
    }
}