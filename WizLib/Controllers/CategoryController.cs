using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinLib_Model.Models;
using WizLib_DataAccess.Data;

namespace WizLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            //use asnotracking when u user to retrive a list only read only parpuse
            var objlist = _applicationDbContext.Categories.AsNoTracking().ToList();
            return View(objlist);
        }

        public IActionResult Upsert(int? id)
        {
            Category obj = new Category();
            if(id==null)
            {
                return View(obj);

            }
            obj = _applicationDbContext.Categories.FirstOrDefault(c => c.Category_Id == id);
                if(obj==null)
            {
                return null;
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Category_Id==null)
                {
                    _applicationDbContext.Categories.Add(category);
                }
                else
                {

                    _applicationDbContext.Categories.Update(category);
                }
                _applicationDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public IActionResult Delete(int? id)
        {
            
            var obj = _applicationDbContext.Categories.FirstOrDefault(c => c.Category_Id == id);
            _applicationDbContext.Remove(obj);
            _applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateMultiple2(Category category)
        {
            List<Category> categories = new List<Category>();
            for(var i=1;i<=2;i++)
            {
                categories.Add(new Category { Name = "Samrat" });
            }
            _applicationDbContext.AddRange(categories);

            _applicationDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult CreateMultiple5(Category category)
        {
            List<Category> categories = new List<Category>();
            for (var i = 1; i <= 5; i++)
            {
                categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }
            _applicationDbContext.AddRange(categories);
            _applicationDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult RemoveMultiple2(Category category)
        {
            IEnumerable<Category> categories = _applicationDbContext.Categories.OrderByDescending(c => c.Category_Id).Take(2).ToList();

            _applicationDbContext.RemoveRange(categories);
            _applicationDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult RemoveMultiple5(Category category)
        {
            IEnumerable<Category> categories = _applicationDbContext.Categories.OrderByDescending(c => c.Category_Id).Take(5).ToList();
            
            _applicationDbContext.RemoveRange(categories);
            _applicationDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
