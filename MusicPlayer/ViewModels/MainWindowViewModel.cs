using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using MusicPlayer.Infrastrucutre.Commands;
using MusicPlayer.ViewModels.Base;

namespace MusicPlayer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Свойства обьектов

            #region CurrentSongConnectionString


            public string _currentSongConnectionString;
                public string CurrentSongConnectionString
                {
                    get => _currentSongConnectionString;
                    set => SetField(ref _currentSongConnectionString, value);
                }

        #endregion

            #region MediaPlayer


            private MediaPlayer _player = new MediaPlayer();

            public MediaPlayer Player 
            {
                get => _player;
                set => SetField(ref _player, value);
            }

        #endregion

            #region CurrentSongName

        private string _currentSongName = String.Empty;
            public string CurrentSongName
            {
                get => _currentSongName;
                set => SetField(ref _currentSongName, value);
            }

        #endregion


        #endregion

        #region Команды

        #region OpenMusicFileCommand

        private ICommand _openMusicFileCommand;
        public ICommand OpenMusicFileCommand
        {
            get => _openMusicFileCommand;
            set => SetField(ref _openMusicFileCommand, value);
        }
        private bool CanOpenMusicFileCommandExecute(object p) => true;

        private void OnOpenMusicFileCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"; // Указываем фильтр для файлов
            if (openFileDialog.ShowDialog() == true)
            {
                CurrentSongConnectionString = openFileDialog.FileName;

                CurrentSongName =  openFileDialog.SafeFileName;

                Player.Open(new Uri(CurrentSongConnectionString));

            }
        }


        #endregion


        #region PlayAndStopMusicCommand

        private ICommand _playAndStopMusicCommand;
        private bool _isPlaying = false;
        public ICommand PlayAndStopMusicCommand
        {
            get => _playAndStopMusicCommand;
            set => SetField(ref _playAndStopMusicCommand, value);
        }
        private bool CanPlayAndStopMusicCommandExecute(object p) => true;

        private void OnPlayAndStopMusicCommandExecuted(object p)
        {
            if (!_isPlaying)
            {
                Player.Play();
                _isPlaying = true;
            }
            else
            {
                Player.Stop();
                _isPlaying = false;
            }
        }


        #endregion

        #endregion
        public MainWindowViewModel()
        {
            
            #region Commands

            
            OpenMusicFileCommand = new LambdaCommand(OnOpenMusicFileCommandExecuted, CanOpenMusicFileCommandExecute);

            PlayAndStopMusicCommand =
                new LambdaCommand(OnPlayAndStopMusicCommandExecuted, CanPlayAndStopMusicCommandExecute);

            #endregion

        }
    }
}
