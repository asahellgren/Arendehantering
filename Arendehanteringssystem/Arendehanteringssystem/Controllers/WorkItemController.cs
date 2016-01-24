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
    [RoutePrefix("WorkItem")]
    public class WorkItemController : ApiController
    {
        private readonly WorkItemRepository _dbContext = new WorkItemRepository();
        // GET api/<controller>
        [HttpGet, Route(Name = "GetAllWorkItems")]
        public IEnumerable<WorkItem> GetAll()
        {
            var result = _dbContext.GetAll();
            return result;
        }

        // GET api/<controller>/5
        [Route("{id}", Name = "GetWorkItemById"), HttpGet]
        public WorkItem Get(int id)
        {
            var result = _dbContext.Find(id);
            return result;
        }

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]WorkItem item)
        {
            WorkItem newItem = _dbContext.Add(item);
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Headers = { Location = new Uri(Url.Link("GetWorkItemById", new { id = newItem.Id })) }
            };
            return response;
        }

        // PUT api/<controller>/5
        [HttpPut, Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]WorkItem item)
        {
            WorkItem updatedItem = _dbContext.Update(item);
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Headers = { Location = new Uri(Url.Link("GetWorkItemById", new { id = updatedItem.Id })) }
            };
            return response;
        }

        // DELETE api/<controller>/5
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            _dbContext.Remove(id);
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };
            return response;
        }
        // GET api/<controller>/5
        [Route("{id}/SetStatus", Name = "SetStatus"), HttpGet]
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
        
        // GET api/<controller>/5
        [Route("{id}/AssignUser", Name = "AsignUser"), HttpGet]
        public HttpResponseMessage SetStatus(int id, int userId)
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

        //GET api/workitem?search=frontend
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
    }
}
