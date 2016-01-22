using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using DAL.Entities;
using DAL.Repositories;
using Newtonsoft.Json;

namespace Arendehanteringssystem.Controllers
{
    [RoutePrefix("issue")]
    public class IssueController : ApiController
    {
        private readonly IssueRepository _dBContext = new IssueRepository();

        // GET api/issue/5
        [Route("{id}", Name = "GetIssueById")]
        public Issue Get(int id)
        {
            return _dBContext.Find(id);
        }

        // POST api/issue
        [HttpPost, Route]
        public HttpResponseMessage Post([FromBody]Issue issue)
        {
            var newIssue = _dBContext.Add(issue);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Headers = { Location = new Uri(Url.Link("GetIssueById", new { id = newIssue.Id })) },

            };
            return response;
        }

        // PUT api/issue
        [HttpPut, Route]
        public HttpResponseMessage Put([FromBody]Issue issue)
        {
            var updatedIssue = _dBContext.Update(issue);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Headers = { Location = new Uri(Url.Link("GetIssueById", new { id = updatedIssue.Id })) }
            };
            return response;
        }


    }
}