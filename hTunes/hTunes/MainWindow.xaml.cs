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

namespace hTunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand LaunchAboutPage = new RoutedCommand();
        private static MusicLib musicLib = new MusicLib();

        public MainWindow()
        {
            InitializeComponent();

            playlistBox.Items.Add("All Music");
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void playlistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = playlistBox;
            //bool stop = true;
            //stop = false;
           // playlistSongs = musicLib.SongsForPlaylist(playlistBox.SelectedValue);
        }
    }
}
