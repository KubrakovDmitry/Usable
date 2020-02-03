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

namespace Catalog.Research
{
    /// <summary>
    /// Логика взаимодействия для ResearchByAuthor.xaml
    /// </summary>
    public partial class ResearchByAuthor : Window
    {
        public ResearchByAuthor()
        {
            InitializeComponent();
        }

        void ResearchClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
