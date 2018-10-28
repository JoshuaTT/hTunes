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
        private const string defaultPlaylist = "All Music";
        private string currentPlaylist = "All Music";

        public MainWindow()
        {
            InitializeComponent();

            playlistBox.Items.Add("All Music");
            changePlaylistSource(musicLib.SongsForPlaylist(defaultPlaylist));
        }
        

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
        private void openButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void newPlaylist_Click(object sender, RoutedEventArgs e)
        {
            NewPlaylist newPlaylist = new NewPlaylist();
            newPlaylist.ShowDialog();
        }

        private void playlistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPlaylist = playlistBox.SelectedValue.ToString();

            if (currentPlaylist != defaultPlaylist)
                playlistSongs.IsReadOnly = true;
            else
                playlistSongs.IsReadOnly = false;

            changePlaylistSource(musicLib.SongsForPlaylist(currentPlaylist));
        }


        private void changePlaylistSource(DataTable sourceTableFull)
        {
            playlistSongs.ItemsSource = sourceTableFull.DefaultView;
        }

        private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RemoveSong_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool canRemove = false;

            if (defaultPlaylist == currentPlaylist)
                canRemove = true;

            e.CanExecute = canRemove;
        }

        private void RemoveSong_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RemoveFromPlaylist_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool canRemove = false;

            if (defaultPlaylist != currentPlaylist)
                canRemove = true;

            e.CanExecute = canRemove;

        }

        private void RemoveFromPlaylist_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand Play = new RoutedUICommand("Play", "Play", typeof(MainWindow));
        public static readonly RoutedUICommand RemoveSong = new RoutedUICommand("RemoveSong", "RemoveSong", typeof(MainWindow));
        public static readonly RoutedUICommand RemoveFromPlaylist = new RoutedUICommand("RemoveFromPlaylist", "RemoveFromPlaylist", typeof(MainWindow));

    }
}
