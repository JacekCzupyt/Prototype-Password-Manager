using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF2_14.DirectoryTreeItems
{
    class DirectoryItem : AbstractDirectoryTreeItem
    {
        public override object CreateDataContext()
        {
            return $"{this.Header} ({this.Items.Count})";
        }

        public override Page GetPage()
        {
            return new DirectoryPage();
        }

        //public override string Serialize()
        //{
        //    return "<DirectoryItem>\n" +
        //        base.Serialize() +
        //        $"\n<Header>{this.Header}</Header>" +
        //        "\n</DirectoryItem>";
        //}

        public DirectoryItem(string s, Page1 page)
        {
            var menu = new ContextMenu();
            menu.Items.Add(new MenuItem() { Header = s, IsEnabled = false });
            menu.Items.Add(new Separator());

            var adddir = new MenuItem() { Header = "Add directory" };
            adddir.Click += page.Add_Subdirectory;
            menu.Items.Add(adddir);
            

            var addfile = new MenuItem() { Header = "Add file" };

            var addpass = new MenuItem() { Header = "Passwords" };
            addpass.Click += page.Add_Sub_Passwords;
            addfile.Items.Add(addpass);

            var addimg = new MenuItem() { Header = "Image" };
            addimg.Click += page.Add_Sub_Image;
            addfile.Items.Add(addimg);

            menu.Items.Add(addfile);

            menu.Items.Add(new Separator());

            var rename = new MenuItem() { Header = "Rename" };
            rename.Click += page.RenameItem;
            menu.Items.Add(rename);

            var delete = new MenuItem() { Header = "Delete" };
            delete.Click += page.DeleteItem;
            menu.Items.Add(delete);

            Header = s;
            this.FontWeight = FontWeights.Bold;
            ContextMenu = menu;
        }
    }
}
