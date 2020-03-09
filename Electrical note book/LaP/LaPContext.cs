namespace LaP
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class LaPContext : DbContext
    {
        // Контекст настроен для использования строки подключения "LaPContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "LaP.LaPContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "LaPContext" 
        // в файле конфигурации приложения.
        /*
        static LaPContext()
        {
            Database.SetInitializer<LaPContext>(new ContextInitializer());
        }
        */
        public LaPContext()
            : base("name=LaPContext")
        {
        }

        public DbSet<Account> Accounts { set; get; }
        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}