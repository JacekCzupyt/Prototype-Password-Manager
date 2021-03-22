using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF2_14.DirectoryTreeItems
{
    abstract class AbstractDirectoryTreeItem : TreeViewItem
    {
        public abstract object CreateDataContext();
        public abstract Page GetPage();
        //public virtual string Serialize()
        //{
        //    String s = "<Items>\n";
        //    foreach (var item in Items)
        //    {
        //        if (item is AbstractDirectoryTreeItem)
        //        {
        //            s += (item as AbstractDirectoryTreeItem).Serialize() + "\n";
        //        }
        //    }
        //    s += "</Items>";
        //    return s;
        //}
    }
}
