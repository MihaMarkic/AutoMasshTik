using System;
using System.Windows.Input;

namespace AutoMasshTik.Engine.Core
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> canExecute;
        private readonly Action<T> execute;

        public event EventHandler CanExecuteChanged;
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);
        public virtual bool CanExecute(T parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        void ICommand.Execute(object parameter) => Execute((T)parameter);
        public virtual void Execute(T parameter)
        {
            execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
