using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.DBOperations;

namespace WebApi.Controllers
{
  [ApiController]
  [Route("[controller]s")]
  public class BookController : ControllerBase
  {
    private readonly BookStoreDbContext _context;

    public BookController(BookStoreDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public List<Book> GetBooks()
    {
      var bookList = _context.Books.OrderBy(x => x.Id).ToList<Book>();
      return bookList;
    }

    // [HttpGet]
    // public Book Get([FromQuery] int id)
    // {
    //   var book = BookList.Where(x => x.Id == id).SingleOrDefault();
    //   return book;
    // }

    [HttpGet("{id}")]
    public Book GetBooksById(int id)
    {
      var book = _context.Books.Where(x => x.Id == id).SingleOrDefault();
      return book;
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] Book newBook)
    {
      var book = _context.Books.SingleOrDefault(x => x.Title == newBook.Title);
      if (book is not null)
        return BadRequest();

      _context.Books.Add(newBook);
      _context.SaveChanges();
      return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] Book updateBook)
    {
      var book = _context.Books.SingleOrDefault(x => x.Id == id);
      if (book is null)
        return BadRequest();

      book.GenreId = updateBook.GenreId != default ? updateBook.GenreId : book.GenreId;
      book.PageCount = updateBook.PageCount != default ? updateBook.PageCount : book.PageCount;
      book.PublishDate = updateBook.PublishDate != default ? updateBook.PublishDate : book.PublishDate;
      book.Title = updateBook.Title != default ? updateBook.Title : book.Title;

      _context.SaveChanges();

      return Ok();
    }

    [HttpDelete("{id}")]
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