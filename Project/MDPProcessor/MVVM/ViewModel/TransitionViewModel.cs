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

    public class TransitionViewModel : ViewModelBase
    {
        public RelayCommand OnLoadTransitionCommand { get; set; }
        public RelayCommand<string> OnSwitchSheetCommand { get; set; }
        public ExcelData TransitionExcel { get; set; }
        public string TransitionFileName
        {
            get { return TransitionExcel?.filePath != string.Empty && TransitionExcel?.filePath != null ? new FileInfo(TransitionExcel.filePath).Name : "Select a file..."; }
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
                NotifyPropertyChanged(nameof(GridContext));
            }
        }


        public DataView GridContext 
        { 
            get 
            {
                if (TransitionExcel?.filePath != string.Empty && TransitionExcel?.filePath != null && TransitionExcel.data.Tables[SelectedTable] != null)
                {
                    return TransitionExcel.data.Tables[SelectedTable].DefaultView;
                }
                else
                {
                    return new DataView();
                }
                
            } 
        }

        public TransitionViewModel()
        {
            OnSwitchSheetCommand = new RelayCommand<string>(SwitchSheet);
        }

        public void NotifyUpdateExcelFiles()
        {
            if (TransitionExcel.data == null)
                return;

            List<TableInfo> tables = new List<TableInfo>();
            foreach (DataTable table in TransitionExcel.data.Tables)
            {
                TableInfo tableInfo = new TableInfo() { TableName = table.TableName};
                tables.Add(tableInfo);
            }

            TableCollection = tables.ToArray();
            if (TableCollection.Length > 0)
            {
                SelectedTable = TableCollection[0].TableName;
                NotifyPropertyChanged(nameof(TableCollection));
            }
            NotifyPropertyChanged(nameof(GridContext));
            NotifyPropertyChanged(nameof(TransitionFileName));
            if (TransitionExcel.data != null)
            {
                NotifyPropertyChanged(nameof(TransitionExcel.data.Tables));
            }
        }

        public void UpdateExcelFile()
        { 
            TransitionExcel.Save();
        }

        public void SwitchSheet(string sheet)
        {
            SelectedTable = sheet;
        }
    }
}
