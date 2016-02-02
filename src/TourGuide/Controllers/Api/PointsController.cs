using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AutoMapper;
using TourGuide.Models;
using ViewModels;
using System.Net;
using TourGuide.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TourGuide.Controllers.Api
{
   // [Route("api/routes/{routeName}/[controller]")]
    public class PointsController : Controller
    {
        private ITripRepository _repository;
        private CoordService _coordService;

        public PointsController(ITripRepository repository, CoordService coordService)
        {
            _repository = repository;
            _coordService = coordService;
        }

        // GET: api/values
        [HttpGet("api/routes/{routeName}/[controller]")]
        public JsonResult Get(string routeName)
        {
            try
            {
                var result = _repository.GetRouteByName(routeName);
                if (result == null)
                    return Json(null);
                else return Json(Mapper.Map<IEnumerable<PointViewModel>>(result.Points.OrderBy(t => t.Arrival)));
                
            }
            catch(Exception ex)
                {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Couln't find a route named {routeName}");
            }
            
        }

        [HttpGet("api/allpoints")]
        public JsonResult Get()
        {
            try
            {
                return Json(_repository.GetAllPoints());
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Error with getting data about points" + ex.Message);
            }

        }

        //TODO
        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            try {
                var result = Mapper.Map<PointViewModel>(_repository.GetPoints(id));
                Response.StatusCode = (int) HttpStatusCode.Found;
                return Json(result);
            }
            catch(Exception)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(false);
            }
        }

        // POST api/values
        [HttpPost("api/routes/{routeName}/[controller]")]
        public async Task<JsonResult> Post(string routeName, [FromBody] PointViewModel vm)
        {
            try {
                
                if (ModelState.IsValid)
                {
                    //Map to the Entity
                    var newPoint = Mapper.Map<Point>(vm);

                    //Looking up Geocoordinates
                    var coordResult = await _coordService.Lookup(newPoint.Name);

                    if(!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(coordResult.Message);
                    }

                    newPoint.Latitude = coordResult.Latitude;
                    newPoint.Longitude = coordResult.Longitude;

                    //Save to Database
                    _repository.AddPointToRoute(routeName, newPoint);
                    if(await _repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json("Point have been added succesfully");
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
                    return Json("Point information isn't valid");
                }
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Point haven't been added because " + ex.Message);
            }
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
