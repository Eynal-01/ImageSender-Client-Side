using ImageSender_Client_Side.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace ImageSender_Client_Side.Domain.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public RelayCommand SelectImageCommand { get; set; }
        public RelayCommand SendImageCommand { get; set; }
        public RelayCommand ConnectServerCommand { get; set; }

        private BitmapImage image;

        public BitmapImage Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }

        public Socket Socket { get; set; }

        public byte[] Data { get; set; }

        public bool IsConnected { get; set; } = false;

        [Obsolete]
        public MainWindowViewModel()
        {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            ConnectServerCommand = new RelayCommand((obj) =>
            {
                var ipAddress = IPAddress.Parse(myIP);
                var port = 27001;
                if (IsConnected == false)
                {
                    Task.Run(() =>
                    {
                        var ep = new IPEndPoint(ipAddress, port);
                        try
                        {
                            IsConnected = true;
                            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            Socket.Connect(ep);

                            if (Socket.Connected)
                            {
                                MessageBox.Show("Connected successfully");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });
                }
                else
                {
                    MessageBox.Show("Already connected");
                }
            });

            SelectImageCommand = new RelayCommand((obj) =>
            {
                File_send(Image);
            });
            //
            SendImageCommand = new RelayCommand((obj) =>
            {
                if (IsConnected == true)
                {

                    try
                    {
                        var imagesend = Image;
                        var bytes = GetJPGFromImageControl(Image);
                        Socket.Send(bytes);
                        MessageBox.Show("Image sended successfully");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
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
