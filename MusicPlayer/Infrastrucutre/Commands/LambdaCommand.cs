using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Infrastrucutre.Commands.Base;

namespace MusicPlayer.Infrastrucutre.Commands
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> _execute;

        private readonly Func<object, bool> _canExecute;

        public LambdaCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute ?? throw new ArgumentNullException();
        }
        public override bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }
    
        public override void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
