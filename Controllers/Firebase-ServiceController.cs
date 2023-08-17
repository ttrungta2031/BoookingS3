using BoookingS3.Models;
using BoookingS3.Services;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BoookingS3.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class firebaseserviceController : ControllerBase

    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly bookings3Context _context;
        private static string apiKey = "AIzaSyAyhkbjsPw286UTN49IpQkoxWMGQxtH03k";

        private static string Bucket = "bookings3-44438.appspot.com";
        private static string AuthEmail = "ttrungta2100@gmail.com";
        private static string AuthPassword = "Test@123";
        public firebaseserviceController(IConfiguration config, bookings3Context context, IHostingEnvironment env)
        {
            _config = config;
            _context = context;
            _env = env;
        }

      
        [HttpPost("loginfirebaseEmailPass")]
        public async Task<IActionResult> LoginEmailPass(string email, string password)
        {
            //validate firebase authen
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            //validate authen = email + pass
            var a = await auth.SignInWithEmailAndPasswordAsync(email,password);
            //validate auth = token tu FE
            // var au = await auth.SignInWithCustomTokenAsync();
            string tokenfb = a.FirebaseToken;
            
            // var userfb = await auth.GetUserAsync(a);
            if (tokenfb != "")
            {
                var hasuser = _context.Users.SingleOrDefault(p => p.Email == email && password == p.Password);
                if (hasuser == null)
                {
                    return BadRequest(" The User not exist or Invalid username/password!!");
                }
                var token = GenerateJwtToken(hasuser);
                
                if (hasuser != null)
                {
                    return Ok(new { StatusCode = 200, Message = "Authenticate success", data = token, tokenfb });
                }
            }

                // var hasuser = _context.Users.SingleOrDefault(p => p.UserName == user.UserName && user.Password == p.Password && p.RoleId == 1);
                /*    
                    var existuser = await _context.Users.FindAsync(user.Id);
                  //  var result =  _context.Users.Where(x => x.Id == user.Id).Select(x => new { x.UserName, x.Role, x.RoleId, x.Password }).ToArray();

                    if (existuser == null) return  BadRequest(" The User not exist");
                    if (existuser.UserName == user.UserName && existuser.Password==user.Password &&existuser.RoleId == 1)
                    {

                        var token = GenerateJwtToken(user);
                        return Ok(token);
                    }*/
                return BadRequest("Invalid User");
        }


        [HttpPost("logincustomer")]
        public async Task<IActionResult> loginCustomer(string email, string password)
        {
            var b = GetAccount_byUsername(email);
            var a = GetAccount_byEmail(email);
           

            if (a != null || b!= null)
            {
                var hasuser = _context.Users.SingleOrDefault(p => (p.Email.ToUpper() == email.ToUpper() || p.UserName.ToUpper()==email.ToUpper()) && password == p.Password && p.RoleId==2 && p.Status == true);
                if (hasuser == null)
                {
                    return BadRequest(" The User not exist or Invalid email/password!!");
                }

                if (hasuser != null)
                {
                    if(b == null)
                    {
                        b = GetAccount_byEmail(email);
                    }

                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));// url > firebase
                    var authen = await auth.SignInWithEmailAndPasswordAsync(b.Email, password);
                    string tokenfb = authen.FirebaseToken;
                    //  var send = await auth.SendEmailVerificationAsync(tokenfb);

                    var cusid = (from cu in _context.Customers
                                     join us in _context.Users on cu.UserId equals us.Id
                                     where cu.UserId == hasuser.Id
                                     select new
                                     {
                                         CustomerId = cu.Id
                                     }).ToList();

                    var token = GenerateJwtToken(hasuser);

                    if (tokenfb != "")
                    {                      
                            return Ok(new { StatusCode = 200, Message = "Login with customer successful", data =cusid,token});                       
                    }


                   
                }
            }
            return BadRequest("Invalid User or Invalid email/password");
        }



        [HttpPost("loginspaowner")]
        public async Task<IActionResult> loginSpaowner(string email, string password)
        {
            var b = GetAccount_byUsername(email);
            var a = GetAccount_byEmail(email);


            if (a != null || b != null)
            {
                var hasuser = _context.Users.SingleOrDefault(p => (p.Email.ToUpper() == email.ToUpper() || p.UserName.ToUpper() == email.ToUpper()) && password == p.Password && p.RoleId == 3 && p.Status == true);
                if (hasuser == null)
                {
                    return BadRequest(" The User not exist or Invalid email/password!!");
                }

                if (hasuser != null)
                {
                    if (b == null)
                    {
                        b = GetAccount_byEmail(email);
                    }

                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var authen = await auth.SignInWithEmailAndPasswordAsync(b.Email, password);
                    string tokenfb = authen.FirebaseToken;
                    if (tokenfb != "")
                    {
                        return Ok(new { StatusCode = 200, Message = "Login with spaowner successful" });
                    }



                }
            }
            return BadRequest("Invalid User or Invalid email/password");
        }


        [HttpPost("loginfirebasewithtoken")]
        public async Task<IActionResult> LoginWithToken(string token)
        {
            //validate firebase authen
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            //validate authen = email + pass
         //   var a = await auth.SignInWithEmailAndPasswordAsync(user.Email, user.Password);
            //validate auth = token tu FE
         //    var au = await auth.SignInWithCustomTokenAsync(token);
            var au = await auth.GetUserAsync(token);
            
          //  string tokenfb = a.FirebaseToken;
         //    var userfb = await auth.GetUserAsync(au);
            if (token != "")
            {
                var hasuser = _context.Users.SingleOrDefault(p => p.Email == au.Email);
                if (hasuser == null)
                {
                    return BadRequest(" The User not exist or Invalid username/password!!");
                }
                var tokenjwt = GenerateJwtToken(hasuser);
                if (hasuser != null)
                {
                    return Ok(new { StatusCode = 200, Message = "Authenticate success", data = tokenjwt });
                }
            }
            return BadRequest("Invalid Token");
        }




        [HttpPost("sendmessage")]
        public async Task<IActionResult> SendNotifi(string title, string body)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });
            
            var fcmToken = "fKkLgEfjJC5RiR0xaHFGDn:APA91bGL9WuFwTRMGkXdd4wYcv3iQ4W-wIkkLrpC3vpnDwhrYF9rdLcmqJWIsn3CUcud2rGW4bCf0YjY0PKNlZVWZ89HSh7ibfCgLqnClgZnaZERwow4j15uquxqwKxlS1iu3agYrZ1v";
            //  fcm cua web: var fcmToken = "fKkLgEfjJC5RiR0xaHFGDn:APA91bGL9WuFwTRMGkXdd4wYcv3iQ4W-wIkkLrpC3vpnDwhrYF9rdLcmqJWIsn3CUcud2rGW4bCf0YjY0PKNlZVWZ89HSh7ibfCgLqnClgZnaZERwow4j15uquxqwKxlS1iu3agYrZ1v";
            //  eEE7zdTlTbuXcwuX32NQu2: APA91bFVAUk78aqB0 - Xgzdy7qi5 - wwLgRLXxIbQcuijxALJTfpegEkj - WdwT_OskwSQgh7 - kUTclXZ2YUg_Plf4S1VM3rLxlYdVZtE2 - LqYtWgAX9Tp8XnOmoLA1l4FDLzREk2sFEMmD
            // fcmtoken moi cai thiet bi
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    {"Mydataaa","SE1417aaa" },
                },
                Token = fcmToken,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            string res =  FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            if (res != "")
            {
                return Ok(new { StatusCode = 200, Message = "Successfully sent message", data = res,title,body });
            }
            return BadRequest("Error with FCMtoken");
        }


        private Models.User GetAccount_byEmail(string email)
        {
            var account = _context.Users.Where(a => a.Email.ToUpper() == email.ToUpper()).FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            return account;
        }


        private Models.User GetAccount_byUsername(string username)
        {
            var account = _context.Users.Where(a => a.UserName.ToUpper() == username.ToUpper()).FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            return account;
        }
      









        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            var fileupload = file;
            FileStream fs = null ;
            if (fileupload.Length > 0)
            {
                { 
                string foldername = "firebaseFiles";
                string path = Path.Combine($"Images", $"Images/{foldername}");


                if (Directory.Exists(path))
                {

                        using (fs = new FileStream(Path.Combine(path, fileupload.FileName), FileMode.Create))
                        {

                            await fileupload.CopyToAsync(fs);
                        }
                        
                            fs = new FileStream(Path.Combine(path, fileupload.FileName), FileMode.Open);
                        

                    }
                  
                
                else
                {
                    Directory.CreateDirectory(path);
                }
            }
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);


            var cancel = new CancellationTokenSource();

                var upload = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                    ).Child("images").Child(fileupload.FileName).PutAsync(fs, cancel.Token);

                // await upload;
                try
                {
                    string link = await upload;
                    return Ok(new { StatusCode = 200, Message = "Upload FIle success" });
                }
                catch(Exception ex)
                {
                    throw;
                }
           
        }
               
            
            return BadRequest("Failed Upload");
    }







        private string GenerateJwtToken(Models.User user)
        {
            var securitykey = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]);
            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Name, user.Password)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securitykey), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
    
        }
    }

}
