﻿using System;
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
    public sealed class UserController : ApiController
    {
        private readonly UserRepository _dbContext = new UserRepository();

        // GET api/user
        [Route, HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _dbContext.GetAll();
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

        // GET api/user/5
        [Route("{id}", Name = "GetUserById"), HttpGet]
        public User Get(int id)
        {
            return _dbContext.Find(id);
        }

        // PUT api/user/5
        [Route("{id}"), HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]User user)
        {
            var response = new HttpResponseMessage();
            if (user != null)
            {
                var updateSuccessful = _dbContext.Update(user);
                if (updateSuccessful)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Headers.Location = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Forbidden;
                }                
            }
            else
            {
                response.StatusCode = HttpStatusCode.Forbidden;
            }
            return response;
        }

        // DELETE api/user/5
        [Route("{id}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var response = new HttpResponseMessage();
            var removeSuccessful = _dbContext.Remove(id);
            response.StatusCode = removeSuccessful ? HttpStatusCode.OK : HttpStatusCode.Forbidden;
            return response;
        }


        // PUT api/user/5/jointeam/5
        [Route("{userId}/jointeam/{teamId}"), HttpPut]
        public HttpResponseMessage JoinTeam(int userId, int teamId)
        {
            var response = new HttpResponseMessage();
            var joinSuccessful = _dbContext.JoinTeam(userId, teamId);
            if (joinSuccessful)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Location = new Uri(Url.Link("GetUserById", new {id = userId}));
            }
            else
            {
                response.StatusCode = HttpStatusCode.Forbidden;
            }

            return response;
        }

        //DELETE api/user/5/leaveteam/5
        [Route("{userId}/leaveteam/{teamId}"), HttpPut]
        public HttpResponseMessage LeaveTeam(int userId, int teamId)
        {
            var response = new HttpResponseMessage();

            var leaveSuccessful = _dbContext.LeaveTeam(userId, teamId);

            if (leaveSuccessful)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Location = new Uri(Url.Link("GetUserById", new { id = userId }));
            }
            else
            {
                response.StatusCode = HttpStatusCode.Forbidden;
            }

            return response;
        }
    }
}
