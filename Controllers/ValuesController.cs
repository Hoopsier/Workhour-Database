using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Työtunnit_API.Models;

namespace Työtunnit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _dbContext;
        public UsersController(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            try
            {
                var customers = _dbContext.Users.ToList();
                return Ok(customers);
            }
            catch (Exception ex)
            {

                return BadRequest("error: " + ex.Message);
            }
        }
        // GET: api/User
        [HttpGet("Name/{tname}")]
        public ActionResult GetUserByName(string tname) //id is used instead of email, due emails basically being an ID
        {
            
            try
            {
                var user = _dbContext.Users.Where(t => t.Name.Contains(tname));
                if (user == null)
                {
                    return BadRequest("Henkilöä sanalla: " + tname + " ei löydy.");
                }
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException);
            }
        }
        // POST: api/User
        [HttpPost]
        public ActionResult Register(string _Name, string _Password)
        {
            try
            {
                User user = new User();
                user.Id = 0;
                user.Name = _Name;
                user.Password = _Password;
                user.Role = 1;
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return Ok("Käyttäjä luotu onnistuneesti");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException);
            }
        }
        // GET: api/User
        [HttpGet("Login")]
        public ActionResult Login(int _Id, string _Password)
        {
            try
            {
                var user = _dbContext.Users.Where(t => t.Id == _Id && t.Password == _Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException);
            }
        }


        [HttpPut("UpdateUserData")]
        public ActionResult PriviledgeChange(int _Id, int _Role)
        {
            try
            {
                var user = _dbContext.Users.Find(_Id);
                if (user == null)
                {
                    return BadRequest("Edit failed");
                }
                user.Role = _Role;
                _dbContext.SaveChanges();
                return Ok("Edit Succeeded");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException);
                
            }
        }
    }
}
