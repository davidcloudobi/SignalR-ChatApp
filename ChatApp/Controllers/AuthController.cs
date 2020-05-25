using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR;
using SignalR.JWT;

namespace ChatApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       


        private readonly UserManager<UserModel> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ChatDbContent _chatDbContent;

        public AuthController(UserManager<UserModel> userManager,IJwtGenerator jwtGenerator, SignInManager<UserModel> signInManager, ChatDbContent chatDbContent)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _chatDbContent = chatDbContent;
        }

        
        [HttpPost("reg")]
        public async Task<IActionResult> Register(InputModel inputModel)
        {


            var res = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == inputModel.userName);

            if (res == null)
            {

                var newUser = new UserModel()
                {
                    UserName = inputModel.userName

                };
                var result = await _userManager.CreateAsync(newUser, inputModel.password);

                var chatUser = new User()
                {
                    UserName = newUser.UserName
                };
                await _chatDbContent.User.AddAsync(chatUser);
               await _chatDbContent.SaveChangesAsync();

                if (result.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(newUser);
                    return Ok(token);
                }



                //var user = new List<Claim>
                //{new Claim(ClaimTypes.Name , inputModel.userName),
                //    new Claim(ClaimTypes.Anonymous , inputModel.userName)

                //};



                ////#############################################################
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, inputModel.userName)
                //};

                //var claimsIdentity = new ClaimsIdentity(
                //    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var authProperties = new AuthenticationProperties();

                //await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme,
                //    new ClaimsPrincipal(claimsIdentity),
                //    authProperties);
                ////####################################################

                //   var claims = new List<Claim>
                //   {
                //       new Claim(ClaimTypes.Name , inputModel.userName),
                //       new Claim(ClaimTypes.Anonymous , inputModel.userName),
                //       new Claim(ClaimTypes.Role, "Administrator")
                //       //new Claim("FullName", user.FullName),
                //       //new Claim(ClaimTypes.Role, "Administrator"),
                //   };

                //   var UserIdentity = new ClaimsIdentity(claims, "User Identity");
                //   var userPrincipal = new ClaimsPrincipal(new[] { UserIdentity });
                //await   HttpContext.SignInAsync(userPrincipal);
            }
          


           //var newUser = new UserModel()
           //{
           //    UserName =inputModel.userName

           //};
           // var result = await _userManager.CreateAsync(newUser, inputModel.password);

           // if (result.Succeeded)
           // {
           //     var token = _jwtGenerator.CreateToken(newUser);
           //     return Ok(token);
           // }
          
          //  var result = await _signInManager.CheckPasswordSignInAsync(user, newUser.Password, false);

            return Ok();
        }

        [Authorize]
        [HttpPost("login")]

        public async Task<IActionResult> Login(InputModel inputModel)
        {
           // var accessToken = await HttpContext.GetTokenAsync("access_token");
        
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == inputModel.userName);
            
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, inputModel.password, false);
                if (result.Succeeded)
                {
                    var User = new List<Claim>()
                    {new Claim(ClaimTypes.Name , user.UserName),
                        new Claim(ClaimTypes.Anonymous , user.UserName)

                    };

                    var UserIdentity = new ClaimsIdentity(User, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { UserIdentity });
                    await HttpContext.SignInAsync(userPrincipal);
                }
            }



            //var newUser = new UserModel()
            //{
            //    UserName = username

            //};
           // var result = await _userManager.CreateAsync(newUser, password);
             

            return Ok();
        }
    }
}