using System.Windows.Input;

namespace WaterAllocationConsole
{
    public class WAViewModel
    {
        public ICommand CloseCommand { get; private set; }
        public ICommand MinimizeCommand { get; private set; }


        public enum ButtonClickType
        {
            Close,
            Minimize
        }

        public delegate void WAEventHandler(ButtonClickType e);
        public static event WAEventHandler HeaderClick;

        public WAViewModel()
        {
            this.CloseCommand = new DelegateCommand(CloseCommandExecute, OnCanExecute);
            this.MinimizeCommand = new DelegateCommand(MinimizeCommandExecute, OnCanExecute);
        }

        private bool OnCanExecute(object obj)
        {
            return true;
        }

        private void MinimizeCommandExecute(object obj)
        {
            if (HeaderClick != null)
                HeaderClick(ButtonClickType.Minimize);
        }

        private void CloseCommandExecute(object obj)
        {
            if (HeaderClick != null)
                HeaderClick(ButtonClickType.Close);
        }
    }
}
