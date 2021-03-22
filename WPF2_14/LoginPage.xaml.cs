using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace WPF2_14
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!File.Exists(@"Passwords.bin"))
            {
                Page1 p = new Page1();
                p.MasterPassword = MasterPasswordBox.Password;
                NavigationService.Navigate(p);
            }
            else
            {
                byte[] data = File.ReadAllBytes(@"Passwords.bin");
                byte[] decrypted = DataEncryption.Decrypt(MasterPasswordBox.Password, data);
                if (decrypted == null)
                    MessageBox.Show("Invalid password");
                else
                {
                    BinaryFormatter d = new BinaryFormatter();
                    List<SerializableDirectoryTree> l = d.Deserialize(new MemoryStream(decrypted)) as List<SerializableDirectoryTree>;
                    Page1 p = new Page1();
                    p.MasterPassword = MasterPasswordBox.Password;
                    foreach (var item in l)
                    {
                        p.DirectoryTree.Items.Add(item.TurnIntoTreeView(p));
                    }
                    NavigationService.Navigate(p);
                }
            }
        }
    }
}
