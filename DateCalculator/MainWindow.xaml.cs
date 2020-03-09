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
using WWD_lib;

namespace DateCalculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            date1TextBox.Text = DateTime.Now.Date.AddDays(-1).ToString("d");
            date2TextBox.Text = DateTime.Now.Date.ToString("d");
            toDayTextBox.Text = DateTime.Now.Date.ToString("d");
        }

        private void Calendar1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            date1TextBox.Text = calendar1.SelectedDate.ToString();
        }

        private void Calendar2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            date2TextBox.Text = calendar2.SelectedDate.ToString();
        }

        private void DayCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            int countDays = DateWorker.CaclculateNumberDays(DateTime.Parse(date1TextBox.Text), DateTime.Parse(date2TextBox.Text));
            MessageBox.Show("Число дней равняется " + countDays.ToString() + "!");
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            date1TextBox.Text = DateTime.Now.Date.AddDays(-1).ToString("d");
            date2TextBox.Text = DateTime.Now.Date.ToString("d");
        }

        private void AgeCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int age = DateWorker.CalculateAge(DateTime.Parse(birthTextBox.Text),DateTime.Parse(toDayTextBox.Text));
                if(age%10 == 1)
                    MessageBox.Show("Возраст: " + age.ToString() + " год");
                else
                    MessageBox.Show("Возраст: " + age.ToString() + " лет");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Вы ввели не дату!");
                MessageBox.Show("Введите дату!");
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Вы не ввели дату!");
                MessageBox.Show("Введите дату!");
            }
        }

        private void ResetAgeButton_Click(object sender, RoutedEventArgs e)
        {
            birthTextBox.Text = "Введите дату рождения";
            toDayTextBox.Text = DateTime.Now.Date.ToString("d");
        }
    }
}
