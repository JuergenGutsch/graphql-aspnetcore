using System;

namespace GraphQlDemo.Query.Models
{
    public class Author
    {
        public Author()
        {
            Books = new Book[] { };
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public Book[] Books { get; set; }
    }
}
