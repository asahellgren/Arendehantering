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
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        private readonly UserRepository _dbContext = new UserRepository();

        // GET api/user
        [Route(Name = "GetAllUsers"), HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _dbContext.GetAll();
        }

        // GET api/user/5
        [Route("{id}", Name = "GetUserById"), HttpGet]
        public User Get(int id)
        {
            return _dbContext.Find(id);
        }

        // GET api/user/5/teams
        [Route("{id}/teams"), HttpGet]
        public User GetUserWithTeams(int id)
        {
            return _dbContext.GetUserWithTeams(id);
        }

        // POST api/user
        [Route, HttpPost]
        public HttpResponseMessage Post([FromBody]User user)
        {
            var newUser = _dbContext.Add(user);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Headers = { Location = new Uri(Url.Link("GetUserById", new { id = newUser.Id })) }
            };
            return response;
        }

        // PUT api/user
        [Route, HttpPut]
        public HttpResponseMessage Put([FromBody]User user)
        {
            var response = new HttpResponseMessage();
            if (user != null)
            {
                var updatedUser = _dbContext.Update(user);

                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Location = new Uri(Url.Link("GetUserById", new { id = updatedUser.Id }));
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        // PUT api/user/5/addteam/5
        [Route("{userId}/addteamtouser/{teamId}", Name = "AddTeamToUser"), HttpPut]
        public HttpResponseMessage PutTeamToUser(int userId, int teamId)
        {
            var updatedUserId = _dbContext.AddUserToTeam(userId, teamId);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Headers = { Location = new Uri(Url.Link("GetUserById", new { id = updatedUserId })) }

            };
            return response;
        }

        // DELETE api/user/5
        [Route("{id}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            _dbContext.Remove(id);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        //DELETE api/user/5/removeteamfromuser/5
        [Route("{userId}/removeteamfromuser/{teamId}", Name = "RemoveTeamFromUser"), HttpDelete]
        public HttpResponseMessage Delete(int userId, int teamId)
        {
            _dbContext.RemoveTeamFromUser(userId, teamId);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }





    }
}
