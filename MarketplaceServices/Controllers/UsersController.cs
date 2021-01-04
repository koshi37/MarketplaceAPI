using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MarketplaceDataAccess;
using System.Security.Claims;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;

namespace MarketplaceServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        marketplaceEntities db = new marketplaceEntities();

        //private readonly AppSettings _appSettings;

        /*public UsersController(IOptions<AppSettings> appSettings)
        {
            db.Configuration.ProxyCreationEnabled = false;
            _appSettings = appSettings.Value;
        }*/

        [HttpGet]
        public IEnumerable<user> GetList()
        {
            return db.user.ToList();
        }
        //pobierz listę bez haseł
        [HttpGet]
        public IEnumerable<user> GetAll()
        {
            var list = db.user.ToList();
            return list.Select(x => {
                x.password = null;
                return x;
            });
        }

        [HttpGet]
        public user GetUserByName(string username)
        {
            return db.user.FirstOrDefault(u => u.username == username);
        }

        [HttpGet]
        public IEnumerable<user> FindUser(string searchString)
        {
            var users = from u in db.user
                        select u;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(a => a.username.Contains(searchString));
            }

            return users;
        }

        [HttpGet]
        public user GetUserById(int id)
        {
            return db.user.FirstOrDefault(u => u.user_id == id);
        }

        [HttpGet]
        public user Login(string username, string password)
        {
            var user = db.user.FirstOrDefault(u => u.username == username);
            if(!(user is null))
            {
                if (user.password == password)
                {
                    user.password = "";
                    return user;
                }
                else
                    return null;
            }
            else
                return null;
        }
        /*
        public user Authenticate(string username, string password)
        {
            var _users = db.user;
            var user = _users.SingleOrDefault(x => x.username == username && x.password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.user_id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.password = null;

            return user;
        }*/
        /*
        [HttpGet]
        public Object GetToken()
        {
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://mysite.com";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", "1"));
            permClaims.Add(new Claim("name", "bilal"));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }
        */
        [HttpGet]
        public String GetUsername(int id)
        {
            var user = db.user.Where(u => u.user_id == id).FirstOrDefault();
            return user.username;
        }

        [Authorize]
        [HttpPost]
        public Object GetName2()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var name = claims.Where(p => p.Type == "name").FirstOrDefault()?.Value;
                return new
                {
                    data = name
                };

            }
            return null;
        }

        [HttpPost]
        public IHttpActionResult Register(user _user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var ifUsernameTaken = db.user.Any(u => u.username == _user.username);
            var ifEmailTaken = db.user.Any(u => u.email == _user.email);

            if (ifUsernameTaken)
            {
                return BadRequest("Already exist user with that username.");
            }
            else if (ifEmailTaken)
            {
                return BadRequest("Email taken.");
            }
            else
            {
                

                try
                {
                    var maxUserId = db.user.Max(u => u.user_id);
                    var nextUserId = maxUserId + 1;

                    db.user.Add(new user()
                    {
                        user_id = nextUserId,
                        username = _user.username,
                        password = _user.password,
                        email = _user.email,
                        phone = String.IsNullOrEmpty(_user.phone) ? "" : _user.phone,
                        address = String.IsNullOrEmpty(_user.address) ? "" : _user.address
                    });
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return Ok("Registered successfully");
            }
                
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

                var _user = db.user
                    .Where(u => u.user_id == id)
                    .FirstOrDefault();

                db.Entry(_user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

            return Ok($"Successfully deleted user {_user.username}");
        }

        [HttpPut]
        public IHttpActionResult UpdateImage(user _user)
        {
            var existingUser = db.user.Where(u => u.user_id == _user.user_id).FirstOrDefault<user>();

            if (existingUser != null)
            {
                existingUser.image_url = _user.image_url;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateEmail(user _user)
        {
            var existingUser = db.user.Where(u => u.user_id == _user.user_id).FirstOrDefault<user>();

            if (existingUser != null)
            {
                existingUser.email = _user.email;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult UpdatePhone(user _user)
        {
            var existingUser = db.user.Where(u => u.user_id == _user.user_id).FirstOrDefault<user>();

            if (existingUser != null)
            {
                existingUser.phone = _user.phone;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateAddress(user _user)
        {
            var existingUser = db.user.Where(u => u.user_id == _user.user_id).FirstOrDefault<user>();

            if (existingUser != null)
            {
                existingUser.address = _user.address;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateUser(user _user)
        {
            var existingUser = db.user.Where(u => u.user_id == _user.user_id).FirstOrDefault<user>();

            if (existingUser != null)
            {
                existingUser.address = _user.address;
                existingUser.phone = _user.phone;
                existingUser.email = _user.email;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult SetPermissions(user _user)
        {
            var existingUser = db.user.Where(u => u.user_id == _user.user_id).FirstOrDefault<user>();

            if (existingUser != null)
            {
                existingUser.permissions = _user.permissions;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
