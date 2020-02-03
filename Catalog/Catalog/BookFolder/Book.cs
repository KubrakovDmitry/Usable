using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BookFolder
{
    [Serializable]
    class Book
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Author { set; get; }
        public string Description { set; get; }
        public string Progress { set; get; }
        public string Category { get; set; }
        public string Path { get; set; }
        public string LiteratureType { set; get; }
    }
}
