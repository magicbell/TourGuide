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
        private WikiLoadService _wikiLoadService;

        public PointsController(ITripRepository repository, CoordService coordService, WikiLoadService wikiLoadService)
        {
            _repository = repository;
            _coordService = coordService;
            _wikiLoadService = wikiLoadService;
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
                else return Json(Mapper.Map<IEnumerable<PointViewModel>>(result.Points.OrderBy(t => t.OrdNumber)));
                
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

        // PUT api/values/5/6
        [HttpPut("api/routes/{routeName}/[controller]/{id1}/{id2}")]
        public async Task<JsonResult> Put(int id1, int id2)
        {
            try
            {
                _repository.SwapPoints(id1, id2);
                if (await _repository.SaveAll())
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json("Points have been swaped succesfully");
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Points haven't been swaped");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE api/values/5
        [HttpDelete("api/routes/{routeName}/[controller]/{pointId}")]
        public async Task<JsonResult> Delete(string routeName, int pointId)
        {
            try
            {
                _repository.DeletePointFromRoute(routeName, pointId);
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
                return Json("Point haven't been deleted: " + ex.Message);
            }
        }

        //Getting wikiPages
        [HttpGet("api/routes/{routeName}/[controller]/wiki/{radius}")]
        public async Task<JsonResult> Get(string routeName, double radius)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    //Taking all points from current route
                    var points = _repository.GetRouteByName(routeName).Points;

                    if (points == null)
                        return Json("Points weren't found");
                    else
                    {
                        List<WikiLoadServiceResult> result = new List<WikiLoadServiceResult>();

                        //For every point looking up Wiki pages
                        foreach (var point in points)
                        {
                            var wikiParams = new WikiLoadServiceParams(radius, point.Latitude, point.Longitude);
                            var wikiResult = await _wikiLoadService.Lookup(wikiParams);


                            if (!wikiResult.Success)
                            {
                                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                return Json(wikiResult.Message);
                            }
                            else result.Add(wikiResult);
                        }
                        return Json(result);
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Parameters for wiki request aren't valid");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Wiki pages cann't be found because " + ex.Message);
            }
        }
    }
}
