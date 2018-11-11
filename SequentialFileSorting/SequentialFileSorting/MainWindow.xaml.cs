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
using System.Windows.Forms;
using System.Windows.Shapes;
using System.IO;
using System.Text;
using System.Windows.Controls;
using FileIO.RecordIO;
using SequentialFileSorting.Sorting;
using SequentialFileSorting.SortingManagment;
using Sortowanie_rekordów;

namespace SequentialFileSorting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const long MAX_AMOUT_OF_LINES = 1000;
        private string filePath;

        public bool allowDisplayingFiles;
        public bool largeFileWarningPrompted;

        private PolyPhaseSorting Sorter;
        private ValueComponentsSplitter ValueComponentsSplitter;
        
        private long linesIfChoosenFile; 

        private int busyTextAnimation;

        public MainWindow()
        {
            InitializeComponent();
            
            ValueComponentsSplitter = new ValueComponentsSplitter();
            
            allowDisplayingFiles = true;
            largeFileWarningPrompted = false;
            busyTextAnimation = 0;

            Label_StatusBarLabel.Content = "No file selected.";
        }

        private void enableButtons(bool SortButton, bool StepButton, bool ChooseFileButton)
        {
            Button_Step.IsEnabled = StepButton;
            Button_Sort.IsEnabled = SortButton;
            Button_Open.IsEnabled = ChooseFileButton;
        }

        private void enableCheckboxes(bool ShowSortedFile, bool ShowUnsortedFile, bool ShowRecordValues)
        {
            CheckBox_ShowSortedFile.IsEnabled = ShowSortedFile;
            CheckBox_ShowUnsortedFile.IsEnabled = ShowUnsortedFile;
            CheckBox_ShowRecordValues.IsEnabled = ShowRecordValues;
        }

        private void Button_ChooseFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new System.Windows.Forms.OpenFileDialog())
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        filePath = dialog.FileName;
                        checkPathAndEnableButtons();
                        createNewSorter();
                    }
                }
            }
            catch(Exception ex)
            {
                createNewExceptionWindow(ex, "Details not avaliable for this exception");
                enableButtons(false, false, true);
                enableCheckboxes(false, false, false);
            }
        }

        private void createNewSorter()
        {
            TextBox_FileToSort.Text = filePath;
            linesIfChoosenFile = Utilities.getNumberOfLinesInFile(filePath);
            var fileParameters = new FileParameters()
            {
                BlockSize = int.Parse(TextBox_BlockSize.Text), Separator = " ",
                SourceFileName = filePath,
                TemporaryBufferFileDirectory = TextBox_BufferLocation.Text
            };
            var sortingParameters = new SortingParameters()
            {
                NumberOfTemporaryFiles = int.Parse(TextBox_NumberOfTemporaryBuffers.Text)
            };
            Sorter = new PolyPhaseSorting(sortingParameters, fileParameters);
            Sorter.Distribution.Distribute();
        }

        private void checkPathAndEnableButtons()
        {
            if (Utilities.pathIsDirectory(filePath))
            {
                Label_StatusBarLabel.Content = "Selected file: \"" + filePath + "\" is a directory.";
            }
            else
            {
                Label_StatusBarLabel.Content = "Selected file: \"" + filePath + "\"";
                enableButtons(true, true, true);
                enableCheckboxes(true, true, true);
            }
        }

        private void Button_SortClick(object sender, RoutedEventArgs e)
        {
            enableButtons(false, false, false);
            enableCheckboxes(false, false, false);
            lockRecordCreationTools();
            Label_StatusBarLabel.Content = "Busy...";
            try
            {
                Sorter.Merger.Merge();
                Sorter.RestoreOriginalFileName();
            }
            catch(Exception ex)
            {
                var ExWin = new ExceptionWindow(ex.GetType().ToString(), ex.Message);
                ExWin.ShowDialog();
            }
            Label_StatusBarLabel.Content = buildAccessInfoString("Done! ");
            displayFileInTextblock(TextBlock_SortedFile);
            enableButtons(true, true, true);
            lockRecordCreationTools(false);
        }

        private void BusyInfo()
        {
            switch (busyTextAnimation)
            {
                case 0:
                    busyTextAnimation++;
                    Label_StatusBarLabel.Content = "Sorting.";
                    break;
                case 1:
                    busyTextAnimation++;
                    Label_StatusBarLabel.Content = "Sorting .";
                    break;
                case 2:
                    busyTextAnimation = 0;
                    Label_StatusBarLabel.Content = "Sorting  .";
                    break;
                default:
                    busyTextAnimation = 0;
                    break;
            }
        }

        private string buildAccessInfoString(string beginning = @"")
        {
            StringBuilder infoString = new StringBuilder();
            infoString.Append(beginning);
            infoString.Append("Disk read-accesses: ");
            infoString.Append(Sorter.ReadAccesses);
            infoString.Append(" | Disk write-accesses: ");
            infoString.Append(Sorter.WriteAccesses);
            infoString.Append(" | Phases: ");
            infoString.Append(Sorter.Steps);
            infoString.Append(".");
            if (Sorter.Merger.FileIsSorted)
            {
                infoString.Append(" You can now choose a new file to sort");
            }

            return infoString.ToString();
        }

        private void Button_StepClick(object sender, RoutedEventArgs e)
        {
            enableCheckboxes(false, false, false);
            enableButtons(false, false, false);
            lockRecordCreationTools();
            Label_StatusBarLabel.Content = "Busy...";
            try
            {
                Sorter.Merger.Step();
            }
            catch(Exception ex)
            {
                var ExWin = new ExceptionWindow(ex.GetType().ToString(), ex.Message, ex.StackTrace);
                ExWin.ShowDialog();
            }
            
            string infoText = "Step. ";
            if (Sorter.Merger.FileIsSorted)
            {
                infoText = "Done! ";
                lockRecordCreationTools(false);
                Sorter.RestoreOriginalFileName();
            }
            Label_StatusBarLabel.Content = buildAccessInfoString(infoText);
            tryToDisplayTextIn(TextBlock_SortedFile);
            enableButtons(true, true, true);
        }

        private void CheckBox_ShowRecordValuesClick(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBox_ShowSortedFile.IsChecked)
            {
                CheckBox_ShowSortedFileClick(CheckBox_ShowSortedFile, e);
            }
            if ((bool)CheckBox_ShowUnsortedFile.IsChecked)
            {
                CheckBox_ShowUnsortedFileClick(CheckBox_ShowUnsortedFile, e);
            }
        }

        private void CheckBox_ShowUnsortedFileClick(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBox_ShowUnsortedFile.IsChecked && allowDisplayingFiles)
            {
                Label_StatusBarLabel.Content = "The file has " +
                    getNumberOfRecordsInFile(sender, e).ToString() + " records and " + PreSorting.GetNumberOfSeries(filePath) + " series.";
                tryToDisplayTextIn(TextBlock_UnsortedFile);
            }
            else
            {
                TextBlock_UnsortedFile.Text = string.Empty;
            }

        }

        private void CheckBox_ShowSortedFileClick(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBox_ShowSortedFile.IsChecked && allowDisplayingFiles)
            {
                Label_StatusBarLabel.Content = "The file has " +
                    getNumberOfRecordsInFile(sender, e).ToString() + " records.";
                tryToDisplayTextIn(TextBlock_SortedFile);
            }
            else
            {
                TextBlock_SortedFile.Text = string.Empty;
            }

        }

        private void tryToDisplayTextIn(TextBlock tb)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    displayFileInTextblock(tb);
                }
                catch (Exception ex)
                {
                    StringBuilder messages = new StringBuilder();
                    messages.Append("Error messages:\r\n");
                    messages.Append("[");
                    messages.Append(ex.GetType().ToString());
                    messages.Append("]\r\n\t");
                    messages.Append(ex.Message);
                    messages.Append("\n");
                    if (i == 2)
                    {
                        createNewExceptionWindow(ex, messages.ToString());
                    }
                }
            }
        }

        private void displayFileInTextblock(TextBlock tb)
        {
            var path = Sorter.Merger.FileIsSorted ? filePath : Sorter.GetCurrentDestinationFilePath();
            string[] lines = File.ReadAllLines(path);
            if ((bool)CheckBox_ShowRecordValues.IsChecked)
            {
                addRecordValuesToEachRecord(ref lines);
            }
            StringBuilder fileStr = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                fileStr.Append("> ");
                fileStr.Append(lines[i]);
                fileStr.Append(Environment.NewLine + Environment.NewLine);
            }
            tb.Text = fileStr.ToString();
        }

        private void addRecordValuesToEachRecord(ref string[] lines)
        {
            for(int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] != '\0')
                    lines[i] = "[" + new Record(ValueComponentsSplitter.GetValues(lines[i])).Value + "]\t" + lines[i];
            }
        }

        private long getNumberOfRecordsInFile(object sender, RoutedEventArgs e)
        {
            long lines = 0;
            try
            {
                var checkBox = sender as System.Windows.Controls.CheckBox;
                if ((bool)checkBox.IsChecked)
                {
                    lines = Utilities.getNumberOfLinesInFile(filePath);
                }
            }
            catch (Exception fileException)
            {
                createNewExceptionWindow(fileException, 
                    @"The file might be used by the sorting algorithm or a another program.");
            }

            return lines;
        }

        private void createNewExceptionWindow(Exception ex, string details = null)
        {
            string exceptionDetails = details;
            if(string.IsNullOrEmpty(details))
            {
                exceptionDetails = ex.StackTrace;
            }
            else
            {
                exceptionDetails = details + "\n" + ex.StackTrace;
            }
            ExceptionWindow ExWin = new ExceptionWindow(ex.GetType().ToString(), ex.Message, exceptionDetails);
            ExWin.ShowDialog();
            allowDisplayingFiles = false;
        }

        private void ButtonGenerateRecordFileClick(object sender, RoutedEventArgs e)
        {
            var RecGenWin = new RecordGeneratorWindow();
            RecGenWin.ShowDialog();
            if (RecGenWin.operationSucceeded())
            {
                filePath = RecGenWin.getFilePath();
                createNewSorter();
                checkPathAndEnableButtons();
                lockRecordCreationTools();
            }
            else
            {
                Label_StatusBarLabel.Content = "Operation cancelled.";
            }
        }

        private void ButtonCreateRecordFileClick(object sender, RoutedEventArgs e)
        {
            var CreateRecWin = new RecordGeneratorWindow();
            CreateRecWin.ShowDialog();
            if (CreateRecWin.operationSucceeded())
            {
                filePath = CreateRecWin.getFilePath();
                createNewSorter();
                checkPathAndEnableButtons();
                lockRecordCreationTools();
            }
            else
            {
                Label_StatusBarLabel.Content = "Operation cancelled.";
            }

        }

        private void lockRecordCreationTools(bool doLock = true)
        {
            Button_GenerateRecordFile.IsEnabled = !doLock;
            Button_CreateRecordFile.IsEnabled = !doLock;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TextBox_NumberOfTemporaryBuffers_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TextBox_BlockSize_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}