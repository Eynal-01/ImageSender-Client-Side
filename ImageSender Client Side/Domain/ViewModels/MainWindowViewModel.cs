using ImageSender_Client_Side.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ImageSender_Client_Side.Domain.ViewModels
{
    public class MainWindowViewModel
    {
        public RelayCommand SelectImageCommand { get; set; }
        public RelayCommand SendImageCommand { get; set; }
        public Image Image { get; set; }

        public MainWindowViewModel()
        {
            SelectImageCommand = new RelayCommand((obj) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
            });
        }
    }
}
