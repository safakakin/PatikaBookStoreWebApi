using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BookOperations.CreateBooks;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.GetBooksById;
using WebApi.BookOperations.UpdateBooks;
using WebApi.DBOperations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static WebApi.BookOperations.CreateBooks.CreateBookCommand;
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

        public BookController(BookStoreDbContext context)
        {
            _context = context;
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
            GetBooksQuery query = new GetBooksQuery(_context);
            var result = query.Handle();
            return Ok(result);
        }



        //Parametrenin route'dan alınması (daha doğru bir yaklaşımdır.)
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            GetBooksByIdQuery query = new GetBooksByIdQuery(_context);
            try
            {
                query.Handle(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(query.Handle(id));
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

            CreateBookCommand command = new CreateBookCommand(_context);
            try
            {
                command.Model = newBook;
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

            UpdateBookCommand command = new UpdateBookCommand(_context);

            try
            {
                command.Model = updatedBook;
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
            var book = _context.Books.SingleOrDefault(x => x.Id == id);

            if (book is null)
                return BadRequest();

            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok();
        }

    }
}
