using System;
using AutoMapper;
using BookStore;
using WebApi.BookOperations.GetBooks;
using WebApi.Common;
using static WebApi.BookOperations.CreateBooks.CreateBookCommand;
using static WebApi.BookOperations.GetBooksById.GetBooksByIdQuery;

namespace WebApi
{
	public class MappingProfile:Profile
	{
		public MappingProfile()
		{
			CreateMap<CreateBookModel, Book>();
			CreateMap<Book, BooksViewIdModel>().ForMember(dest=>dest.Genre,opt=>opt.MapFrom(src=>((GenreEnum)src.GenreId).ToString()));
			CreateMap<Book, BooksViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()));
		}
	}
}

