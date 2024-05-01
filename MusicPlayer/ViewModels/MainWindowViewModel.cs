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

        private CurrentTimer _musicTimer;

        public CurrentTimer MusicTimer
        {
            get => _musicTimer;
            set => SetField(ref _musicTimer, value);
        }



        public int CurrentSongTimeInSeconds
        {
            get => MusicTimer.CurrentTimeInSecond;
            set
            {
                    MusicTimer.CurrentTimeInSecond = value;
                    Player.Position = TimeSpan.FromSeconds(value);
            }
        }

        //public int CurrentSongTimeInSeconds
        //{
        //    get => _musicTimer.CurrentTimeInSecond;
        //    set
        //    {
        //        _currentSongTimeInSeconds = value;
        //        // Передаем значение в MusicTimer и Player
        //        MusicTimer.CurrentTimeInSecond = value;
        //        Player.Position = TimeSpan.FromSeconds(value);
        //    }
        //}


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

        private void ccc()
        {
            Player.Dispatcher.Invoke(() => Player.Position = TimeSpan.FromSeconds(CurrentSongTimeInSeconds));
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
                while (!Player.NaturalDuration.HasTimeSpan)
                {
                    if (Player.NaturalDuration.HasTimeSpan)
                    {
                        break;
                    }
                }
                MusicTimer = new CurrentTimer( Player,  Player.NaturalDuration.TimeSpan.Seconds + Player.NaturalDuration.TimeSpan.Minutes * 60);
                MusicTimer.Start();
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
            
            if (!_isPlaying)
            {
                
                Player.Play();
                MusicTimer._isPlaying = true;
                _isPlaying = true;
              
            }
            else
            {
                Player.Pause();
                MusicTimer._isPlaying = false;
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





        private void YourEventHandler(object sender, int newValue)
        {
            CurrentSongTimeInSeconds = newValue;
        }

        #endregion

        #region TimeChangedCommand


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

            MusicTimer = new CurrentTimer(Player);
        }
    }

    internal class CurrentTimer : ViewModel
    {
        #region свойства

        #region MediaPlayer Player - "Музыкальный проигрыватель"

        ///<summary> Музыкальный проигрыватель </summary>
        private MediaPlayer _Player;

        ///<summary> Музыкальный проигрыватель </summary>
        public MediaPlayer Player
        {
            get => _Player;
            set => SetField(ref _Player, value);
        }

        #endregion

        private int _currentTimeInSecond = 0;
        public int CurrentTimeInSecond 
        {
            get => _currentTimeInSecond;
            set
            {
                SetField(ref _currentTimeInSecond, value);
                Player.Dispatcher.Invoke(() => Player.Position = TimeSpan.FromSeconds(_currentTimeInSecond));
            }
        }

        private int _maxTimeInSecond;
        public int MaxTimeInSecond
        {
            get => _maxTimeInSecond;
            set => SetField(ref _maxTimeInSecond, value);
        }
        public bool _isPlaying = false;

        public TimeSpan MaxTimeInMinutes
        {
            get => TimeSpan.FromSeconds(MaxTimeInSecond);
        }

        public double CurrentTimeInMinutes
        {
            get
            {
                int minutes = CurrentTimeInSecond / 60;
                if (minutes >= 1)
                {
                     return (double)(minutes + (double)((CurrentTimeInSecond % (minutes * 60)) / 100));

                }
                else
                {
                    return 0 + CurrentTimeInSecond / 100;
                }
            }
        }

        #endregion
        public CurrentTimer( MediaPlayer player)
        {
            Player = player;
            callback = new TimerCallback(TimerCallbackMethod);
            timer = new Timer(callback, state, dueTime, period);
        }
        public CurrentTimer(MediaPlayer player, int maxTimeInSecond) : this(player)
        {
            MaxTimeInSecond = maxTimeInSecond;
        }


        TimerCallback callback;
        Timer timer;

        int dueTime = 0; // таймер начнет срабатывать сразу после создания
        int period = 1000; // интервал срабатывания таймера - 1 секунда
        object state = null;
        void TimerCallbackMethod(object state)
        {
            if (_isPlaying && CurrentTimeInSecond <= MaxTimeInSecond)
            {
                CurrentTimeInSecond += 1;
            }
        }
        public void Start()
        {
            CurrentTimeInSecond = 0;
            _isPlaying = false;
        }

        public void Stop()
        {
            _isPlaying = false;
            timer?.Dispose();
            timer = null;
        }
    }

}
