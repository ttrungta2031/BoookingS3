using BoookingS3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoookingS3.Common;
using BoookingS3.Services;

namespace BoookingS3.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class registerController : ControllerBase
    {
        private Random randomGenerator = new Random();
        private readonly bookings3Context _context;
        private readonly IMailService _mailService;

        private static string apiKey = "AIzaSyAyhkbjsPw286UTN49IpQkoxWMGQxtH03k";

        private static string Bucket = "bookings3-44438.appspot.com";
        private static string AuthEmail = "ttrungta2100@gmail.com";
        private static string AuthPassword = "Test@123";
        public registerController(bookings3Context context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;    
        }

      

       


        [HttpPost("register_customer_v2")] //email    -Ta
        public async Task<IActionResult> dangkyCustomer(string emailuser, string fullname, string username, string password, string address, DateTime dob, int phone)
        {
            try
            {
                var auth = new Firebase.Auth.FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey));


                var email = emailuser;
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" }); //ok
                }
                User us = new User();
                var a = GetAccount_byEmail(emailuser);
                if (a != null)
                {
                    if (a.Status == true)
                    {
                        return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Account aldready exists)" });
                    }
                    else if (a.Status == false)
                    {
                        a.Password = password;
                        _context.SaveChanges();
                        await sendCodeEmail(a);

                      //  await auth.SendPasswordResetEmailAsync("tattse140962@fpt.edu.vn");


                        return Ok(new { StatusCode = 200, message = "Email re-sended" });
                    }
                }
                else if (a == null)
                {

                    us.FullName = fullname;
                    us.Address = address;
                    us.Dob = dob;
                    us.RoleId = 2;
                    us.Phone = phone;
                    us.CreateDay = DateTime.Now;
                    us.Status = true;
                    us.UserName = username;
                    us.Email = emailuser;
                    us.Password = password;

                    await _context.Users.AddAsync(us);
                    await _context.SaveChangesAsync();
                    await sendCodeEmail(us);
                   await auth.CreateUserWithEmailAndPasswordAsync(emailuser, password);

                    var customer = GetAccount_byEmail(emailuser);
                    if(customer != null) { 
                    Customer cus = new Customer();
                        cus.UserId = customer.Id;
                        await _context.Customers.AddAsync(cus);
                        await _context.SaveChangesAsync();

                    }
                    if (us.Id > 0)
                    {
                        return Ok(new { StatusCode = 200, Message = "Account registered successfully" });
                    }
                }
                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Can't create account)" });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (" + ex.Message + ")" });
            }
        }


        [HttpPost("register_spaowner_v2")] //email    -Ta
        public async Task<IActionResult> dangkySpaowner([FromBody] User user)
        {
            try
            {

                var auth = new Firebase.Auth.FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey));
                var email = user.Email;
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" }); //ok
                }
                User us = new User();
                var a = GetAccount_byEmail(user.Email);
                if (a != null)
                {
                    if (a.Status == true)
                    {
                        return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Account aldready exists)" });
                    }
                    else if (a.Status == false)
                    {
                        a.Password = user.Password;
                        _context.SaveChanges();
                        await sendCodeEmail(a);
                        return Ok(new { StatusCode = 200, message = "Email re-sended" });
                    }
                }
                else if (a == null)
                {

                    us.FullName = user.FullName;
                    us.Address = user.Address;
                    us.Dob = user.Dob;
                    us.RoleId = 3;
                    us.Phone = user.Phone;
                    us.CreateDay = DateTime.Now;
                    us.Status = true;
                    us.UserName = user.UserName;
                    us.Email = user.Email;
                    us.Password = user.Password;

                    await _context.Users.AddAsync(us);
                    await _context.SaveChangesAsync();
                    await sendCodeEmail(us);
                  await auth.CreateUserWithEmailAndPasswordAsync(user.Email, user.Password);

                    var spaowner = GetAccount_byEmail(user.Email);
                    if (spaowner != null)
                    {
                        Spa spa = new Spa();
                        spa.UserId = spaowner.Id;
                        await _context.Spas.AddAsync(spa);
                        await _context.SaveChangesAsync();

                    }


                    if (us.Id > 0)
                    {
                        return Ok(new { StatusCode = 200, Message = "Account registered successfully" });
                    }
                }
                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Can't create account)" });

            }
            catch (Exception ex)
            {

                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (" + ex.Message + ")" });
            }
        }

        [HttpPost("register_customer")] //email    -Ta
        public async Task<IActionResult> registerCustomer(string emailuser, string fullname, string username, string password, string address, DateTime dob, int phone)
        {
            try
            {
                var email = emailuser;
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" }); //ok
                }
                User us = new User();
                var a = GetAccount_byEmail(emailuser);
                if (a != null)
                {
                    if (a.Status == true)
                    {
                        return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Account aldready exists)" });
                    }
                    else if (a.Status == false)
                    {
                        a.Password = password;
                        _context.SaveChanges();
                        await sendCodeEmail(a);
                        return Ok(new { StatusCode = 200, message = "Email re-sended" });
                    }
                }
                else if (a == null)
                {

                    us.FullName = fullname;
                    us.Address = address;
                    us.Dob = dob;
                    us.RoleId = 2;
                    us.Phone = phone;
                    us.CreateDay = DateTime.Now;
                    us.Status = false;
                    us.UserName = username;
                    us.Email = emailuser;
                    us.Password = password;

                    await _context.Users.AddAsync(us);
                    await _context.SaveChangesAsync();
                    await sendCodeEmail(us);
                    if (us.Id > 0)
                    {
                        return Ok(new { StatusCode = 200, Message = "Account registered successfully" });
                    }
                }
                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Can't create account)" });

            }
            catch (Exception ex)
            {

                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (" + ex.Message + ")" });
            }
        }


        [HttpPost("register_spaowner")] //email    -Ta
        public async Task<IActionResult> registerSpaowner(string emailuser, string fullname, string username, string password, string address, DateTime dob, int phone)
        {
            try
            {
                var email = emailuser;
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" }); //ok
                }
                User us = new User();
                var a = GetAccount_byEmail(emailuser);
                if (a != null)
                {
                    if (a.Status == true)
                    {
                        return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Account aldready exists)" });
                    }
                    else if (a.Status == false)
                    {
                        a.Password = password;
                        _context.SaveChanges();
                        await sendCodeEmail(a);
                        return Ok(new { StatusCode = 200, message = "Email re-sended" });
                    }
                }
                else if (a == null)
                {

                    us.FullName = fullname;
                    us.Address = address;
                    us.Dob = dob;
                    us.RoleId = 3;
                    us.Phone = phone;
                    us.CreateDay = DateTime.Now;
                    us.Status = false;
                    us.UserName = username;
                    us.Email = emailuser;
                    us.Password = password;

                    await _context.Users.AddAsync(us);
                    await _context.SaveChangesAsync();
                    await sendCodeEmail(us);
                    if (us.Id > 0)
                    {
                        return Ok(new { StatusCode = 200, Message = "Account registered successfully" });
                    }
                }
                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (Can't create account)" });

            }
            catch (Exception ex)
            {

                return StatusCode(409, new { StatusCode = 409, message = "Account registered failed (" + ex.Message + ")" });
            }
        }



        [HttpPut("confirm")]  //-Ta
        public async Task<IActionResult> submitCode(string email, string code)
        {
            try
            {
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" });//ok
                }

                var a = GetAccount_byEmail(email);
                if (a != null)
                {
                    if (a.Status == true)
                    {
                        return StatusCode(409, new { StatusCode = 409, message = "Account aldready active" });//ok
                    }
                    else
                    {
                        if (a.Code == code)
                        {
                            a.Status = true;
                            a.Email = email;
                            _context.Users.Update(a);
                            await _context.SaveChangesAsync();

                            return Ok(new { StatusCode = 200, Message = "Email verification successfully" });
                        }
                        else
                        {
                            return StatusCode(400, new { StatusCode = 400, message = "Email verification failed (incorrect code)" }); //ok
                        }
                    }
                }
                return StatusCode(400, new { StatusCode = 400, message = "Email verification failed (account does not exist)" });//ok
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = "Account confirm failed (" + ex.Message + ")" });//ok
            }
        }


        // api/Register/resend  //-Ta
        [HttpPost("resend")]  //email  
        public async Task<IActionResult> reSendEmail(string email)
        {
            try
            {
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" });
                }
                var a = GetAccount_byEmail(email);
                if (a != null)
                {
                    if (a.Status == true)
                    {
                        await sendCodeEmail(a);
                        return StatusCode(200, new { StatusCode = 200, message = "Email re-send (The account already active, but the code to get the password if you forgot!)" });
                    }
                    else
                    {
                        await sendCodeEmail(a);
                        return StatusCode(200, new { StatusCode = 200, message = "Email re-send" }); 
                    }
                }
                return StatusCode(400, new { StatusCode = 400, message = "Email send failed (account does not exist)" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = "Account resend failed (" + ex.Message + ")" });
            }
        }

        // api/Register/change-pass //-Ta
        [HttpPut("change-pass")] //email, passhash, code   
        public async Task<IActionResult> changePass(string email, string code , string password)
        {
            try
            {
                email = addTailEmail(email);
                if (!Validate.isEmail(email))
                {
                    return StatusCode(409, new { StatusCode = 409, message = "Exception Email format" });//ok
                }

                var a = GetAccount_byEmail(email);
                if (a != null)
                {
                    if (a.Code == code)
                    {
                        a.Password = password;
                        _context.Users.Update(a);
                        await _context.SaveChangesAsync();

                        return Ok(new { StatusCode = 200, Message = "Password change successfully" });//ok
                    }
                    else
                    {
                        return StatusCode(400, new { StatusCode = 400, message = "Password change failed (incorrect code)" });//ok
                    }
                }
                return StatusCode(400, new { StatusCode = 400, message = "Password changen failed (account does not exist)" });//ok
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = "Account changepass failed (" + ex.Message + ")" });//ok
            }
        }


        //send Email(useraccount) //-Ta
        private async Task<bool> sendCodeEmail(User a)
        {
            try
            {
                a.Email = addTailEmail(a.Email);
                if (!Validate.isEmail(a.Email))
                {
                    return false;
                }
                a.Code = randomGenerator.Next(1, 100000).ToString(); //moi lan resend la reset code
                _context.Users.Update(a);
                await _context.SaveChangesAsync();
                var mailRequest = new MailRequest();
                mailRequest.ToEmail = a.Email;
                var username = mailRequest.ToEmail.Split('@')[0];
                mailRequest.Subject = "Hello " + username;
                mailRequest.Description = "sendCodeEmail";
                mailRequest.Value = a.Code;
                await _mailService.SendCodeEmailAsync(mailRequest);
                Console.WriteLine(mailRequest.ToEmail);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        // Get account by(email) //-Ta
        private User GetAccount_byEmail(string email)
        {
            var account = _context.Users.Where(a => a.Email.ToUpper() == email.ToUpper()).FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            return account;
        }


         //-Ta
        private string addTailEmail(string email)
        {
            if (!email.Contains("@")) //auto add ".com"              
            {
                return email + "@gmail.com";
            }
            return email;
        }
    }
}
