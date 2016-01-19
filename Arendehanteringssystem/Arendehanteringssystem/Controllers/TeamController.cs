using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Entities;
using DAL.Repositories;

namespace Arendehanteringssystem.Controllers
{
    [RoutePrefix("team")]
    public class TeamController : ApiController
    {
        private readonly TeamRepositoryRepository _dBContext = new TeamRepositoryRepository();

        // GET api/team
        [Route(Name = "GetAllTeams"), HttpGet]
        public IEnumerable<Team> Get()
        {
            return _dBContext.GetAll();
        }

        // GET api/team/5
        [Route("{id}", Name = "GetTeamById"), HttpGet]
        public Team Get(int id)
        {
            return _dBContext.Find(id);
        }

        // GET api/team/5
        [Route("{id}/details"), HttpGet]
        public Team GetTeamWithUsers(int id)
        {
            return _dBContext.GetTeamWithUser(id);
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
            var updatedTeam = _dBContext.Update(team);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Headers = { Location = new Uri(Url.Link("GetTeamById", new { id = updatedTeam.Id })) }
            };
            return response;
        }

        // PUT api/team/5/addmember/5
        [Route("{teamid}/addmember/{userId}", Name = "AddTeamMember"), HttpPut]
        public HttpResponseMessage Put(int teamId, int userId)
        {
            var updatedTeamId = _dBContext.AddTeamMember(teamId, userId);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Headers = { Location = new Uri(Url.Link("GetTeamById", new { id = updatedTeamId })) }

            };
            return response;
        }

        // DELETE api/team/5
        [Route("{id}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            _dBContext.Remove(id);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        //DELETE api/team/5/removemember/5
        [Route("{teamId}/removemember/{userId}", Name = "RemoveTeamMember"), HttpDelete]
        public HttpResponseMessage Delete(int teamId, int userId)
        {
            _dBContext.RemoveTeamMember(teamId, userId);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}