using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TourGuide.Models;
using System.Net;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TourGuide.Controllers.Api
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private ITripRepository _repository;

        public RoutesController(ITripRepository repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            var result = Mapper.Map<IEnumerable<RouteViewModel>>(_repository.GetAllRoutesWithPoints());
            return Json(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]RouteViewModel vm)
        {
            try
            {
                var newRoute = Mapper.Map<Route>(vm);
                if (ModelState.IsValid)
                {
                    //Save to database
                    _repository.Add(newRoute);
                    if (await _repository.SaveAll())
                    {

                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<RouteViewModel>(newRoute));
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json("Point haven't been added");
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Point haven't been added");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Failed");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
