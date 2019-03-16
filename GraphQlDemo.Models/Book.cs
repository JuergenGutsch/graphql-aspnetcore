using System;

namespace GraphQlDemo.Models
{
    public class Book
    {
        public string Isbn { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public Author Author { get; set; }

        public Publisher Publisher { get; set; }

        public DateTime Published { get; set; }

        public int Pages { get; set; }

        public string Description { get; set; }

        public Uri Website { get; set; }
    }
}
