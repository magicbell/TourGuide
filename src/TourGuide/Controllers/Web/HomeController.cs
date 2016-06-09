using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using TourGuide.Models;
using Microsoft.AspNet.Authorization;
using TourGuide.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TourGuide.Controllers
{
    public class HomeController : Controller
    {
        
        private ITripRepository _repository;

        public HomeController(ITripRepository repository)
        {

            _repository = repository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Routes()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contact)
        {
            return View();
        }
    }
}
