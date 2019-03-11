using FluentValidation;
using GraphQlDemo.Models;
using System.Threading.Tasks;

namespace GraphQlDemo.Services.Validation
{
    public class CreateBookValidator : AbstractValidator<Book>
    {
        public CreateBookValidator(IBookService bookService)
        {
            this.RuleFor(m => m.Isbn)
                .NotNull()
                .CustomAsync( async (isbn, context, cancellationToken) =>
                {
                    var bookExists = await bookService.GetBookByIsbnAsync(isbn);
                    if (bookExists != null)
                    {
                        context.AddFailure("ISBN already exists");
                    }
                });
        }
    }
}
