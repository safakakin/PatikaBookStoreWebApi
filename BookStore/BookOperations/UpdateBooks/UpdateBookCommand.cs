using System;
using static WebApi.BookOperations.CreateBooks.CreateBookCommand;
using WebApi.DBOperations;
using Microsoft.EntityFrameworkCore;

namespace WebApi.BookOperations.UpdateBooks
{
	public class UpdateBookCommand
	{
        public UpdateBookModel Model { get; set; }

        public int BookId { get; set; }

        private readonly BookStoreDbContext _dbContext;

        public UpdateBookCommand(BookStoreDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public void Handle(int _id)
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id ==_id);
            if (book is null)
                throw new InvalidOperationException("Belirtilen Id'ye sahip kitap mevcut değildir.");

            book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId;
            book.Title = Model.Title != default ? Model.Title : book.Title;
            _dbContext.SaveChanges();
        }

        public class UpdateBookModel
        {
            
            public string Title { get; set; }
            public int GenreId { get; set; }
        }
    }
}

