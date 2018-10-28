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
        private string defaultPlaylist = "All Music";

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
            string playlistName = playlistBox.SelectedValue.ToString();
            changePlaylistSource(musicLib.SongsForPlaylist(playlistName));
        }


        private void changePlaylistSource(DataTable sourceTableFull)
        {
            DataTable viewableSource = new DataTable();
            viewableSource.Columns.Add(new DataColumn("id", typeof(int)));
            viewableSource.Columns.Add(new DataColumn("title", typeof(string)));
            viewableSource.Columns.Add(new DataColumn("artist", typeof(string)));
            viewableSource.Columns.Add(new DataColumn("album", typeof(string)));
            viewableSource.Columns.Add(new DataColumn("genre", typeof(string)));


            foreach (DataRow row in sourceTableFull.Rows)
            {
                DataRow newRow = viewableSource.NewRow();

                newRow["id"] = row.ItemArray[0];
                newRow["title"] = row.ItemArray[1];
                newRow["artist"] = row.ItemArray[2];
                newRow["album"] = row.ItemArray[3];
                newRow["genre"] = row.ItemArray[6];

                viewableSource.Rows.Add(newRow);
            }

            playlistSongs.ItemsSource = viewableSource.DefaultView;

        }

    }
}
