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

            addPlaylists();
            changePlaylistSource(musicLib.SongsForPlaylist(defaultPlaylist));
        }


        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.FileName = "";
            openFileDialog.DefaultExt = "*.wma;*.wav;*mp3";
            openFileDialog.Filter = "Media files|*.mp3;*.m4a;*.wma;*.wav|MP3 (*.mp3)|*.mp3|M4A (*.m4a)|*.m4a|Windows Media Audio (*.wma)|*.wma|Wave files (*.wav)|*.wav|All files|*.*";

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            // Load the selected song
            if (result == true)
            {
                Song newSong = GetSongDetails(openFileDialog.FileName);
                if (newSong != null)
                {
                    musicLib.AddSong(newSong);
                }
            }
        }
        private void newPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var newPlaylist = new NewPlaylist();
            newPlaylist.ShowDialog();
            bool addPlaylist = newPlaylist.WasOKClicked;
            if (addPlaylist == true)
            {
                string newPlaylistName = newPlaylist.getPlaylistName;
                musicLib.AddPlaylist(newPlaylistName);
                playlistBox.Items.Add(newPlaylistName);
            }
        }
        private Song GetSongDetails(string filename)
        {
            Song song = null;

            try
            {
                // PM> Install-Package taglib
                // http://stackoverflow.com/questions/1750464/how-to-read-and-write-id3-tags-to-an-mp3-in-c
                TagLib.File file = TagLib.File.Create(filename);

                song = new Song
                {
                    Title = file.Tag.Title,
                    Artist = file.Tag.AlbumArtists.Length > 0 ? file.Tag.AlbumArtists[0] : "",
                    Album = file.Tag.Album,
                    Genre = file.Tag.Genres.Length > 0 ? file.Tag.Genres[0] : "",
                    Length = file.Properties.Duration.Minutes + ":" + file.Properties.Duration.Seconds,
                    Filename = filename
                };

                return song;
            }
            catch (TagLib.UnsupportedFormatException)
            {
                MessageBox.Show("You did not select a valid song file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return song;
        }

        // Ask Josh
        private void addPlaylists()
        {
            playlistBox.Items.Add("All Music");
            var playlists = musicLib.Playlists;
            foreach (var name in playlists)
            {
                musicLib.AddPlaylist(name);
                playlistBox.Items.Add(name);
            }
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
            try
            {
                DataRowView selectedRowWrapper = playlistSongs.SelectedItem as DataRowView;
                DataRow selectedRow = selectedRowWrapper.Row;
                int toRemoveId = (int)selectedRow.ItemArray[0];
                musicLib.DeleteSong(toRemoveId);
            }
            catch(Exception)
            { }
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
            int toRemoveIndex = playlistSongs.SelectedIndex;

            try
            {
                DataRowView selectedRowWrapper = playlistSongs.SelectedItem as DataRowView;
                DataRow selectedRow = selectedRowWrapper.Row;
                int toRemoveId = (int)selectedRow.ItemArray[0];

                musicLib.RemoveSongFromPlaylist(toRemoveIndex, toRemoveId, currentPlaylist);
            }
            catch(Exception)
            { }
        }

    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand Play = new RoutedUICommand("Play", "Play", typeof(MainWindow));
        public static readonly RoutedUICommand RemoveSong = new RoutedUICommand("RemoveSong", "RemoveSong", typeof(MainWindow));
        public static readonly RoutedUICommand RemoveFromPlaylist = new RoutedUICommand("RemoveFromPlaylist", "RemoveFromPlaylist", typeof(MainWindow));

    }
}
