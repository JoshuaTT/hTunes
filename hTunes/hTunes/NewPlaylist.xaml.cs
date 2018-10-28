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
using System.Windows.Shapes;

namespace hTunes
{
    /// <summary>
    /// Interaction logic for NewPlaylist.xaml
    /// </summary>
    public partial class NewPlaylist : Window
    {
        MusicLib musicLib = new MusicLib();
        public NewPlaylist()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string newPlaylist = newPlaylistTextBox.Text;
            musicLib.AddPlaylist(newPlaylist);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
