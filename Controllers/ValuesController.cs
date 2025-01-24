using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using Työtunnit_API.Models;

namespace Työtunnit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   public class UsersController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public UsersController(DataContext dbContext)
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
        // POST: api/Users/Register
        [HttpPost("Register")]
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
                try
                {
                    var USER = _dbContext.Users.Where(t => t.Name == _Name && t.Password == _Password);
                    return Ok(USER);
                }
                catch (Exception e)
                {
                    return BadRequest("Maaaaan another error?" + e);
                }
                
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
    [Route("api/[controller]")]
    [ApiController]
    public class WorkhoursController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public WorkhoursController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public ActionResult AddItem([FromBody] Workhour body)
        {
            try
            {
                var Person = _dbContext.Users.Find(body.UId);
                body.UserName = Person.Name;
                _dbContext.Workhours.Add(body);
                _dbContext.SaveChanges();
                return Ok("Successfully added hours for "+body.UserName);
            }
            catch (Exception ex)
            {

                return BadRequest("Error: " + ex);
            }
        }
        [HttpGet("FetchUserHours")]
        public ActionResult FetchUsrData(int _UId, int _Role)
        {
            switch (_Role) 
            {
                case 1 or 2:
                    try
                    {
                        var userHours = _dbContext.Workhours.Where(t => t.UId == _UId);
                        return Ok(userHours);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest("Catch error: " + ex);
                    }
                case 3:
                    try
                    {
                        var userHours = _dbContext.Workhours;
                        return Ok(userHours);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest("Catch error: " + ex);
                    }
                default:
                    return BadRequest("No Access at role 0");
            }
        }
        [HttpPost("ModifyHours")]
        public ActionResult AdminModifyHours(int _Role, int _Id, string _Type, int _NewContentInt,string? _NewContentStr)
        {
            if (_Role != 3 && _Role != 2) { return BadRequest("Not an Admin or Project leader"); }
            try
            {
                var Hours = _dbContext.Workhours.Find(_Id);
                var Edited = "";
                switch (_Type)
                {
                    case "Edit Hours":
                        Hours.Hours = _NewContentInt;
                        Edited = "Edited Hours";
                    break;
                    case "Delete Hours":
                        if (_Role != 3) { return BadRequest("Not an Admin"); }
                        _dbContext.Remove(Hours);
                        Edited = "Deleted Hours";
                    break;
                    case "Comment":
                        Hours.Comment = _NewContentStr;
                        Edited = "Commented Data";
                    break;
                }
                _dbContext.SaveChanges();
                return Ok(Edited+" from id "+_Id);
            }
            catch (Exception ex)
            {

                return BadRequest("ModHours Error: " + ex);
            }

        }
    }
}