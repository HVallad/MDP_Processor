using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDPProcessor.Core;
using System.Windows;

namespace MDPProcessor.MVVM.Model
{
    public enum MDP_Type
    {
        Transition,
        Reward,
        Policy
    }
    public class ExcelData : ObservableObject
    {
        private string _filePath;
        public string filePath
        {
            get { return _filePath; }
            set 
            { 
                if (File.Exists(value))
                {
                    data = LoadExcelFile(value);
                    if (data != null && data.Tables.Count > 0)
                    {
                        _filePath = value;
                    }
                    else
                    {
                        _filePath = string.Empty; 
                    }
                }
            }
        }

        public MDP_Type mdp_type { get; set; }

        public DataSet data { get; set; }

        private DataSet LoadExcelFile(string filePath)
        {
            DataSet dataSet = new DataSet();

            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read every sheet
                foreach (IXLWorksheet workSheet in workBook.Worksheets)
                {
                    //Create a new DataTable.
                    DataTable dt = new DataTable();
                    dt.TableName = workSheet.Name;

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        if (row.Cells().Count() == 0)
                        {
                            break;
                        }
                        //Use the first row to add columns to DataTable.
                        if (dt.Columns.Count == 0)
                        {
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                continue;
                            }
                            else
                            {
                                for (int j = 0; j < row.Cells().Count(); j++)
                                {
                                    dt.Columns.Add(new DataColumn(j.ToString()));
                                }
                            }
                            
                        }


                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }

                    dataSet.Tables.Add(dt);

                }
            }
            if (mdp_type == MDP_Type.Transition)
            {
                if (CheckIfStoichastic(dataSet))
                {
                    return dataSet;
                }
                else
                {
                    filePath = string.Empty;
                    return new DataSet();
                }
            }
            else
            {
                return dataSet;
            }


            
        }

        private bool CheckIfStoichastic(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return false;
            }
            bool isStoic = true;
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    double sumVal = 0;
                    foreach (string cellVal in row.ItemArray)
                    {
                        sumVal += double.Parse(cellVal);
                    }
                    if (sumVal != 1.00)
                    {
                        isStoic = false;
                        break;
                    }
                }
                if (!isStoic)
                {
                    break;
                }
            }
            if (!isStoic)
            {
                MessageBox.Show("Imported excel sheet is not stoichastic. Please ensure rows add up to 1!");
            }

            return isStoic;

        }

        public void Save()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt in data.Tables)
                {
                    wb.Worksheets.Add(dt);
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    File.WriteAllBytes(this.filePath, stream.ToArray());
                }
            }


        }

        private MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }
}
