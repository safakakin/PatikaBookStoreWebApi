using System;
using FluentValidation;
using WebApi.BookOperations.UpdateBooks;

namespace WebApi.BookOperations.GetBooksById
{
	public class GetBooksByIdQueryValidator: AbstractValidator<GetBooksByIdQuery>
    {
		public GetBooksByIdQueryValidator()
		{
			RuleFor(query => query.BookId).GreaterThan(0);
		}
	}
}

