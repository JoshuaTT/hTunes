using System;
using System.Collections.Generic;
using System.Data;
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
        private MusicLib musicLib = new MusicLib();

        public MainWindow()
        {
            InitializeComponent();

            playlistBox.Items.Add("All Music");
        }



        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void newPlaylist_Click(object sender, RoutedEventArgs e)
        {
            NewPlaylist newPlalist = new NewPlaylist();
            newPlalist.ShowDialog();
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
