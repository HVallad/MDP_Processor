using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MDPProcessor.Core;
using MDPProcessor.MVVM.ViewModel;

namespace MDPProcessor.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = _navigator.ViewModels[ViewType.Home];
                        break;
                    case ViewType.Transition:
                        _navigator.CurrentViewModel = _navigator.ViewModels[ViewType.Transition];
                        break;
                    case ViewType.Reward:
                        _navigator.CurrentViewModel = _navigator.ViewModels[ViewType.Reward];
                        break;
                    case ViewType.Policy:
                        _navigator.CurrentViewModel = _navigator.ViewModels[ViewType.Policy];
                        break;
                }
            }
        }
    }
}
