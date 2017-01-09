using Microsoft.Win32;
using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUIExamWPF
{
    public class Selection : INotifyPropertyChanged
    {

        private const string FOLDER_SELECTION_DEFAULT_FILENAME = "Denne Mappe";
        private string path;
        private List<string> selection;

        public Selection()
        {
            path = "";
            initCommands();
        }

        public string Path {
            get
            {
                return path;
            }

            set
            {
                if (path != value) path = value; NotifyPropertyChanged(); NotifyPropertyChanged("PathAsSource");
            }
        }
        public ImageSource PathAsSource
        {
            get
            {
                if (string.IsNullOrEmpty(Path)) return null;
                var source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(Path, UriKind.Absolute);
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
                return source;
            }
        }

        private void initCommands()
        {
            BrowseCommand = new RelayCommand(
                () => showBrowseDialog());

            RenameCommand = new RelayCommand<string>(
                renameFiles);
        }

        public ICommand BrowseCommand { get; private set; }

        private void showBrowseDialog()
        {
            OpenFileDialog dialog = setupDialog();
            if (dialog.ShowDialog() == true)
            {
                Console.WriteLine("Filename: " + System.IO.Path.GetFileName(dialog.FileName));
                selection = getSelectedFileNames(dialog);
                Path = selection[0];
            }
        }

        private List<string> getSelectedFileNames(OpenFileDialog dialog)
        {
            List<string> result;
            if(System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) == FOLDER_SELECTION_DEFAULT_FILENAME)
            {
                string dir_name = System.IO.Path.GetDirectoryName(dialog.FileName);
                Console.WriteLine(dir_name);
                result = new List<string>(Directory.GetFiles(dir_name).ToArray());
            }else
            {
                result = new List<string>(dialog.FileNames);
            }
            return result;
        }

        private OpenFileDialog setupDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF";
            dialog.AddExtension = false;
            dialog.Multiselect = true;
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            //default filename to detect folder selection
            dialog.FileName = FOLDER_SELECTION_DEFAULT_FILENAME;
            return dialog;
        }

        public ICommand RenameCommand { get; private set; }

        private void renameFiles(string name)
        {
            name = System.IO.Path.GetFileNameWithoutExtension(name);
            for (int i = 0; i < selection.Count; i++)
            {
                string file_ext = System.IO.Path.GetExtension(selection[i]);
                string dir_path = System.IO.Path.GetDirectoryName(selection[i]);
                try
                {
                    File.Move(selection[i], dir_path + "\\" + name + " - " + (i + 1) + file_ext);
                }
                catch(IOException)
                {
                    File.Move(selection[i], dir_path + "\\" + name + " - " + (i + 1) + " (Copy)" + file_ext);
                }
                
            }
        }

        






        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
