using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MDPProcessor.Core;
using MDPProcessor.MVVM.Model;

namespace MDPProcessor.MVVM.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public RelayCommand OnLoadTransitionCommand { get; set; }
        public RelayCommand OnGeneratePolicyCommand { get; set; }
        public ExcelData TransitionExcel { get; set; }
        public FiniteHorizon FHData { get; set; }

        public string TransitionFileName {
            get { return TransitionExcel?.filePath != string.Empty && TransitionExcel?.filePath != null ? new FileInfo(TransitionExcel.filePath).Name : "Select a file..."; }
        }
        public RelayCommand OnLoadRewardsCommand { get; set; }
        public ExcelData RewardExcel { get; set; }
        public string RewardsFileName
        {
            get { return RewardExcel?.filePath != string.Empty && RewardExcel?.filePath != null ?  new FileInfo(RewardExcel.filePath).Name : "Select a file..."; }
        }

        public void NotifyUpdateExcelFiles()
        {
            NotifyPropertyChanged(nameof(TransitionFileName));
            NotifyPropertyChanged(nameof(RewardsFileName));
        }

    }
}
