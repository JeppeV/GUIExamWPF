using Microsoft.Win32;
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


namespace GUIExamWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FOLDER_SELECTION_DEFAULT_FILENAME = "Denne Mappe";

        private Selection CurrentSelection;

        public MainWindow()
        {

            CurrentSelection = new Selection();
            InitializeComponent();
            
            //path_selection_button.Click += path_selection_button_Click;
            DataContext = CurrentSelection;



        }

        private void path_selection_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = setupDialog();
            if(dialog.ShowDialog() == true)
            {
                handleDialogResult(dialog);
            }
            
        }

        private void handleDialogResult(OpenFileDialog dialog)
        {
            //path_selection_input.Text = dialog.FileName;
            CurrentSelection.Path = dialog.FileName;
            Console.WriteLine("FileName: " + dialog.FileName);
            
        }

        private OpenFileDialog setupDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            
            dialog.Filter = "Image Files(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF";
            dialog.AddExtension = false;
            
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            //default filename to detect folder selection
            dialog.FileName = FOLDER_SELECTION_DEFAULT_FILENAME;
            return dialog;
        }
    }
}
