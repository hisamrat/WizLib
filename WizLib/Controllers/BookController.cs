using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinLib_Model.Models;
using WinLib_Model.ViewModels;
using WizLib_DataAccess.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BookController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var objlist = _applicationDbContext.Books.Include(i=>i.Publisher).
                Include(u=>u.BookAuthors).ThenInclude(u=>u.Author).ToList();

            //List<Book> objlist = _applicationDbContext.Books.ToList();
            
            //foreach(var obj in objlist)
            //{
            //    _applicationDbContext.Entry(obj).Reference(i => i.Publisher).Load();
            //    _applicationDbContext.Entry(obj).Collection(u => u.BookAuthors).Load();
            //    foreach(var bookauthor in obj.BookAuthors)
            //    {
            //        _applicationDbContext.Entry(bookauthor).Reference(i => i.Author).Load();
            //    }
            //    //least efficient it does load duplicate value
            //    //obj.Publisher = _applicationDbContext.Publishers.FirstOrDefault(c => c.Publisher_Id == obj.Publisher_Id);

            //    //explicit loading more efficient it does not load duplicate value
            //   // _applicationDbContext.Entry(obj).Reference(u => u.Publisher).Load();
            
            
            //}
            return View(objlist);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();

            //ViewBag.Publisher = new SelectList(_applicationDbContext.Publishers.ToList(), "Publisher_Id", "Name");
            obj.PublisherList = _applicationDbContext.Publishers.Select(i => new SelectListItem
            {

                Text = i.Name,
                Value = i.Publisher_Id.ToString()

            });
            if (id == null)
            {
                return View(obj);

            }
            obj.Book = _applicationDbContext.Books.FirstOrDefault(c => c.Book_Id == id);
            //obj.Book = _applicationDbContext.Books.FirstOrDefault(c => c.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
           
                if (obj.Book.Book_Id == 0)
                {
                    _applicationDbContext.Books.Add(obj.Book);
                }
                else
                {

                    _applicationDbContext.Books.Update(obj.Book);
                }
                _applicationDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            
            
        }

        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();

            //ViewBag.Publisher = new SelectList(_applicationDbContext.Publishers.ToList(), "Publisher_Id", "Name");
           
            if (id == null)
            {
                return View(obj);

            }
            obj.Book = _applicationDbContext.Books.Include(u=>u.BookDetail).FirstOrDefault(c => c.Book_Id == id);
            //obj.Book.BookDetail = _applicationDbContext.BookDetails.FirstOrDefault(u => u.BookDetail_Id == obj.Book.BookDetail_Id);
            
            //obj.Book = _applicationDbContext.Books.FirstOrDefault(c => c.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {

            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                _applicationDbContext.BookDetails.Add(obj.Book.BookDetail); ;
                _applicationDbContext.SaveChanges();
                var BookFromDb = _applicationDbContext.Books.FirstOrDefault(c => c.Book_Id == obj.Book.Book_Id);
                BookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _applicationDbContext.SaveChanges();
            
            
            }
            else
            {

                _applicationDbContext.BookDetails.Update(obj.Book.BookDetail);
                _applicationDbContext.SaveChanges();


            }
           
            return RedirectToAction(nameof(Index));


        }
        public IActionResult Delete(int? id)
        {

            var obj = _applicationDbContext.Books.FirstOrDefault(c => c.Book_Id == id);
            _applicationDbContext.Books.Remove(obj);
            _applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {

            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _applicationDbContext.BookAuthors.Include(b => b.Book).
                Include(i => i.Author).Where(u => u.Book_Id == id).ToList(),


                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },

                Book = _applicationDbContext.Books.FirstOrDefault(u => u.Book_Id == id)
        };

            List<int> tempListOfAssignedAuthors = obj.BookAuthorList.
                Select(u => u.Author_Id).ToList();

            //not in clause in linq

            var templist = _applicationDbContext.Authors.
                Where(u => !tempListOfAssignedAuthors.Contains(u.Author_Id)).ToList();

            obj.AuthorList = templist.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });
            return View(obj);


        }
        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {

            if(bookAuthorVM.BookAuthor.Book_Id!=0 && bookAuthorVM.BookAuthor.Author_Id!=0)
            {
                _applicationDbContext.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _applicationDbContext.SaveChanges();
            }

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });

            }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId,BookAuthorVM bookAuthorVM)
        {

            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _applicationDbContext.BookAuthors.
                FirstOrDefault(u => u.Author_Id == authorId && u.Book_Id == bookId);
            _applicationDbContext.BookAuthors.Remove(bookAuthor);
            _applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });

        }

    }
}
