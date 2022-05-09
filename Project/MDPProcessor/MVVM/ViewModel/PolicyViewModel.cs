using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDPProcessor.Core;
using MDPProcessor.MVVM.Model;

namespace MDPProcessor.MVVM.ViewModel
{
    public class PolicyViewModel : ViewModelBase
    {
        public RelayCommand OnGeneratePolicyCommand { get; set; }
        
        public ExcelData PolicyExcel { get; set; }

        public FiniteHorizon FHData { get; set; }

        public string PolicyFileName
        {
            get { return PolicyExcel?.filePath != string.Empty && PolicyExcel?.filePath != null ? new FileInfo(PolicyExcel.filePath).Name : "Select a file..."; }
        }

        private string _selectedTable = string.Empty;

        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                NotifyPropertyChanged(nameof(SelectedTable));
            }
        }

        public DataView GridContext
        {
            get
            {
                if (PolicyExcel?.filePath != string.Empty && PolicyExcel?.filePath != null)
                {
                    return PolicyExcel.data.Tables[SelectedTable].DefaultView;
                }
                else
                {
                    return new DataView();
                }

            }
        }

        public void NotifyUpdateExcelFiles()
        {
            SelectedTable = PolicyExcel.data?.Tables[0].TableName;
            NotifyPropertyChanged(nameof(GridContext));
            NotifyPropertyChanged(nameof(PolicyFileName));
        }
    }
}
