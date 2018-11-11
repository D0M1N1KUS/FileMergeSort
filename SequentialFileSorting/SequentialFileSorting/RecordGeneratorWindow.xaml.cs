using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using RecordFileGenerator;
using RecordFileGenerator;

namespace SequentialFileSorting
{
    /// <summary>
    /// Interaction logic for RecordGeneratorWindow.xaml
    /// </summary>
    public partial class RecordGeneratorWindow : Window
    {
        const string defaultFileName = "Records.txt";

        private RandomPlaintextRecordGenerator RecordGenerator;
        private string filePath;

        private bool success;

        public RecordGeneratorWindow()
        {
            InitializeComponent();
            RecordGenerator = new RandomPlaintextRecordGenerator();
            success = false;
        }

        private void ButtonSaveIn(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                TextBox_FilePath.Text = filePath;
                Button_Generate.IsEnabled = true;
            }

        }

        private void ButtonGenerate(object sender, RoutedEventArgs e)
        {
            if (!checkFileSizeValue())
            {
                return;
            }

            this.IsEnabled = false;

            if (comboBox.SelectedIndex == 0)
            {
                new RecordFileGenerator.RecordFileGenerator(filePath, RecordGenerator).Generate(
                    long.Parse(TextBoxFileSize.Text), GeneratorSizeType.NumberOfElements);
            }
            else if(comboBox.SelectedIndex == 2)
            {
                new RecordFileGenerator.RecordFileGenerator(filePath, RecordGenerator).Generate(
                    long.Parse(TextBoxFileSize.Text) * 1024 * 1024, GeneratorSizeType.FileSize);
            }
            else
            {
                new RecordFileGenerator.RecordFileGenerator(filePath, RecordGenerator).Generate(
                    long.Parse(TextBoxFileSize.Text) * 1024, GeneratorSizeType.NumberOfElements);
            }
            success = true;
            this.Close();
        }

        private bool checkFileSizeValue()
        {
            long o;
            if (!long.TryParse(TextBoxFileSize.Text.ToString(), out o))
            {
                TextBoxFileSize.BorderBrush = System.Windows.Media.Brushes.Red;
                return false;
            }

            return true;
        }

        private long getSizeMultiplier()
        {
            if(comboBox.SelectedIndex == 0)
            {
                return 1024;
            }
            if(comboBox.SelectedIndex == 1)
            {
                return 1048576;
            }
            throw new Exception("Wrong ComboBox indexation or index not supported!");
        }

        public string getFilePath()
        {
            return filePath;
        }

        public bool operationSucceeded()
        {
            return success;
        }
    }
}
