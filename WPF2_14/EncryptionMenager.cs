using System;
using System.Collections.Generic;
using System.Globalization;
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
using WPF_Project;
using System.Xml.Serialization;
using System.IO;
using WPF2_14.DirectoryTreeItems;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using WPF_Project;


namespace WPF2_14
{
    partial class Page1
    {
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            FileStream FileWriter = new FileStream(@"Passwords.bin", FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();

            List<SerializableDirectoryTree> l = new List<SerializableDirectoryTree>();

            foreach (var child in DirectoryTree.Items)
                if (child is AbstractDirectoryTreeItem)
                    l.Add(new SerializableDirectoryTree(child as AbstractDirectoryTreeItem));

            using (var mem = new MemoryStream())
            {
                formatter.Serialize(mem, l);
                var encrypted = DataEncryption.Encrypt(MasterPassword, mem.ToArray());
                FileWriter.Write(encrypted, 0, encrypted.Length); 
            }

            //formatter.Serialize(FileWriter, l);

            FileWriter.Close();

        }
    }

    [Serializable]
    class SerializableDirectoryTree
    {
        public string Name;
        public object Data;

        public List<SerializableDirectoryTree> Children = new List<SerializableDirectoryTree>();

        public SerializableDirectoryTree(AbstractDirectoryTreeItem item)
        {
            Name = item.Header as string;
            if (item is PasswordItem)
                Data = (item as PasswordItem).PassowrdList;
            if (item is ImageItem)
            {
                Data = ((item as ImageItem).Img as BitmapImage).ToBytes();
            }
                
            if (item is DirectoryItem)
                Data = null;

            foreach (var child in item.Items)
                if (child is AbstractDirectoryTreeItem)
                    Children.Add(new SerializableDirectoryTree(child as AbstractDirectoryTreeItem));
        }

        public AbstractDirectoryTreeItem TurnIntoTreeView(Page1 page)
        {
            AbstractDirectoryTreeItem item;
            if (Data == null)
            {
                item = new DirectoryItem(Name, page);
            }
            else if (Data is byte[])
            {
                item = new ImageItem(Name, page, Data as byte[]);
            }
            else if (Data is List<PasswordData>)
            {
                item = new PasswordItem(Name, page) { PassowrdList = Data as List<PasswordData> };
                foreach(var pass in (item as PasswordItem).PassowrdList)
                {
                    if(pass.SerializedBitmap!=null)
                    {
                        pass.IconImage = new BitmapImage();
                        pass.IconImage.FromBytes(pass.SerializedBitmap);
                    }
                }   
            }
            else
                throw new ArgumentException();

            foreach(var child in Children)
            {
                item.Items.Add(child.TurnIntoTreeView(page));
            }
            return item;
        }
    }
}
