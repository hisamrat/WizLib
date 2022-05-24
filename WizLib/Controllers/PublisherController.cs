using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinLib_Model.Models;
using WizLib_DataAccess.Data;

namespace WizLib.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PublisherController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var objlist = _applicationDbContext.Publishers.ToList();
            return View(objlist);
        }

        public IActionResult Upsert(int? id)
        {
            Publisher obj = new Publisher();
            if(id==null)
            {
                return View(obj);

            }
            obj = _applicationDbContext.Publishers.FirstOrDefault(c => c.Publisher_Id == id);
                if(obj==null)
            {
                return null;
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher publisher)
        {
            if(ModelState.IsValid)
            {
                if(publisher.Publisher_Id==null)
                {
                    _applicationDbContext.Publishers.Add(publisher);
                }
                else
                {

                    _applicationDbContext.Publishers.Update(publisher);
                }
                _applicationDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }
        public IActionResult Delete(int? id)
        {
            
            var obj = _applicationDbContext.Publishers.FirstOrDefault(c => c.Publisher_Id == id);
            _applicationDbContext.Publishers.Remove(obj);
            _applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
       
    }
}
