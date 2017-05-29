namespace GraphQlDemo.Models
{
    public class Publisher
    {
        public Publisher()
        {
            Books = new Book[] { };
            Authors = new Author[] { };
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Book[] Books { get; set; }

        public Author[] Authors { get; set; }
    }
}
