using Catalog.BookFolder;
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Path = System.IO.Path;

namespace Catalog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ObservableCollection<Book> books = new ObservableCollection<Book>(); // список книг
        static int Id = 0;
        static string pathDirLists = @"The diractory of files";
        static string pathCategory = @".\The diractory of files\Categories.txt";
        static string pathProgress = @".\The diractory of files\Progress.txt";
        static string pathLitType = @".\The diractory of files\LitType.txt";    //путь к файлу типов литературы
        static string pathBooks = @"Books.dat";
        static string pathDic = @"The diractory of books";
        BinaryFormatter formatter = new BinaryFormatter();
        // списки характеристик
        static ObservableCollection<string> listcategory = new ObservableCollection<string>();          // список категорий
        static ObservableCollection<string> listprogress = new ObservableCollection<string>();          // список прогресса
        static ObservableCollection<string> listLiteraterTypes = new ObservableCollection<string>();    // список типов литературы
        // списки книг для поиска
        static ObservableCollection<Book> listbooksbyauthor = new ObservableCollection<Book>();         // список книг по автору
        static ObservableCollection<Book> listbooksbyprogress = new ObservableCollection<Book>();       // список книг по прогруссу
        static ObservableCollection<Book> listbooksbycategory = new ObservableCollection<Book>();       // список книг по категории
        static ObservableCollection<Book> listbooksbytypelit = new ObservableCollection<Book>();        // список книг по типу литературы
        static ObservableCollection<Book> listbooksbyseveralparams = new ObservableCollection<Book>();  // список книг по нескольким параметрам

        public MainWindow()
        {
            // считываем данные о книга из файла в список
            LoadBookFile();
            CreatDir();                     // создание папки для файлов характеристик
            CreatFilesoftheLists();         // создание файлов характеристик 
            LoadFileToLists();              // загрука характеристик из файлов в списки
            
            InitializeComponent();
            CreatBookFile();                // создание файл для хранения информации книгах
            CreatBookDiractoty();           // создание какталога для хранения самих книг
            catalogGrid.ItemsSource = books;// загружаем данных о кнга из списка книг
        }
          
        static void CreatDir()
        {
            if (!Directory.Exists(pathDirLists))
                Directory.CreateDirectory(pathDirLists);
        }

        static void CreatFilesoftheLists()
        {
            if (!File.Exists(pathCategory))
                File.Create(pathCategory);
            if (!File.Exists(pathProgress))
                File.Create(pathProgress);
            if (!File.Exists(pathLitType))
                File.Create(pathLitType);
        }
        
        static void LoadFileToLists()
        {
            string line;

            //заполнение списка категорий
            using (StreamReader streamReader = new StreamReader(pathCategory, Encoding.Default))
            {
                while((line = streamReader.ReadLine()) != null)
                {
                    listcategory.Add(line);
                }
            }
            //заполнение списка прогресса
            using (StreamReader streamReader = new StreamReader(pathProgress, Encoding.Default))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    listprogress.Add(line);
                }
            }
            //заполнение списка типов литературы
            using (StreamReader streamReader = new StreamReader(pathLitType, Encoding.Default))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    listLiteraterTypes.Add(line);
                }
            }
        }
        
        static void CreatBookFile()
        {            
            if (!File.Exists(pathBooks))
                File.Create(pathBooks);
        }

        static void CreatBookDiractoty()
        {            
            if (!Directory.Exists(pathDic))
                Directory.CreateDirectory(pathDic);
        }

        void LoadBookFile()
        {
            try
            {
                
                    using (FileStream fileStream = new FileStream(pathBooks, FileMode.OpenOrCreate))
                    {
                        books = (ObservableCollection<Book>)formatter.Deserialize(fileStream);                     
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Файл пока пусть");
                MessageBox.Show(ex.ToString());
            }            
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddBookButton(object sender, RoutedEventArgs e)
        {
            if (books.Count > 0)
            {
                Id = books.Max(b => b.Id);
            }
            Id++;
            
            BookAdding window = new BookAdding();
            window.Owner = this;
            window.ProgressComboBox.ItemsSource = listprogress;
            window.CategoryComboBox.ItemsSource = listcategory;
            window.LiteratureTypeComboBox.ItemsSource = listLiteraterTypes;

            if (window.ShowDialog() == true)
            {                
                if (window.NameTextBox.Text != "" && window.AuthorTextBox.Text != "" && window.PathTextBox.Text != "" && window.FileNameTextBox.Text != "" && window.DescriptionTextBox.Text != "")
                {
                    Book book = new Book
                    {
                        Id = Id,
                        Name = window.NameTextBox.Text,
                        Author = window.AuthorTextBox.Text,
                        Progress = window.ProgressComboBox.SelectedItem.ToString(),
                        Category = window.CategoryComboBox.SelectedItem.ToString(),
                        LiteratureType = window.LiteratureTypeComboBox.SelectedItem.ToString(),
                        Path = Path.Combine(pathDic, window.FileNameTextBox.Text),
                        Description = window.DescriptionTextBox.Text
                    };

                    books.Add(book);
                    File.Copy(Path.Combine(window.PathTextBox.Text, window.FileNameTextBox.Text), book.Path);

                    try
                    {
                        using (FileStream fileStream = new FileStream(pathBooks, FileMode.Truncate))
                        {
                            formatter.Serialize(fileStream, books);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }                        
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }                   
            }            
        }

        void SaveChangeButton(object sender, RoutedEventArgs e)
        {
            if (listbooksbyauthor.Count != 0)
            {
                for(int i = 0; i < books.Count; i++)
                {
                    for(int j = 0; j < listbooksbyauthor.Count; j++)
                    {
                        if (books[i].Id == listbooksbyauthor[j].Id)
                            books[i] = listbooksbyauthor[j];
                    }
                }
            }
                
            if (listbooksbycategory.Count != 0)
                for(int i = 0; i < books.Count; i++)
                    for(int j = 0; j < listbooksbycategory.Count; j++)
                        if (books[i].Id == listbooksbycategory[j].Id)
                            books[i] = listbooksbycategory[j];

            if (listbooksbyprogress.Count != 0)
                for (int i = 0; i < books.Count; i++)
                    for (int j = 0; j < listbooksbyprogress.Count; j++)
                        if (books[i].Id == listbooksbyprogress[j].Id)
                            books[i] = listbooksbyprogress[j];

            if (listbooksbytypelit.Count != 0)
                for (int i = 0; i < books.Count; i++)
                    for (int j = 0; j < listbooksbytypelit.Count; j++)
                        if (books[i].Id == listbooksbytypelit[j].Id)
                            books[i] = listbooksbytypelit[j];

            if (listbooksbyseveralparams.Count != 0)
                for (int i = 0; i < books.Count; i++)
                    for (int j = 0; j < listbooksbyseveralparams.Count; j++)
                        if (books[i].Id == listbooksbyseveralparams[j].Id)
                            books[i] = listbooksbyseveralparams[j];

            try
            {
                using (FileStream fileStream = new FileStream(pathBooks, FileMode.Truncate))
                {
                    formatter.Serialize(fileStream, books);
                }
                MessageBox.Show("Измениния прошли успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void DeleteBookButton(object sender, RoutedEventArgs e)
        {
            if(catalogGrid.SelectedItems.Count > 0)
            {
                var book = catalogGrid.SelectedItem as Book;
                if(book != null)
                {
                    if (listbooksbyauthor.Count != 0)
                        listbooksbyauthor.Remove(book);
                    if (listbooksbycategory.Count != 0)
                        listbooksbycategory.Remove(book);
                    if (listbooksbyprogress.Count != 0)
                        listbooksbyprogress.Remove(book);
                    if (listbooksbytypelit.Count != 0)
                        listbooksbytypelit.Remove(book);
                    if (listbooksbyseveralparams.Count != 0)
                        listbooksbyseveralparams.Remove(book);
                }                

                File.Delete(book.Path);
                if (book != null)
                    books.Remove(book);
                try
                {
                    using (FileStream fileStream = new FileStream(pathBooks, FileMode.Truncate))
                    {
                        formatter.Serialize(fileStream, books);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
		// Открытие файла программой поумолчанию
        void OpenBookButton(object sender, RoutedEventArgs e)
        {
            // если датагриде есть хотя бы один элемент, 
            // выбираем выделеный элемент,
            // преобразуем его в объект типа Book,
            // по пути к файлу открываем книгу с помощью программы поумолчанию
            try
            {
                //if (catalogGrid.IsFocused)
                //{
                    if (catalogGrid.SelectedItems.Count > 0)
                    {
                        var book = catalogGrid.SelectedItem as Book;
                        if (book != null)
                            Process.Start(Path.Combine(@"", book.Path));
                        else
                            MessageBox.Show("Нужно выделить книгу");
                    }
                    else
                        MessageBox.Show("В каталоге ничего нет");
                //}                    
                //else
                //    MessageBox.Show("Нужно выделить книгу");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Добавление категории
        void CategoryAddButton(object sender, RoutedEventArgs e)
        {
            CategoryAdding window = new CategoryAdding();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                listcategory.Add(window.TextCategory.Text);
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(pathCategory, true, Encoding.Default))
                    {
                        streamWriter.WriteLine(window.TextCategory.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // удаление категории
        void CategoryDeleteButton(object sender, RoutedEventArgs e)
        {
            CategoryRemoval window = new CategoryRemoval();
            window.Owner = this;
            window.CategoryListBox.ItemsSource = listcategory;
            string line;
            if (window.ShowDialog() == true)
            {
                line = window.CategoryListBox.SelectedItem.ToString();
                listcategory.Remove(line);
                try
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(pathCategory, false, Encoding.Default))
                        {
                            int i = 0;
                            while (i < listcategory.Count)
                            {
                                streamWriter.WriteLine(listcategory[i]);
                                i++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // дабавление состаяния прогресса
        void ProgressAddButton(object sender, RoutedEventArgs e)
        {
            AddingProgress window = new AddingProgress();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                listprogress.Add(window.TextProgress.Text);
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(pathProgress, true, Encoding.Default))
                    {
                        streamWriter.WriteLine(window.TextProgress.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // удаление состояния прогресса
        void ProgressDeleteButton(object sender, RoutedEventArgs e)
        {
            ProgressRemoval window = new ProgressRemoval();
            window.Owner = this;
            window.ProgressListBox.ItemsSource = listprogress;
            string line;
            if (window.ShowDialog() == true)
            {
                line = window.ProgressListBox.SelectedItem.ToString();
                listprogress.Remove(line);
                try
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(pathProgress, false, Encoding.Default))
                        {
                            int i = 0;
                            while (i < listprogress.Count)
                            {
                                streamWriter.WriteLine(listprogress[i]);
                                i++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // добавление типа литературы
        void LTAddButton(object sender, RoutedEventArgs e)
        {
            AddTypeLit window = new AddTypeLit();
            window.Owner = this;
            if(window.ShowDialog() == true)
            {
                listLiteraterTypes.Add(window.TextTypeLit.Text);
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(pathLitType, true, Encoding.Default))
                    {
                        streamWriter.WriteLine(window.TextTypeLit.Text);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // удаление типа литературы
        void LTDeleteButton(object sender, RoutedEventArgs e)
        {
            DeleteLibType window = new DeleteLibType();
            window.Owner = this;
            window.LitTypeListBox.ItemsSource = listLiteraterTypes;
            string line;
            if(window.ShowDialog() == true)
            {
                line = window.LitTypeListBox.SelectedItem.ToString();
                listLiteraterTypes.Remove(line);
                try
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(pathLitType, false, Encoding.Default))
                        {
                            int i = 0;
                            while(i < listLiteraterTypes.Count)
                            {
                                streamWriter.WriteLine(listLiteraterTypes[i]);
                                i++;
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // поиск по типу
        void TypeRearch(object sender, RoutedEventArgs e)
        {
            listbooksbycategory.Clear();
            listbooksbyprogress.Clear();
            listbooksbyauthor.Clear();
            listbooksbyseveralparams.Clear();

            Research.ReseearchByLitType window = new Research.ReseearchByLitType();
            window.Owner = this;

            window.LitTypeComboBox.ItemsSource = listLiteraterTypes;

            if (window.ShowDialog() == true)
            {
                string type = window.LitTypeComboBox.SelectedItem.ToString();                       // название типа
                var filtlist = books.Where(b => b.LiteratureType == type);   //.OrderBy(b => b).Select(b => b); 
                listbooksbytypelit = new ObservableCollection<Book>(filtlist);
                if(listbooksbytypelit.Count != 0)
                    catalogGrid.ItemsSource = listbooksbytypelit;
                else
                    MessageBox.Show("Ничего не найдено!");
            }
        }

        // поиск по состоянию прогресса
        void ProgressRearch(object sender, RoutedEventArgs e)
        {
            listbooksbyauthor.Clear();
            listbooksbytypelit.Clear();
            listbooksbycategory.Clear();
            listbooksbyseveralparams.Clear();

            Research.ResearchByProgress window = new Research.ResearchByProgress();
            window.Owner = this;

            window.ProgressComboBox.ItemsSource = listprogress;

            if (window.ShowDialog() == true)
            {
                string progress = window.ProgressComboBox.SelectedItem.ToString();                 // название состояния прогресса
                var filtlist = books.Where(b => b.Progress.Contains(progress));    //.OrderBy(b => b).Select(b => b);
                listbooksbyprogress = new ObservableCollection<Book>(filtlist);
                if (listbooksbyprogress.Count != 0)
                    catalogGrid.ItemsSource = listbooksbyprogress;
                else
                    MessageBox.Show("Ничего не найдено!");
            }
        }

        // поиск каталог
        void CategoryRearch(object sender, RoutedEventArgs e)
        {
            listbooksbyauthor.Clear();
            listbooksbyprogress.Clear();
            listbooksbytypelit.Clear();
            listbooksbyseveralparams.Clear();

            Research.ResearchByCategory window = new Research.ResearchByCategory();
            window.Owner = this;

            window.CategotyComboBox.ItemsSource = listcategory;

            if (window.ShowDialog() == true)
            {
                string category = window.CategotyComboBox.SelectedItem.ToString();                  // название категории
                var filtlist = books.Where(b => b.Category == category);    //.OrderBy(b => b).Select(b => b);
                listbooksbycategory = new ObservableCollection<Book>(filtlist);
                if(listbooksbycategory.Count != 0)
                    catalogGrid.ItemsSource = listbooksbycategory;
                else
                    MessageBox.Show("Ничего не найдено!");
            }
        }

        // поиск автора
        void AuthorRearch(object sender, RoutedEventArgs e)
        {
            listbooksbycategory.Clear();
            listbooksbyprogress.Clear();
            listbooksbytypelit.Clear();
            listbooksbyseveralparams.Clear();

            Research.ResearchByAuthor window = new Research.ResearchByAuthor();
            window.Owner = this;
            if(window.ShowDialog() == true)
            {
                string name = window.AuthorTextBox.Text;                // имя автора
                var filtlist = books.Where(b => b.Author == name); //.OrderBy(b => b).Select(b => b);
                listbooksbyauthor = new ObservableCollection<Book>(filtlist);
                if (listbooksbyauthor.Count != 0)
                    catalogGrid.ItemsSource = listbooksbyauthor;
                else
                    MessageBox.Show("Ничего не найдено!");
            }
        }

        // вывод всей литературы
        void ShowAll(object sender, RoutedEventArgs e)
        {
            catalogGrid.ItemsSource = books;
            listbooksbyauthor.Clear();
            listbooksbycategory.Clear();
            listbooksbyprogress.Clear();
            listbooksbytypelit.Clear();
            listbooksbyseveralparams.Clear();
        }

        // поиск по нескольким параметрам
        void SeveralParams(object sender, RoutedEventArgs e)
        {
            listbooksbycategory.Clear();
            listbooksbyprogress.Clear();
            listbooksbytypelit.Clear();
            listbooksbyauthor.Clear();

            Research.ResearchByParams window = new Research.ResearchByParams();
            window.Owner = this;            

            window.CategoryComboBox.ItemsSource = listcategory;
            window.ProgressComboBox.ItemsSource = listprogress;
            window.TypeComboBox.ItemsSource = listLiteraterTypes;

            if (window.ShowDialog() == true)
            {
                if (window.CategoryRadioButton.IsChecked == false && window.AuthorRadioButton.IsChecked == false &&
                    window.TypeRadioButton.IsChecked == false && window.ProgressRadioButton.IsChecked == false)
                {
                    MessageBox.Show("Выберите что-нибудь!");
                }
                else
                {
                    if (window.AuthorRadioButton.IsChecked == true)
                    {
                        string name = window.AuthorTextBox.Text;
                        listbooksbyseveralparams = new ObservableCollection<Book>(books.Where(b => b.Author == name)); //.OrderBy(b => b).Select(b => b);   
                    }
                    if (window.ProgressRadioButton.IsChecked == true)
                    {
                        string progress = window.ProgressComboBox.SelectedItem.ToString();
                        if (listbooksbyseveralparams.Count == 0)
                        {
                            var filter = books.Where(b => b.Progress.Contains(progress));
                            listbooksbyseveralparams = new ObservableCollection<Book>(filter);
                        }
                        else
                        {
                            var num = books.Where(b => b.Progress.Contains(progress));
                            listbooksbyseveralparams = new ObservableCollection<Book>(listbooksbyseveralparams.Where(b => b.Progress.Contains(progress)));
                        }
                            
                    }
                    
                    if (window.CategoryRadioButton.IsChecked == true)
                    {
                        string category = window.CategoryComboBox.SelectedItem.ToString();
                        if (listbooksbyseveralparams.Count == 0)
                        {
                            listbooksbyseveralparams = new ObservableCollection<Book>(books.Where(b => b.Category == category));
                        }
                        else
                            listbooksbyseveralparams = new ObservableCollection<Book>(listbooksbyseveralparams.Where(b => b.Category == category));
                    }
                    if (window.TypeRadioButton.IsChecked == true)
                    {
                        string type = window.TypeComboBox.SelectedItem.ToString();
                        if (listbooksbyseveralparams.Count == 0)
                        {
                            listbooksbyseveralparams = new ObservableCollection<Book>(books.Where(b => b.LiteratureType == type));
                        }
                        else
                            listbooksbyseveralparams = new ObservableCollection<Book>(listbooksbyseveralparams.Where(b => b.LiteratureType == type));
                    }
                    if (listbooksbyseveralparams.Count == 0)
                        MessageBox.Show("Ничего не найдено");
                    else
                        catalogGrid.ItemsSource = listbooksbyseveralparams;
                }    
                /*
                string name = window.AuthorTextBox.Text;                // имя автора
                var filtlist = books.Where(b => b.Author == name); //.OrderBy(b => b).Select(b => b);
                listbooksbyauthor = new ObservableCollection<Book>(filtlist);
                if (listbooksbyauthor.Count != 0)
                    catalogGrid.ItemsSource = listbooksbyauthor;
                else
                    MessageBox.Show("Ничего не найдено!");

                */
            }
        }
    }
}
