using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MDPProcessor.MVVM.ViewModel;

namespace MDPProcessor.MVVM.View
{
    /// <summary>
    /// Interaction logic for TransitionsView.xaml
    /// </summary>
    public partial class RewardsView : UserControl
    {
        public RewardsView()
        {
            InitializeComponent();
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            RewardsViewModel _rewardsViewModel = (RewardsViewModel)this.DataContext;
            if (_rewardsViewModel.RewardsExcel.data != null)
            {
                _rewardsViewModel.UpdateExcelFile();
            }
        }
    }
}
