using System;
using System.ComponentModel.DataAnnotations;

namespace GraphQlDemo.Models
{
    public class Book
    {
        [Key]
        public string Isbn { get; set; }

        public string Title { get; set; }

        public int AuthorId { get; set; }

        public int PublisherId { get; set; }

        public string SubTitle { get; set; }

        public DateTime Published { get; set; }

        public int Pages { get; set; }

        public string Description { get; set; }

        public Uri Website { get; set; }

        // navigation properties
        public virtual Author Author { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
