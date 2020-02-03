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
using System.Windows.Forms;

namespace Catalog
{
    /// <summary>
    /// Логика взаимодействия для BookAdding.xaml
    /// </summary>
    public partial class BookAdding : Window
    {
        public BookAdding()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        /*
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        */
    }
}
