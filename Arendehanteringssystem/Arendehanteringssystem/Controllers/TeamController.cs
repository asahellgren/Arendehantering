using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.Http;
using DAL.Entities;
using DAL.Repositories;

namespace Arendehanteringssystem.Controllers
{
    [RoutePrefix("team")]
    public class TeamController : ApiController
    {
        private readonly TeamRepository _dBContext = new TeamRepository();

        // GET api/team
        [Route(Name = "GetAllTeams"), HttpGet]
        public IEnumerable<Team> Get()
        {
            var result = _dBContext.GetAll();
            if (result == null)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Could not process request", Encoding.UTF8, "text/plain")
                };
                throw new HttpResponseException(response);
            }
            return result;
        }

        // GET api/team/5
        [Route("{id}", Name = "GetTeamById"), HttpGet]
        public Team Get(int id)
        {
            var team = _dBContext.Find(id);
            if (team == null)
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("TeamId does not exist", Encoding.UTF8, "text/plain");
                throw new HttpResponseException(response);
            }
            return team;
        }

        // POST api/team
        [Route, HttpPost]
        public HttpResponseMessage Post([FromBody]Team team)
        {
            var newTeam = _dBContext.Add(team);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Headers = { Location = new Uri(Url.Link("GetTeamById", new { id = newTeam.Id })) }
            };
            return response;
        }

        // PUT api/team
        [Route, HttpPut]
        public HttpResponseMessage Put([FromBody]Team team)
        {
            var response = new HttpResponseMessage();
            if (team != null)
            {
                if (_dBContext.Update(team))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Headers.Location = new Uri(Url.Link("GetTeamById", new { id = team.Id }));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
            }
            return response;
        }

        // PUT api/team/5/addmember/5
        [Route("{teamid}/addmember/{userId}", Name = "AddTeamMember"), HttpPut]
        public HttpResponseMessage Put(int teamId, int userId)
        {
            var response = new HttpResponseMessage();
            if (_dBContext.AddTeamMember(teamId, userId))
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Location = new Uri(Url.Link("GetTeamById", new { id = teamId }));
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        // DELETE api/team/5
        [Route("{id}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = _dBContext.Remove(id) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return response;
        }

        //DELETE api/team/5/removemember/5
        [Route("{teamId}/removemember/{userId}", Name = "RemoveTeamMember"), HttpDelete]
        public HttpResponseMessage Delete(int teamId, int userId)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = _dBContext.RemoveTeamMember(teamId, userId) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return response;
        }
    }
}