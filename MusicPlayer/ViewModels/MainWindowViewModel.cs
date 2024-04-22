using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MaterialDesignThemes.Wpf;
using MusicPlayer.ViewModels.Base;
using System.Windows.Threading;

namespace MusicPlayer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Свойства обьектов

        #region CurrentSongConnectionString


        public string _currentSongConnectionString = String.Empty;
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


        private double _totalTime;
        public double TotalTime
        {
            get => _totalTime;
            set => SetField(ref _totalTime, value);
        }

        private int _musicTime;
        public int MusicTime
        {
            get => _musicTime;
            set
            {
                int currentTimeValue = value;
                //в секундах 
                /*
                 *int minutes = (int)currentTimeValue;
                   double doubleSeconds = currentTimeValue % 1;
                   int seconds = (int)(doubleSeconds * 100);
                 */
                
                TimeSpan result = new TimeSpan(0, 0, 0, value);
                Player.Position = result;
                SetField(ref _musicTime, value);
            } 
        }



        #endregion

        #region CurrentSongName

        private string _currentSongName = "Выберите песню";
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

                CurrentSongName = openFileDialog.SafeFileName;

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
        private bool CanPlayAndStopMusicCommandExecute(object p) => CurrentSongConnectionString != string.Empty;

        private void OnPlayAndStopMusicCommandExecuted(object p)
        {

            ///Надо изменить сккоро !!!!!!!!!!!!!!!!!
             int minutesInt = Player.NaturalDuration.TimeSpan.Minutes;
            double secondsInt = Player.NaturalDuration.TimeSpan.Seconds;

            TotalTime = minutesInt + (secondsInt / 100);

            
            if (!_isPlaying)
            {
                Player.Play();
                _isPlaying = true;
              
            }
            else
            {
                Player.Pause();
                _isPlaying = false;
              

            }

            ChangeImagePlayAndStopCommand.Execute(null);
        }


        #endregion

        #region ChangeImagePlayAndStopCommand 

        private ObservableCollection<PackIcon> IconsPlayAndStopCollection;

        private PackIcon _firstIcon;
        public PackIcon FirstIcon
        {
            get => _firstIcon;
            set => SetField(ref _firstIcon, value);
        }


        private ICommand _changeImagePlayAndStopCommand;

        public ICommand ChangeImagePlayAndStopCommand
        {
            get => _changeImagePlayAndStopCommand;
            set => SetField(ref _changeImagePlayAndStopCommand, value);
        }

        private bool CanChangeImagePlayAndStopCommandExecute(object p) => true;

        private void OnChangeImagePlayAndStopCommandExecuted(object p)
        {
            IconsPlayAndStopCollection.Move(0, 1);
            FirstIcon = IconsPlayAndStopCollection[0];
            // 1 Пусть отображается первый элемент ObservableCollection тогда при нажатии кнопки они будут меняться местами(выбрал этот вариант)

            // 2 Пусть отображается один из двух если сейчас отображается первый то будет отображаться в следующий раз
            //тогда будет проверять в параметре какой обьект установлен сейчас и будем устанавливать тот который сейчас не стоит


        }



        #endregion

        private ICommand _timeChangedCommand;
        public ICommand TimeChangedCommand
        {
            set => SetField(ref _timeChangedCommand, value);
            get => _timeChangedCommand;
        }

        private bool CanTimeChangedCommandExecute(object? p) => _isPlaying;

        private void OnTimeChangedCommandExectuted(object? p)
        {
            //TimeSpan CurrentTime = Player.Position;
            //double minutesDouble = CurrentTime.Minutes;
            //int secondsDouble = CurrentTime.Seconds;
            //MusicTime = secondsDouble;
        }


        #endregion

        #region Test

        


        #endregion
        public MainWindowViewModel()
        {

            #region Commands


            OpenMusicFileCommand = new LambdaCommand(OnOpenMusicFileCommandExecuted, CanOpenMusicFileCommandExecute);

            PlayAndStopMusicCommand =
                new LambdaCommand(OnPlayAndStopMusicCommandExecuted, CanPlayAndStopMusicCommandExecute);

            ChangeImagePlayAndStopCommand = new LambdaCommand(OnChangeImagePlayAndStopCommandExecuted,
                CanChangeImagePlayAndStopCommandExecute);

            TimeChangedCommand = new LambdaCommand(OnTimeChangedCommandExectuted, CanTimeChangedCommandExecute);

            #endregion

            var playIcon = new PackIcon { Kind = PackIconKind.Play, Width = 20, Height = 20 };
            var stopIcon = new PackIcon { Kind = PackIconKind.Stop, Width = 20, Height = 20 };
            FirstIcon = playIcon;
            IconsPlayAndStopCollection = new ObservableCollection<PackIcon>() { playIcon, stopIcon };
         

        }
    }
}
