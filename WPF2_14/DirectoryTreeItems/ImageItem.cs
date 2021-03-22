using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF2_14.DirectoryTreeItems
{
    class ImageItem : AbstractDirectoryTreeItem
    {
        public BitmapImage Img = new BitmapImage();

        public override object CreateDataContext()
        {
            return Img;
        }

        public override Page GetPage()
        {
            return new ImagePage();
        }

        //public override string Serialize()
        //{
        //    return "<ImageItem>\n" +
        //        base.Serialize() +
        //        $"\n<Header>{this.Header}</Header>" +
        //        "\n</ImageItem>";
        //}

        public ImageItem(string s, Page1 page, byte[] loadedImg=null)
        {
            if (loadedImg == null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Images |*.png;*.jpeg";
                if (dialog.ShowDialog() == true)
                {
                    Img.BeginInit();
                    Img.UriSource = new Uri(dialog.FileName);
                    Img.EndInit();
                }
                else
                    Img = null;
            }
            else
            {
                Img.FromBytes(loadedImg);
            }

            var menu = new ContextMenu();
            menu.Items.Add(new MenuItem() { Header = s, IsEnabled = false });
            menu.Items.Add(new Separator());

            var rename = new MenuItem() { Header = "Rename" };
            rename.Click += page.RenameItem;
            menu.Items.Add(rename);

            var delete = new MenuItem() { Header = "Delete" };
            delete.Click += page.DeleteItem;
            menu.Items.Add(delete);

            Header = s;
            this.FontStyle = FontStyles.Italic;
            this.FontWeight = FontWeights.Normal;
            ContextMenu = menu;
        }
    }

    public static class BitmapImageExtensions
    {
        public static byte[] ToBytes(this BitmapImage image)
        {
            BitmapEncoder imgSaver = new PngBitmapEncoder();
            imgSaver.Frames.Add(BitmapFrame.Create(image));
            using (var stream = new MemoryStream())
            {
                imgSaver.Save(stream);
                return stream.ToArray();
            }
        }

        public static void FromBytes(this BitmapImage Img, byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                Img.BeginInit();
                Img.CacheOption = BitmapCacheOption.OnLoad;
                Img.StreamSource = mem;
                Img.EndInit();
            }
        }
    }

    
}
