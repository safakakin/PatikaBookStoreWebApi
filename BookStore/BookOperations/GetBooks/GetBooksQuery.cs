using System;
using AutoMapper;
using BookStore;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.GetBooks
{
	public class GetBooksQuery
	{
		//private readonly sayesinde sadece constructer içerisinden set edilmesini sağlayabiliriz. 
		private readonly BookStoreDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetBooksQuery(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<BooksViewModel> Handle()
		{
            var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList<Book>();
			List<BooksViewModel> wm = _mapper.Map<List<BooksViewModel>>(bookList);
			//new List<BooksViewModel>();
			//foreach (var book in bookList)
			//{
			//	wm.Add(new BooksViewModel()
			//	{
			//		Title = book.Title,
			//		Genre = ((GenreEnum)book.GenreId).ToString(),
			//		PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy"),
			//		PageCount=book.PageCount
			//	});
			//}
            return wm;
        }
	}

	public class BooksViewModel
	{
		public string Title { get; set; }
		public int PageCount { get; set; }
		public string PublishDate { get; set; }
		public string Genre { get; set; }
    }
}