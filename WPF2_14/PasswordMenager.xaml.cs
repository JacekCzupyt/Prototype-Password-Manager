using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PasswordMenager.xaml
    /// </summary>
    public partial class PasswordMenager : Page
    {
        List<PasswordData> passwordList;
        CollectionViewSource PasswordViewSource;
        public PasswordData CurrentlyEdited = null;

        List<PasswordData> test = new List<PasswordData> { new PasswordData { AccountName = "test1" } };

        public PasswordMenager()
        {
            InitializeComponent();
        }

        private void AddPassword(object sender, RoutedEventArgs e)
        {
            var newPass = new PasswordData { AccountName = "Account name" };
            passwordList.Add(newPass);
            PasswordListView.SelectedItem = newPass;
            PasswordViewSource.View.Refresh();
            EditButtonClick(null, null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is List<PasswordData>)
            {
                passwordList = this.DataContext as List<PasswordData>;
            }
            else throw new Exception();

            PasswordViewSource = (FindResource("PasswordList") as CollectionViewSource);

            PasswordViewSource.Filter += PasswordViewSource_Filter;

            PasswordForm.DataContext = CurrentlyEdited;
        }

        private void PasswordViewSource_Filter(object sender, FilterEventArgs e)
        {
            if(e.Item is PasswordData)
                e.Accepted = (e.Item as PasswordData).AccountName.ToLower().Contains(SearchBox.Text.ToLower());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordViewSource.View.Refresh();
        }

        private void PasswordListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasswordListView.SelectedItem != null)
            {
                PasswordDisplay.Visibility = Visibility.Visible;
                PasswordDisplayBox.Password = (PasswordListView.SelectedItem as PasswordData).Password;
            }
                
            else
                PasswordDisplay.Visibility = Visibility.Collapsed;
        }

        private void IconButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images |*.png;*.jpeg";
            if (dialog.ShowDialog() == true)
            {
                CurrentlyEdited.IconImage = new BitmapImage(new Uri(dialog.FileName));
                CurrentlyEdited.SerializedBitmap = CurrentlyEdited.IconImage.ToBytes();
            }
            //Image img = new Image();
            //img.Source = CurrentlyEdited.IconImage;
            //(sender as Button).Content = img;

        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            CurrentlyEdited = (PasswordListView.SelectedItem as PasswordData).Copy();
            PasswordForm.DataContext = CurrentlyEdited;



            PasswordDisplay.Visibility = Visibility.Collapsed;
            PasswordForm.Visibility = Visibility.Visible;
            PasswordListView.IsEnabled = false;
            AddButton.IsEnabled = false;
            SearchBox.IsEnabled = false;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            passwordList.Remove(PasswordListView.SelectedItem as PasswordData);
            PasswordViewSource.View.Refresh();
        }

        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentlyEdited.CreationTime == DateTime.MinValue)
            {
                CurrentlyEdited.CreationTime = DateTime.Now;
                CurrentlyEdited.LastEditTime = DateTime.Now;
            }   
            else if(!CurrentlyEdited.IsEqualTo(PasswordListView.SelectedItem as PasswordData))
                CurrentlyEdited.LastEditTime = DateTime.Now;

            int ind = passwordList.IndexOf(PasswordListView.SelectedItem as PasswordData);
            passwordList[ind] = CurrentlyEdited;
            PasswordViewSource.View.Refresh();
            PasswordListView.SelectedItem = CurrentlyEdited;

            CurrentlyEdited = null;

            PasswordDisplay.Visibility = Visibility.Visible;
            PasswordForm.Visibility = Visibility.Collapsed;
            PasswordListView.IsEnabled = true;
            AddButton.IsEnabled = true;
            SearchBox.IsEnabled = true;
            
        }

        public void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentlyEdited.CreationTime == DateTime.MinValue)
            {
                passwordList.Remove(PasswordListView.SelectedItem as PasswordData);
                PasswordViewSource.View.Refresh();
            }
                

            CurrentlyEdited = null;

            if(PasswordListView.SelectedItem!=null)
                PasswordDisplay.Visibility = Visibility.Visible;
            PasswordForm.Visibility = Visibility.Collapsed;
            PasswordListView.IsEnabled = true;
            AddButton.IsEnabled = true;
            SearchBox.IsEnabled = true;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText((sender as Button).DataContext as string);
        }

        private void WebsiteHyperlinkClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start((sender as Hyperlink).DataContext as string);
        }

        private void EmailHyperlinkClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + (sender as Hyperlink).DataContext as string);
        }
    }

    class PasswordToBarLength : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                int str = (int)PasswordStrengthUtils.CalculatePasswordStrength(value as string);
                return $"{str}*";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PasswordToBarLengthInverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                int str = (int)PasswordStrengthUtils.CalculatePasswordStrength(value as string);
                return $"{5-str}*";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PasswordToBarColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                int str = (int)PasswordStrengthUtils.CalculatePasswordStrength(value as string);
                switch(str)
                {
                    case 0:
                        return Brushes.Red;
                    case 1:
                        return Brushes.Red;
                    case 2:
                        return Brushes.Orange;
                    case 3:
                        return Brushes.Yellow;
                    case 4:
                        return Brushes.Lime;
                    case 5:
                        return Brushes.Green;
                    default:
                        return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PasswordStrengthName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                int str = (int)PasswordStrengthUtils.CalculatePasswordStrength(value as string);
                return Enum.GetName(typeof(PasswordStrength), str);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                if (value as string == null || value as string == "")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class FirstLetter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string && (value as string).Length>0)
            {
                return (value as string)[0];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class HideEmpty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                if (value as string == null || value as string == "")
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class HideSelectButtonText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ImageToData : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is BitmapImage && value!=null)
            {
                var img = value as BitmapImage;
                return $"Resolution: {(int)img.Width}x{(int)img.Height}\n" +
                    $"DPI: {img.DpiX}x{img.DpiY}\n" +
                    $"Format: {img.Format}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
