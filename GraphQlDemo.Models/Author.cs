using System.Collections.Generic;
using System.Linq;

namespace GraphQlDemo.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<int> Publishers { get; set; }
    }
}
