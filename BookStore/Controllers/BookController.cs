using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BookOperations.CreateBooks;
using WebApi.BookOperations.DeleteBook;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.GetBooksById;
using WebApi.BookOperations.UpdateBooks;
using WebApi.DBOperations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static WebApi.BookOperations.CreateBooks.CreateBookCommand;
using static WebApi.BookOperations.GetBooksById.GetBooksByIdQuery;
using static WebApi.BookOperations.UpdateBooks.UpdateBookCommand;

namespace WebApi.Controllers
{
    //Hangi endpoint nasıl erişileceğini gelen
    //requesti hangi resource'un karşılayacağı bilgisidir
    [Route("[controller]s")]


    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly BookStoreDbContext _context; //readonly değiştirilememesi için
        private readonly IMapper _mapper;

        public BookController(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //private static List<Book> BookList = new List<Book>()
        //{
        //    new Book{Id=1,Title="Lean Startup",GenreId=1,PageCount=200,PublishDate=new DateTime(2001,06,12)},
        //    new Book{Id=2,Title="Herland",GenreId=2,PageCount=250,PublishDate=new DateTime(2010,05,23)},
        //    new Book{Id=3,Title="Dune",GenreId=2,PageCount=540,PublishDate=new DateTime(2008,12,21)}
        //};


        //Sadece 1 tane parametresiz httpget alabilir.
        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context,_mapper);
            var result = query.Handle();
            return Ok(result);
        }



        //Parametrenin route'dan alınması (daha doğru bir yaklaşımdır.)
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            BooksViewIdModel result;
            try
            {
                GetBooksByIdQuery query = new GetBooksByIdQuery(_context, _mapper);
                query.BookId = id;
                GetBooksByIdQueryValidator validator = new GetBooksByIdQueryValidator();
                validator.ValidateAndThrow(query);
                result=query.Handle(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);

        }
        //[HttpGet]
        //public Book Get([FromQuery] string id)
        //{
        //    var book = BookList.Where(book => book.Id == Convert.ToInt32(id) ).SingleOrDefault();
        //    return book;
        //}

        [HttpPost]

        public IActionResult AddBook([FromBody]CreateBookModel newBook)
        {

            CreateBookCommand command = new CreateBookCommand(_context,_mapper);
            try
            {
                command.Model = newBook;
                CreateBookCommandValidator validator = new CreateBookCommandValidator();
                validator.ValidateAndThrow(command);

                //ValidationResult result = validator.Validate(command);
                //if(!result.IsValid)
                //    foreach (var item in result.Errors)
                //        Console.WriteLine("Özellik "+item.PropertyName+"- Error Message: "+item.ErrorMessage);
                //else
                //command.Handle();
                command.Handle();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok();
        }

        [HttpPut("{id}")]

        public IActionResult UpdateBook (int id,[FromBody]UpdateBookModel updatedBook)
        {
            try
            {
                UpdateBookCommand command = new UpdateBookCommand(_context);
                command.BookId = id;
                command.Model = updatedBook;
                UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
            
        }

        [HttpDelete(("{id}"))]

        public IActionResult DeleteBook(int id)
        {
            try
            {
                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id;
                DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

    }
}
