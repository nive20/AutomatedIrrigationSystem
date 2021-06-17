using System;
using System.Windows.Input;

namespace WaterAllocationConsole
{
    /// <summary>
    /// A Command that hooks up to the CommandManager, which makes a requery everytime focus changes.
    /// Use this Command if you want the CanExecute Event fired everytime user changes Focus.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Delegate invoked as the pluggable implementation of the ICommand CanExecute() method.
        /// </summary>
        internal readonly Predicate<object> _canExecute;

        /// <summary>
        /// Delegate invoked as the pluggable implementation of the ICommand Execute() method.
        /// </summary>
        private Action<object> _execute;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">Action execute</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">Action execute</param>
        /// <param name="canExecute">Predicate execute</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this(_ => execute(), _ => canExecute())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class. 
        /// Constructor for DelegateCommand class
        /// </summary>
        /// <param name="execute">
        /// Delegate invoked when Command Executed
        /// </param>
        /// <param name="canExecute">
        /// Delegate invoked when Command CanExecute event invoked
        /// </param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        #endregion Constructors and Destructors

        #region Public Events

        /// <summary>
        /// Event hook to the CommandManager
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion Public Events

        #region Public Methods and Operators

        /// <summary>
        /// Method wrapper for CanExecute method.  Delegates to the constructor-injected delegate.
        /// </summary>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        /// <summary>
        /// Common method to clear all references
        /// </summary>
        public virtual void CleanReferences()
        {
            this._execute = null;
        }

        /// <summary>
        /// Method wrapper for Execute method.  Delegates to the constructor-injected delegate.
        /// </summary>
        /// <param name="parameter">parameter</param>
        public void Execute(object parameter)
        {
            try
            {
                this._execute(parameter);
            }
            catch (Exception)
            {
            }
        }

        #endregion Public Methods and Operators
    }
}
