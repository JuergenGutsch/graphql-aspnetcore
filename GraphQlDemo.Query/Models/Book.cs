namespace GraphQlDemo.Query.Models
{
    public class Book
    {
        public string Isbn { get; set; }

        public string Name { get; set; }

        public Author Author { get; set; }

        public Publisher Publisher { get; set; }
    }
}
