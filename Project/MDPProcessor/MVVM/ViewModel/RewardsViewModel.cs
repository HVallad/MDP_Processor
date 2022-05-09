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
    public class RewardsViewModel : ViewModelBase
    {
        public RelayCommand OnLoadRewardsCommand { get; set; }
        public ExcelData RewardsExcel { get; set; }
        public string RewardsFileName
        {
            get { return RewardsExcel?.filePath != string.Empty && RewardsExcel?.filePath != null ? new FileInfo(RewardsExcel.filePath).Name : "Select a file..."; }
        }

        private string _selectedTable = string.Empty;

        public TableInfo[] TableCollection { get; set; }

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
                if (RewardsExcel?.filePath != string.Empty && RewardsExcel?.filePath != null)
                {
                    return RewardsExcel.data.Tables[SelectedTable].DefaultView;
                }
                else
                {
                    return new DataView();
                }

            }
        }

        public void NotifyUpdateExcelFiles()
        {
            if (RewardsExcel.data == null)
                return;

            List<TableInfo> tables = new List<TableInfo>();
            foreach (DataTable table in RewardsExcel.data.Tables)
            {
                TableInfo tableInfo = new TableInfo() { TableName = table.TableName };
                tables.Add(tableInfo);
            }
            TableCollection = tables.ToArray();
            if (TableCollection.Length > 0)
            {
                SelectedTable = TableCollection[0].TableName;
                NotifyPropertyChanged(nameof(TableCollection));
            }
            NotifyPropertyChanged(nameof(GridContext));
            NotifyPropertyChanged(nameof(RewardsFileName));
            if (RewardsExcel.data != null)
            {
                NotifyPropertyChanged(nameof(RewardsExcel.data.Tables));
            }
        }

        public void UpdateExcelFile()
        {
            RewardsExcel.Save();
        }
    }
}
