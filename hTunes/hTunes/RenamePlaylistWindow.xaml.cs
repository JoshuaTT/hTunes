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
    /// Interaction logic for RenamePlaylistWindow.xaml
    /// </summary>
    public partial class RenamePlaylistWindow : Window
    {

        bool renamePlaylist = false;
        private string myOldName;
        public RenamePlaylistWindow(string oldName)
        {
            InitializeComponent();
            myOldName = oldName;
            newNameTextBox.Text = oldName;
            newNameTextBox.SelectAll();
        }

        public bool WasOKClicked
        {
            get { return renamePlaylist; }
        }
        public string getPlaylistName
        {
            get { return newNameTextBox.Text; }
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            renamePlaylist = true;
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
