using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MDPProcessor.MVVM.ViewModel;

namespace MDPProcessor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel dataContext = (MainViewModel)DataContext;
            // pass file dialog string return to delegate
            dataContext.GetFileName = FileDialog_SelectFilePath;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                ToggleMaximize();
            else if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void H_thumb_DragDelta_Left(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //Move the Thumb to the mouse position during the drag operation
            //double xadjust = Width + e.HorizontalChange;
            //if (xadjust >= MinWidth)
            //{
            //    Width = xadjust;
            //}
            //else
            //{
            //    Width = MinWidth;
            //}
        }

        private void H_thumb_DragDelta_Right(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //Move the Thumb to the mouse position during the drag operation
            double xadjust = Width + e.HorizontalChange;
            if (xadjust >= MinWidth)
            {
                Width = xadjust;
            }
            else
            {
                Width = MinWidth;
            }
        }

        private void V_thumb_DragDelta_Top(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //Move the Thumb to the mouse position during the drag operation
            //double yadjust = Height + e.VerticalChange;
            //if (yadjust >= MinHeight)
            //{
            //    Height = yadjust;
            //}
            //else
            //{
            //    Height = MinHeight;
            //}
        }

        private void V_thumb_DragDelta_Bottom(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //Move the Thumb to the mouse position during the drag operation
            double yadjust = Height + e.VerticalChange;
            if (yadjust >= MinHeight)
            {
                Height = yadjust;
            }
            else
            {
                Height = MinHeight;
            }
        }

        private void ToggleMaximize()
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                app_btn_maximize_icon.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.WindowRestoreRegular;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                app_btn_maximize_icon.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.WindowMaximizeRegular;
            }
        }

        private void Button_MinimizeApp(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Button_MaximizeApp(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        private void Button_CloseApp(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private string FileDialog_SelectFilePath()
        {
            string path = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "Excel Files (.xlsx)|*.xlsx;*.xlsm;*.xls";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
            }

            return path;
        }
    }
}
