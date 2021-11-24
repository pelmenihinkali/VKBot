using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NekitVKBot
{
    /// <summary>
    /// Логика взаимодействия для settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        internal bool isClosed = false;
        public Settings()
        {
            InitializeComponent();
            tokenBox.Text = MainWindow.vk.Token;
            idBox.Text = MainWindow.vk.IDGroup.ToString();
            delayBox.Text = MainWindow.vk.Delay.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            isClosed = true;
            
            MainWindow.vk.Token = tokenBox.Text;
            MainWindow.vk.IDGroup = Convert.ToUInt64(idBox.Text);
            MainWindow.vk.Delay = Convert.ToInt32(delayBox.Text);

            MainWindow.SaveData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isClosed = false;
            
        }
    }
}
