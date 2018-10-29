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
        private MediaPlayer mediaPlayer = new MediaPlayer();
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
            if (playlistBox.HasItems)
            {
                currentPlaylist = playlistBox.SelectedValue.ToString();

                if (currentPlaylist != defaultPlaylist)
                    playlistSongs.IsReadOnly = true;
                else
                    playlistSongs.IsReadOnly = false;

                changePlaylistSource(musicLib.SongsForPlaylist(currentPlaylist));
            }
        }


        private void changePlaylistSource(DataTable sourceTableFull)
        {
            playlistSongs.ItemsSource = sourceTableFull.DefaultView;
        }

        private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            playSong();
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
                if (MessageBox.Show("Are you sure you want to remove this song?", "Delete Song", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRowWrapper = playlistSongs.SelectedItem as DataRowView;
                    DataRow selectedRow = selectedRowWrapper.Row;
                    int toRemoveId = (int)selectedRow.ItemArray[0];
                    musicLib.DeleteSong(toRemoveId);
                    changePlaylistSource(musicLib.SongsForPlaylist(currentPlaylist));
                }
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
                string toRemoveIdAsString = selectedRow.ItemArray[0].ToString();
                int toRemoveId;
                Int32.TryParse(toRemoveIdAsString, out toRemoveId);

                musicLib.RemoveSongFromPlaylist(toRemoveIndex + 1, toRemoveId, currentPlaylist);
                changePlaylistSource(musicLib.SongsForPlaylist(currentPlaylist));
            }
            catch(Exception)
            {
                bool stop = true;
            }
        }

        private void RenamePlaylist_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string oldName = playlistBox.SelectedItem.ToString();

            RenamePlaylistWindow renamePlaylistWindow = new RenamePlaylistWindow(oldName);
            renamePlaylistWindow.ShowDialog();
            bool renamePlaylist = renamePlaylistWindow.WasOKClicked;
            if (renamePlaylist == true)
            {
                string newPlaylistName = renamePlaylistWindow.getPlaylistName;
                musicLib.RenamePlaylist(oldName, newPlaylistName);
                playlistBox.Items.Clear();
                addPlaylists();
                //playlistBox.Items.Add(newPlaylistName);
            }

        }

        private void RemovePlaylist_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            musicLib.DeletePlaylist(currentPlaylist);
            playlistBox.Items.Clear();
            addPlaylists();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            playSong();
        }

        private void IfNotDefault_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool notDefault = true;

            if (playlistBox.SelectedItem.ToString() == defaultPlaylist)
                notDefault = false;

            e.CanExecute = notDefault;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void playSong()
        {
            DataRowView rowView = playlistSongs.SelectedItem as DataRowView;
            if (rowView != null)
            {
                mediaPlayer.Open(new Uri(rowView.Row.ItemArray[4].ToString()));
                mediaPlayer.Play();
            }
        }

        private Point startPoint;

        private void playlistSongs_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void playlistSongs_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                // Start the drag-drop if mouse has moved far enough
                if (e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    DataRowView selectedRowWrapper = playlistSongs.SelectedItem as DataRowView;
                    int selectedSongId = (int)selectedRowWrapper.Row.ItemArray[0];
                    DragDrop.DoDragDrop(playlistSongs, selectedSongId, DragDropEffects.Copy);
                }
            }
            catch(Exception)
            {  }

        }        

        private void playlistBox_Drop(object sender, DragEventArgs e)
        {
            

            // If the DataObject contains string data, extract it
            if (e.Data.GetDataPresent(typeof(int)))
            {
                TextBlock overItem = e.OriginalSource as TextBlock;

                int addedSongId = (int)e.Data.GetData(typeof(int));

                musicLib.AddSongToPlaylist(addedSongId, overItem.Text);
            }

        }

        private void playlistBox_DragOver(object sender, DragEventArgs e)
        {

            try
            {
                TextBlock overItem = e.OriginalSource as TextBlock;

                if(musicLib.PlaylistExists(overItem.Text))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else if(overItem.Text == defaultPlaylist)
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            catch(Exception)
            {
                e.Effects = DragDropEffects.None;
            }
        }



        //private void searchTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    (playlistSongs.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%'", searchBar.Text);
        //}
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand Play = new RoutedUICommand("Play", "Play", typeof(MainWindow));
        public static readonly RoutedUICommand RemoveSong = new RoutedUICommand("RemoveSong", "RemoveSong", typeof(MainWindow));
        public static readonly RoutedUICommand RemoveFromPlaylist = new RoutedUICommand("RemoveFromPlaylist", "RemoveFromPlaylist", typeof(MainWindow));
        public static readonly RoutedUICommand RenamePlaylist = new RoutedUICommand("RenamePlaylist", "RenamePlaylist", typeof(MainWindow));
        public static readonly RoutedUICommand RemovePlaylist = new RoutedUICommand("RemovePlaylist", "RemovePlaylist", typeof(MainWindow));
    }
}
