using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using LaP;
using ENBmodelLibrary;
using System.Data.Entity;

namespace Electrical_note_book
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LaPContext db1;
        ContactContext db2;
        public MainWindow()
        {
            InitializeComponent();

            db1 = new LaPContext();
            db1.Accounts.Load();
            AccountGrid.ItemsSource = db1.Accounts.Local.ToBindingList();

            db2 = new ContactContext();
            db2.Contacts.Load();
            contactGrid.ItemsSource = db2.Contacts.Local.ToBindingList();

            db2.Telephones.Load();
            phonesGrid.ItemsSource = db2.Telephones.Local.ToBindingList();

            db2.Emails.Load();
            emailGrid.ItemsSource = db2.Emails.Local.ToBindingList();

            db2.BirthDays.Load();
            birthdayCrid.ItemsSource = db2.BirthDays.Local.ToBindingList();
             
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db1.Dispose();
        }
        //Контакты
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files(*.txt)|*txt|All files (*.*)|*.*";
            //sfd.FileName = "file"
            sfd.InitialDirectory = @"Библиотеки\Документы";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                    {
                        doc.Save(fs, DataFormats.Text);
                    }
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files(*.txt)|*txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                        doc.Load(fs, DataFormats.Text);
                }
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ContactAdding window = new ContactAdding();
            window.Owner = this;

            if (window.ShowDialog() == true)
            {   
                if(window.NameTextBox.Text != "" && window.NameTextBox.Text != "-" && window.NameTextBox.Text != "нет" && window.NameTextBox.Text != " ")
                {
                    Contact contact = new Contact { Name = window.NameTextBox.Text, MainTelephone = window.PhoneTextBox.Text };
                    db2.Contacts.Add(contact);
                    db2.SaveChanges();

                    Telephone telephone = new Telephone { Name = window.NameTextBox.Text, telephone = window.PhoneTextBox.Text, Contact = contact };
                    if (telephone.telephone == "")
                        telephone.telephone = "-";
                    db2.Telephones.Add(telephone);

                    Email email = new Email { Name = window.NameTextBox.Text, email = window.EmailTextBox.Text, Contact = contact };
                    if (email.email == "")
                        email.email = "-";
                    db2.Emails.Add(email);

                    BirthDay birthDay = new BirthDay { Id = contact.Id, Name = window.NameTextBox.Text, Birthday = window.BirthDayTextBox.Text, Contact = contact };
                    if (birthDay.Birthday == "")
                        birthDay.Birthday = "-";
                    db2.BirthDays.Add(birthDay);

                    db2.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Введите имя!");
                }
            }           
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            db2.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(contactGrid.SelectedItems.Count > 0)
            {
                Contact contact = contactGrid.SelectedItem as Contact;

                Telephone telephone = db2.Telephones.FirstOrDefault(tel => tel.ContactId == contact.Id);
                if (telephone != null)
                    db2.Telephones.Remove(telephone);

                Email email = db2.Emails.FirstOrDefault(em => em.ContactId == contact.Id);
                if (email != null)
                    db2.Emails.Remove(email);

                BirthDay birthDay = db2.BirthDays.FirstOrDefault(day => day.Id == contact.Id);
                if (birthDay != null)
                    db2.BirthDays.Remove(birthDay);

                db2.Contacts.Remove(contact);

                db2.SaveChanges();
            }
        }
        //Телефоны
        private void Add_phone_click(object sender, RoutedEventArgs e)
        {
            PhoneWindow window = new PhoneWindow();
            window.Owner = this;

            if(window.ShowDialog() == true)
            {
                window.NameComboBox.ItemsSource = db2.Telephones.Local.ToBindingList();
                Telephone telephone = new Telephone();
                telephone.Name = window.NameComboBox.SelectedItem.ToString();
                telephone.telephone = window.PhoneTextBox.Text;
                Contact contact = db2.Contacts.FirstOrDefault(i => i.Name == window.NameComboBox.SelectedItem.ToString());
                telephone.Contact = contact;

                db2.Telephones.Add(telephone);
                db2.SaveChanges();
            }            
        }

        private void Save_click_phones(object sender, RoutedEventArgs e)
        {
            db2.SaveChanges();
        }

        private void Delete_telephones_click(object sender, RoutedEventArgs e)
        {
            if (AccountGrid.SelectedItems.Count > 0)
            {
                Telephone telephone = AccountGrid.SelectedItem as Telephone;
                if (telephone != null)
                {
                    db2.Telephones.Remove(telephone);
                    db2.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Данных нет");
            }
        }
        //Email
        private void Add_email_click(object sender, RoutedEventArgs e)
        {
            EmailWindow window = new EmailWindow();
            window.Owner = this;

            if(window.ShowDialog() == true)
            {
                window.NameComboBox.ItemsSource = db2.Emails.Local.ToBindingList();
                Email email = new Email();
                email.Name = window.NameComboBox.SelectedItem.ToString();
                email.email = window.EmailTextBox.Text;
                Contact contact = db2.Contacts.FirstOrDefault(i => i.Name == window.NameComboBox.SelectedItem.ToString());
                email.Contact = contact;

                db2.Emails.Add(email);
                db2.SaveChanges();
            }            
        }

        private void Save_click_email(object sender, RoutedEventArgs e)
        {
            db2.SaveChanges();
        }

        private void Delete_email_click(object sender, RoutedEventArgs e)
        {
            if (AccountGrid.SelectedItems.Count > 0)
            {
                Email email = AccountGrid.SelectedItem as Email;
                if (email != null)
                {
                    db2.Emails.Remove(email);
                    db2.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Данных нет");
            }
        }
        //Дни рождения
        private void Save_click_birthday(object sender, RoutedEventArgs e)
        {
            db2.SaveChanges();
        }
        private void Delete_birthday_click(object sender, RoutedEventArgs e)
        {
            if (AccountGrid.SelectedItems.Count > 0)
            {
                BirthDay birthDay = AccountGrid.SelectedItem as BirthDay;
                if (birthDay != null)
                {
                    db2.BirthDays.Remove(birthDay);
                    db2.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Данных нет");
            }
        }
        //Логины и пароли
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            db1.SaveChanges();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(AccountGrid.SelectedItems.Count > 0)
            {
                Account account = AccountGrid.SelectedItem as Account;
                if(account != null)
                {
                    db1.Accounts.Remove(account);
                    db1.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Данных нет");
            }
        }

    }
}
