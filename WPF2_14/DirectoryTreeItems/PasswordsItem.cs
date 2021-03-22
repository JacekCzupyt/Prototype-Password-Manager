using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF2_14.DirectoryTreeItems
{
    class PasswordItem : AbstractDirectoryTreeItem
    {
        public List<PasswordData> PassowrdList = new List<PasswordData>();

        public override object CreateDataContext()
        {
            return PassowrdList;
        }

        public override Page GetPage()
        {
            return new PasswordMenager();
        }

        //public override string Serialize()
        //{
        //    String s = "<PasswordItem>\n" + base.Serialize() + $"\n<Header>{this.Header}</Header>\n";
        //    s += "<PasswordList>\n";
        //    foreach (PasswordData pass in PassowrdList)
        //    {
        //        s += pass.Serialize() + "\n";
        //    }
        //    s += "</PasswordList> \n </PasswordItem>";
        //    return s;
        //}

        public PasswordItem(string s, Page1 page)
        {
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

    [Serializable]
    public class PasswordData : INotifyPropertyChanged
    {
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Website { get; set; }
        public string Notes { get; set; }
        //public string Icon { get; set; }
        public BitmapImage IconImage { get { return BackingImage; } set { BackingImage = value; OnPropertyChanged(); } }
        [NonSerialized] private BitmapImage BackingImage = null;

        [field:NonSerialized]public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public byte[] SerializedBitmap { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastEditTime { get; set; }

        public PasswordData Copy()
        {
            return this.MemberwiseClone() as PasswordData;
        }

        public bool IsEqualTo(PasswordData p)
        {
            return p.AccountName == AccountName &&
                p.Email == Email &&
                p.Login == Login &&
                p.Password == Password &&
                p.Website == Website &&
                p.Notes == Notes &&
                p.SerializedBitmap == SerializedBitmap;
        }

        //public string Serialize()
        //{
        //    String s = "<PasswordData>\n";
        //    s += $"<AccountName>{AccountName}</AccountName>\n";
        //    s += $"<Email>{Email}</Email>\n";
        //    s += $"<Login>{Login}</Login>\n";
        //    s += $"<Password>{Password}</Password>\n";
        //    s += $"<Website>{Website}</Website>\n";
        //    s += $"<Notes>{Notes}</Notes>\n";
        //    //s += $"<Icon>{Icon}</Icon>\n";
        //    s += "</PasswordData>";
        //    return s;
        //}

        public PasswordData() { }

        //public PasswordData(String s)
        //{
        //    foreach (System.Text.RegularExpressions.Match m in
        //        System.Text.RegularExpressions.Regex.Matches(s, @"<AccountName>(.*)</AccountName>"))
        //    {
        //        AccountName = m.Groups[1].Value;
        //    }

        //    foreach (System.Text.RegularExpressions.Match m in
        //        System.Text.RegularExpressions.Regex.Matches(s, @"<Email>(.*)</Email>"))
        //    {
        //        Email = m.Groups[1].Value;
        //    }

        //    foreach (System.Text.RegularExpressions.Match m in
        //        System.Text.RegularExpressions.Regex.Matches(s, @"<Login>(.*)</Login>"))
        //    {
        //        Login = m.Groups[1].Value;
        //    }

        //    foreach (System.Text.RegularExpressions.Match m in
        //        System.Text.RegularExpressions.Regex.Matches(s, @"<Password>(.*)</Password>"))
        //    {
        //        Password = m.Groups[1].Value;
        //    }

        //    foreach (System.Text.RegularExpressions.Match m in
        //        System.Text.RegularExpressions.Regex.Matches(s, @"<Website>(.*)</Website>"))
        //    {
        //        Website = m.Groups[1].Value;
        //    }

        //    foreach (System.Text.RegularExpressions.Match m in
        //        System.Text.RegularExpressions.Regex.Matches(s, @"<Notes>(.*)</Notes>"))
        //    {
        //        Notes = m.Groups[1].Value;
        //    }

        //    //foreach (System.Text.RegularExpressions.Match m in
        //    //    System.Text.RegularExpressions.Regex.Matches(s, @"<Icon>(*)</Icon>"))
        //    //{
        //    //    Icon = m.Groups[1].Value;
        //    //}
        //}
    }
}
