using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Repositories;
using DAL.Entities;

namespace Arendehanteringssystem.Controllers
{
    [RoutePrefix("WorkItem")]
    public class WorkItemController : ApiController
    {
        private readonly WorkItemRepository dbContext = new WorkItemRepository();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<WorkItem> GetAll()
        {
            var result = dbContext.GetAll();
            return result;
        }

        // GET api/<controller>/5
        [Route("{id}", Name = "GetWorkItemById"), HttpGet]
        public WorkItem Get(int id)
        {
            var result = dbContext.Find(id);
            return result;
        }

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]WorkItem item)
        {
            WorkItem newItem = dbContext.Add(item);
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Headers = { Location = new Uri(Url.Link("GetWorkItemById", new { id = newItem.Id })) }
            };
            return response;
        }

        // PUT api/<controller>/5
        [Route("{id}", Name = "UpdateWorkItem"), HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]WorkItem item)
        {
            WorkItem updatedItem = dbContext.Update(item);
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Headers = { Location = new Uri(Url.Link("GetWorkItemById", new { id = updatedItem.Id })) }
            };
            return response;
        }

        // DELETE api/<controller>/5
        [Route("{id}", Name = "DeleteWorkItem"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            dbContext.Remove(id);
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };
            return response;
        }
    }
}
