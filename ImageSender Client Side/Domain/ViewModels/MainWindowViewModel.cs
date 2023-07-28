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
using System.Net;
using System.Net.Sockets;
using System.Windows;

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

        public bool IsConnected { get; set; } = false;

        public MainWindowViewModel()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SelectImageCommand = new RelayCommand((obj) =>
            {
                File_send(Image);
            });

            SendImageCommand = new RelayCommand((obj) =>
            {

                var ipAddress = IPAddress.Parse("10.2.27.8");
                var port = 27001;

                Task.Run(() =>
                {
                    var ep = new IPEndPoint(ipAddress, port);
                    try
                    {
                        if (IsConnected == false)
                        {
                            socket.Connect(ep);

                            if (socket.Connected)
                            {
                                MessageBox.Show("dewdew");
                                var imagesend = Image;
                                var bytes = GetJPGFromImageControl(Image);
                                socket.Send(bytes);
                            }
                        }
                        else
                        {
                            MessageBox.Show("dewdew");
                            var imagesend = Image;
                            var bytes = GetJPGFromImageControl(Image);
                            socket.Send(bytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            });
        }

        public void File_send(object parametr)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Image";
            dlg.DefaultExt = ".png";

            if (dlg.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(dlg.FileName));
                ImageBrush brush = new ImageBrush(Image);
                IsOkay = true;
            }
        }

        public byte[] GetJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
