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
        bool addPlaylist = false;
        public NewPlaylist()
        {
            InitializeComponent();
        }

        public bool WasOKClicked
        {
            get { return addPlaylist; }
        }
        public string getPlaylistName
        {
            get { return newPlaylistTextBox.Text; }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            addPlaylist = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
