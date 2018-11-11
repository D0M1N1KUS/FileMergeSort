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
using System.Windows.Shapes;

namespace SequentialFileSorting.SortingManagment
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow(string ExceptionName, string ExceptionMessage, string Details = null)
        {
            InitializeComponent();
            StringBuilder ExceptionText = new StringBuilder();
            ExceptionText.Append(ExceptionName);
            if (ExceptionMessage != string.Empty) {
                ExceptionText.Append(":\r\n\t");
                ExceptionText.Append(ExceptionMessage);
            }
            TextBlock_Exception.Text = ExceptionText.ToString();
            TextBlock_Details.Text = Details;
        }
    }
}
