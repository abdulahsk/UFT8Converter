using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UTF8Converter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private string _svg;

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

        }


        public event PropertyChangedEventHandler PropertyChanged;
    
        private void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Svg
        {
            get { return _svg; }
            set
            {
                _svg = value;
                OnPropertyChange();
            }
        }


        private async void Myimage_Loaded(object sender, RoutedEventArgs e)
        {
            var file = await Package.Current.InstalledLocation.GetFileAsync("Svg.txt");
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    Svg = dataReader.ReadString(buffer.Length);
                });



            }
            var svgBuffer = CryptographicBuffer.ConvertStringToBinary("svg", BinaryStringEncoding.Utf8);

        }
    }

    public class SVGImageConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var svg = new SvgImageSource();
            try
            {
                var svgBuffer = CryptographicBuffer.ConvertStringToBinary(value?.ToString(), BinaryStringEncoding.Utf8);

                using (var stream = svgBuffer.AsStream())
                {
                    svg.SetSourceAsync(stream.AsRandomAccessStream()).AsTask().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {

            }

            return svg;
        }
    }
}
