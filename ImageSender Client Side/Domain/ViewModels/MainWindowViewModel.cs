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
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace ImageSender_Client_Side.Domain.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public RelayCommand SelectImageCommand { get; set; }
        public RelayCommand SendImageCommand { get; set; }

        private BitmapImage image;

        public BitmapImage Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }

        public bool IsOkay { get; set; } = false;
        public byte[] Data { get; set; }

        public MainWindowViewModel()
        {
            SelectImageCommand = new RelayCommand((obj) =>
            {
                File_send(obj);
            });
        }
        public void File_send(object parametr)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Image"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            //dlg.Filter = "Image Files (.jpg)|*.jpg;*.jpeg;*.png;*.gif;*.tif;"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();
            if (dlg.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(dlg.FileName));
                ImageBrush brush = new ImageBrush(Image);
                //_MW.Background = brush;

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Image));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    Data = ms.ToArray();
                }
                IsOkay = true;
            }
        }
    }
}
