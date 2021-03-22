using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace WPF2_14
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {
        public ImagePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Images | *.png";
            dialog.DefaultExt = "png";
            if(dialog.ShowDialog()==true)
            {
                BitmapEncoder imgSaver = new PngBitmapEncoder();
                imgSaver.Frames.Add(BitmapFrame.Create(this.DataContext as BitmapImage));
                using (var stream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    imgSaver.Save(stream);
                }
            }
        }
    }
}
