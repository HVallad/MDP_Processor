using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MDPProcessor.MVVM.ViewModel;

namespace MDPProcessor.Core
{
    public enum ViewType
    {
        Home,
        Transition,
        Reward,
        Policy
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }

        Dictionary<ViewType, ViewModelBase> ViewModels { get; set; }
    }
}
