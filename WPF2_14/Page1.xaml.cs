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
using WPF2_14.DirectoryTreeItems;

namespace WPF2_14
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public string MasterPassword;


        public Page1()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginPage pg = new LoginPage();
            NavigationService.Navigate(pg);
        }

        private void Add_Directory_Click(object sender, RoutedEventArgs e)
        {
            DirectoryTree.Items.Add(new DirectoryItem($"New Directory",
                this));
                //new RoutedEventHandler[]{ Add_Subdirectory, Add_Sub_Passwords, Add_Sub_Image, RenameItem, DeleteItem }));
        }

        private void Add_Passwords_Click(object sender, RoutedEventArgs e)
        {
            PasswordItem pass = new PasswordItem($"New Passwords", this);
                //new RoutedEventHandler[] { RenameItem, DeleteItem });
            DirectoryTree.Items.Add(pass);
        }

        private void Add_Image_Click(object sender, RoutedEventArgs e)
        {
            ImageItem img = new ImageItem($"New Image", this);
                //new RoutedEventHandler[] { RenameItem, DeleteItem });
            if(img.Img!=null)
            {
                DirectoryTree.Items.Add(img);
            }
            
        }

        TreeViewItem GetMenuOwner(object sender)
        {
            if (!(sender is MenuItem)) throw new Exception();
            var contMenu = ((sender as MenuItem).Parent);
            if (!(contMenu is ContextMenu)) throw new Exception();
            var item = ((contMenu as ContextMenu).PlacementTarget);
            if (!(item is TreeViewItem)) throw new Exception();
            return item as TreeViewItem;
        }

        public void Add_Subdirectory(object sender, RoutedEventArgs e)
        {
            GetMenuOwner(sender).Items.Add(new DirectoryItem($"New Directory",
                this));
                //new RoutedEventHandler[] { Add_Subdirectory, Add_Sub_Passwords, Add_Sub_Image, RenameItem, DeleteItem }));
        }

        public void Add_Sub_Passwords(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem)) throw new Exception();

            GetMenuOwner((sender as MenuItem).Parent).Items.Add(new PasswordItem($"New Passwords", this));
                //new RoutedEventHandler[] { RenameItem, DeleteItem }));
        }

        public void Add_Sub_Image(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem)) throw new Exception();
            var Owner = GetMenuOwner((sender as MenuItem).Parent);

            var img = new ImageItem($"New Image", this);
                //new RoutedEventHandler[] { RenameItem, DeleteItem });
            if(img.Img!=null)
            {
                Owner.Items.Add(img);
            }

            
        }

        private void DirectoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (RightFrame.Content is PasswordMenager && (RightFrame.Content as PasswordMenager).CurrentlyEdited != null)
                (RightFrame.Content as PasswordMenager).CancelButtonClick(null, null);

                

            if (DirectoryTree.SelectedItem == null)
            {
                RightFrame.Navigate(null);
            }
            if (DirectoryTree.SelectedItem is AbstractDirectoryTreeItem)
            {
                var selected = (DirectoryTree.SelectedItem as AbstractDirectoryTreeItem);
                RightFrame.Navigate(selected.GetPage());
                RightFrame.DataContext = selected.CreateDataContext();
            }
        }

        private void RightFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext();
        }

        private void RightFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext();
        }

        void UpdateFrameDataContext()
        {
            var content = (RightFrame.Content as FrameworkElement);
            if (content != null)
                content.DataContext = RightFrame.DataContext;
        }

        public void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem)) throw new Exception();
            var contMenu = ((sender as MenuItem).Parent);
            if (!(contMenu is ContextMenu)) throw new Exception();
            var item = ((contMenu as ContextMenu).PlacementTarget);
            if (!(item is TreeViewItem)) throw new Exception();

            var treeParent = (item as TreeViewItem).Parent;
            if(treeParent is TreeViewItem)
            {
                (treeParent as TreeViewItem).Items.Remove(item);
                return;
            }
            if (treeParent is TreeView)
            {
                (treeParent as TreeView).Items.Remove(item);
                return;
            }
            throw new ArgumentException();
        }

        public void RenameItem(object sender, RoutedEventArgs e)
        {
            var Item = GetMenuOwner(sender);

            if(Item.Header is string)
            {
                string s = Item.Header as string;
                Item.Header = new TextBox() { Text = s };
                (Item.Header as TextBox).Loaded += SetRenameFocus;
            }
        }

        private void SetRenameFocus(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox)
            {
                var item = sender as TextBox;
                Keyboard.Focus(item);
                item.LostKeyboardFocus += FinishRenameItem;
                item.KeyDown += TnterKeyPressed;
            }
            
        }

        private void TnterKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FinishRenameItem(sender, null);
        }

        private void FinishRenameItem(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox && (sender as TextBox).Parent is TreeViewItem)
            {
                var Item = (sender as TextBox).Parent as TreeViewItem;
                Item.Header = (Item.Header as TextBox).Text;
                Item.LostKeyboardFocus -= FinishRenameItem;
                Item.KeyDown -= TnterKeyPressed;
            }
        }
    }
}
