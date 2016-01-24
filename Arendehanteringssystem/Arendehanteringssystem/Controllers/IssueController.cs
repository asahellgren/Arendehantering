using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            var response = new HttpResponseMessage();
            var issue = _dBContext.Find(id);
            if (issue == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("IssueId does not exist", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);

            }
            return issue;
        }

        // POST api/issue
        [HttpPost, Route]
        public HttpResponseMessage Post([FromBody]Issue issue)
        {
            var newIssue = _dBContext.Add(issue);
            var response = new HttpResponseMessage();
            if (newIssue == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Headers.Location = new Uri(Url.Link("GetIssueById", new { id = newIssue.Id }));
            }
            return response;
        }

        // PUT api/issue
        [Route("{id}"), HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Issue issue)
        {
            var response = new HttpResponseMessage();
            if (issue != null)
            {
                if (_dBContext.Update(issue))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Headers.Location = new Uri(Url.Link("GetIssueById", new { id = issue.Id }));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            else
            {
                response.StatusCode = HttpStatusCode.Forbidden;
            }
            return response;
        }


    }
}