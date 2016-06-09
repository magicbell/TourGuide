using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TourGuide.Models;
using System.Net;
using AutoMapper;
using Microsoft.AspNet.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TourGuide.Controllers.Api
{
    [Authorize]
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
            var routes = _repository.GetUserRoutesWithPoints(User.Identity.Name);
            var result = Mapper.Map<IEnumerable<RouteViewModel>>(routes);
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
                
                if (ModelState.IsValid)
                {
                    var newRoute = Mapper.Map<Route>(vm);

                    newRoute.UserName = User.Identity.Name;
                    
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
        [HttpDelete("{routeId}")]
        public async Task<JsonResult> Delete(int routeId)
        {
            try
            {
                _repository.DeleteRoute(routeId);
                if (await _repository.SaveAll())
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json("Point have been deleted succesfully");
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Point haven't been deleted");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Route haven't been deleted: " + ex.Message);
            }
        }

    }
}
