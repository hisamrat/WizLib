using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinLib_Model.Models;
using WizLib_DataAccess.Data;

namespace WizLib.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthorController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var objlist = _applicationDbContext.Authors.ToList();
            return View(objlist);
        }

        public IActionResult Upsert(int? id)
        {
            Author obj = new Author();
            if(id==null)
            {
                return View(obj);

            }
            obj = _applicationDbContext.Authors.FirstOrDefault(c => c.Author_Id == id);
                if(obj==null)
            {
                return null;
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author author)
        {
            if(ModelState.IsValid)
            {
                if(author.Author_Id==0)
                {
                    _applicationDbContext.Authors.Add(author);
                }
                else
                {

                    _applicationDbContext.Authors.Update(author);
                }
                _applicationDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }
       
        
        public IActionResult Delete(int? id)
        {
           var obj= _applicationDbContext.Authors.FirstOrDefault(c => c.Author_Id == id);
            _applicationDbContext.Remove(obj);
            _applicationDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
