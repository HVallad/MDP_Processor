using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDPProcessor.Core;
using MDPProcessor.MVVM.Model;

namespace MDPProcessor.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        public INavigator Navigator { get; set; }

        private ExcelData _transitions;
        public ExcelData transitions
        {
            get { return _transitions; }
            set 
            { 
                _transitions = value; 
                NotifyPropertyChanged(nameof(transitions));
            }
        }
        private ExcelData _rewards;
        public ExcelData rewards 
        {
            get { return _rewards; }
            set 
            { 
                _rewards = value;
                NotifyPropertyChanged(nameof(rewards));
            }
        }

        private ExcelData _policy;
        public ExcelData policy
        {
            get { return _policy; }
            set
            {
                _policy = value;
                NotifyPropertyChanged(nameof(_policy));
            }
        }

        public FiniteHorizon FHData { get; set; }

        public bool IsMenuOpen { get; set; } = false;
        public int SideMenuWidth => IsMenuOpen ? 150 : 45;
        public PackIconFontAwesomeKind SideMenuIcon => IsMenuOpen ? PackIconFontAwesomeKind.ArrowCircleLeftSolid : PackIconFontAwesomeKind.BarsSolid;

        #endregion

        public Func<string> GetFileName = delegate { return string.Empty; };

        #region Commands

        public RelayCommand OnToggleMenuCommand { get; private set; }
        public RelayCommand OnLoadTransitionCommand { get; private set; }
        public RelayCommand OnLoadRewardsCommand { get; private set; }
        public RelayCommand OnGeneratePolicyCommand { get; private set; }

        #endregion

        #region Constructor
        public MainViewModel()
        {
            Navigator = new Navigator();

            IsMenuOpen = false;
            NotifyPropertyChanged(nameof(SideMenuWidth));
            NotifyPropertyChanged(nameof(SideMenuIcon));

            OnToggleMenuCommand = new(OnToggleSideMenu);
            OnLoadTransitionCommand = new(LoadTransitionFile);
            OnLoadRewardsCommand = new(LoadRewardsFile);
            OnGeneratePolicyCommand = new(GeneratePolicy);
            transitions = new ExcelData() { mdp_type = MDP_Type.Transition};
            rewards = new ExcelData() { mdp_type = MDP_Type.Reward };
            policy = new ExcelData() { mdp_type = MDP_Type.Policy };
            FHData = new FiniteHorizon() { DiscountFactor = 1, Iteration = 3};
            
            HomeViewModel _homeViewModel = (HomeViewModel)Navigator.ViewModels[ViewType.Home];
            TransitionViewModel _transitionViewModel = (TransitionViewModel)Navigator.ViewModels[ViewType.Transition];
            RewardsViewModel _rewardViewModel = (RewardsViewModel)Navigator.ViewModels[ViewType.Reward];
            PolicyViewModel _policyViewModel = (PolicyViewModel)Navigator.ViewModels[ViewType.Policy];

            _homeViewModel.OnLoadTransitionCommand = OnLoadTransitionCommand;
            _homeViewModel.TransitionExcel = transitions;
            _homeViewModel.OnLoadRewardsCommand = OnLoadRewardsCommand;
            _homeViewModel.RewardExcel = rewards;
            _homeViewModel.OnGeneratePolicyCommand = OnGeneratePolicyCommand;
            _homeViewModel.FHData = FHData;

            _transitionViewModel.OnLoadTransitionCommand = OnLoadTransitionCommand;
            _transitionViewModel.TransitionExcel = transitions;

            _rewardViewModel.OnLoadRewardsCommand = OnLoadRewardsCommand;
            _rewardViewModel.RewardsExcel = rewards;

            _policyViewModel.OnGeneratePolicyCommand = OnGeneratePolicyCommand;
            _policyViewModel.PolicyExcel = policy;
            _policyViewModel.FHData = FHData;

        }
        #endregion

        #region Methods

        public void OnToggleSideMenu()
        {
            IsMenuOpen = !IsMenuOpen;
            NotifyPropertyChanged(nameof(IsMenuOpen));
            NotifyPropertyChanged(nameof(SideMenuWidth));
            NotifyPropertyChanged(nameof(SideMenuIcon));
        }

        public void LoadTransitionFile()
        {
            string path = GetFileName();
            if (path != null && path != string.Empty)
            {
                transitions.filePath = path;

                UpdateViewModels();
            }
        }

        public void LoadRewardsFile()
        {
            string path = GetFileName();
            if (path != null && path != string.Empty)
            {
                rewards.filePath = path;

                UpdateViewModels();
            }
        }

        private void UpdateViewModels()
        {
            HomeViewModel _homeViewModel = (HomeViewModel)Navigator.ViewModels[ViewType.Home];
            TransitionViewModel _transitionViewModel = (TransitionViewModel)Navigator.ViewModels[ViewType.Transition];
            RewardsViewModel _rewardViewModel = (RewardsViewModel)Navigator.ViewModels[ViewType.Reward];
            PolicyViewModel _policyViewModel = (PolicyViewModel)Navigator.ViewModels[ViewType.Policy];

            _homeViewModel.NotifyUpdateExcelFiles();
            _transitionViewModel.NotifyUpdateExcelFiles();
            _rewardViewModel.NotifyUpdateExcelFiles();
            _policyViewModel.NotifyUpdateExcelFiles();
        }

       private void GeneratePolicy()
       {
            //get output location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save policy...";
            saveFileDialog.Filter = "Excel Files (.xlsx)|*.xlsx;*.xlsm;*.xls";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.ShowDialog();

            string outputPath = saveFileDialog.FileName;

            if (outputPath != string.Empty)
            {
                PythonEngine.GeneratePolicy(transitions.filePath, rewards.filePath, outputPath, FHData.DiscountFactor, FHData.Iteration);
                policy.filePath = outputPath;
                UpdateViewModels();
            }


       }

        #endregion




    }
}
