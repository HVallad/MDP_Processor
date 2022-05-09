using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MDPProcessor.Commands;
using MDPProcessor.MVVM.ViewModel;

namespace MDPProcessor.Core
{
    public class Navigator : ObservableObject, INavigator
    {
        private ViewModelBase _currentViewModel;
        public Dictionary<ViewType, ViewModelBase> ViewModels { get; set; }

        public Navigator()
        {
            ViewModels = new Dictionary<ViewType, ViewModelBase>();
            ViewModels.Add(ViewType.Home, new HomeViewModel());
            ViewModels.Add(ViewType.Transition, new TransitionViewModel());
            ViewModels.Add(ViewType.Reward, new RewardsViewModel());
            ViewModels.Add(ViewType.Policy, new PolicyViewModel());

            _currentViewModel = ViewModels[ViewType.Home];
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                 return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

    }
}
