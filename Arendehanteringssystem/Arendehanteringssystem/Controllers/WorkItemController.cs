using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DAL.Repositories;
using DAL.Entities;

namespace Arendehanteringssystem.Controllers
{
    [RoutePrefix("workitem")]
    public sealed class WorkItemController : ApiController
    {
        private readonly WorkItemRepository _dbContext = new WorkItemRepository();

        // GET api/workitem?pageindex=1&pagesize=3
        [Route, HttpGet]
        public IEnumerable<WorkItem> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            var result = _dbContext.GetAll();
            if (result == null)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Could not process request", Encoding.UTF8, "text/plain")
                };
                throw new HttpResponseException(response);
            }
            if (pageIndex != null && pageSize != null)
            {
                return result.Skip(pageIndex.Value * pageSize.Value - pageSize.Value).Take(pageSize.Value);
            }
            return result;


        }

        // GET api/workitem/5
        [Route("{id}", Name = "GetWorkItemById"), HttpGet]
        public WorkItem Get(int id)
        {
            var response = new HttpResponseMessage();
            var result = _dbContext.Find(id);
            if (result == null)
            {

                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("UserId does not exist", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);
            }
            return result;
        }

        // POST api/workitem
        [Route, HttpPost]
        public HttpResponseMessage Post([FromBody]WorkItem item)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            WorkItem newItem = _dbContext.Add(item);
            if (newItem == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Headers.Location = new Uri(Url.Link("GetWorkItemById", new { id = newItem.Id }));

            }

            return response;
        }

        // PUT api/workitem/5
        [HttpPut, Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]WorkItem item)
        {
            var response = new HttpResponseMessage();
            if (item != null && id == item.Id)
            {
                if (_dbContext.Update(item))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Headers.Location = new Uri(Url.Link("GetWorkItemById", new { id = item.Id }));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        // DELETE api/workitem/5
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var response = new HttpResponseMessage();
            var removeSuccessful = _dbContext.Remove(id);
            response.StatusCode = removeSuccessful ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return response;
        }

        // GET api/workitem/5/SetStatus?done=true
        [Route("{id}/SetStatus", Name = "SetStatus"), HttpPut]
        public HttpResponseMessage SetStatus(int id, bool done)
        {
            var response = new HttpResponseMessage();
            var setStatusSuccessful = _dbContext.SetStatus(id, done);
            if (setStatusSuccessful)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Location = new Uri(Url.Link("GetWorkItemById", new { id }));
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        // GET api/workitem/5?userid=1
        [Route("{id}/AssignUser", Name = "AssignUser"), HttpPut]
        public HttpResponseMessage AssignUser(int id, int userId)
        {
            var response = new HttpResponseMessage();
            var setStatusSuccessful = _dbContext.AssignUser(id, userId);
            if (setStatusSuccessful)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Location = new Uri(Url.Link("GetWorkItemById", new { id }));
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        //GET api/workitem?search=userstring
        [Route, HttpGet]
        public IEnumerable<WorkItem> SearchForWorkitem(string search)
        {
            var response = new HttpResponseMessage();
            var result = _dbContext.Search(search);

            if (result == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Could not process request", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);
            }
            if (result.Count == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("No result found", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);
            }
            return result;
        }
        //GET api/workitem/history?startdate=2016-01-01&enddate=2016-02-01
        [Route("history"), HttpGet]
        public IEnumerable<WorkItem> History(DateTime startDate, DateTime endDate)
        {
            var response = new HttpResponseMessage();
            var result = _dbContext.GetAllDoneBetweenDates(startDate, endDate);

            if (result == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Could not process request", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);
            }
            if (result.Count == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("No result found", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);
            }
            return result;
        }
    }
}
