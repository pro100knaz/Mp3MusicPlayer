using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MusicPlayer.ViewModels.Base;

namespace MusicPlayer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private BitmapImage musicBitmapImage;
        public MainWindowViewModel()
        {
            string path = Environment.CurrentDirectory;
            //_images.Add(new BitmapImage(new Uri($@"{path}\Images\Deer.jpg")));
            //C:\Users\jffgx\OneDrive\Рабочий стол\Projects\Mp3Player\MusicPlayer\MusicPlayer\Data\Images\MusicIcon.png
            musicBitmapImage = new BitmapImage(new Uri($@"{path}\Data\Images\MusicIcon.png"));
        }
    }
}
