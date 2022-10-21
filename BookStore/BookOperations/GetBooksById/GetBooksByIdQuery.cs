using System;
using BookStore;
using System.Linq;
using WebApi.BookOperations.GetBooks;
using WebApi.Common;
using WebApi.DBOperations;
using Microsoft.EntityFrameworkCore;

namespace WebApi.BookOperations.GetBooksById
{
	public class GetBooksByIdQuery
	{
		private readonly BookStoreDbContext _dbContext;

		public GetBooksByIdQuery(BookStoreDbContext dbContext)
		{
			_dbContext = dbContext;
		}

        public BooksViewIdModel Handle(int id)
        {
           
           var book = _dbContext.Books.Where(book => book.Id == id).SingleOrDefault();
           if (book is null)
                throw new InvalidOperationException("Belirtilen Id'ye sahip kitap mevcut değildir.");

           BooksViewIdModel wm = new BooksViewIdModel();
           {
              wm.Title = book.Title;
              wm.Genre = ((GenreEnum)book.GenreId).ToString();
              wm.PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy");
              wm.PageCount = book.PageCount;
           };
           
           return wm;

        }

        public class BooksViewIdModel
		{
			public string Title { get; set; }
            public int PageCount { get; set; }
            public string PublishDate { get; set; }
            public string Genre { get; set; }
        };
	}
}

