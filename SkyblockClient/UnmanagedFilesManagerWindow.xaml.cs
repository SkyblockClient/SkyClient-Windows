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

namespace SkyblockClient
{
    /// <summary>
    /// Interaktionslogik für UnmanagedFilesManagerWindow.xaml
    /// </summary>
    public partial class UnmanagedFilesManagerWindow : Window
    {
        List<string> Files;
        public UnmanagedFilesManagerWindow(List<string> files)
        {
            Files = files;
            InitializeComponent();
        }
    }
}
