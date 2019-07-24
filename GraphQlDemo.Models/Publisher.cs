using System.Collections.Generic;

namespace GraphQlDemo.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // navigation properties
        public virtual ICollection<Book> Books { get; set; }
    }
}
