using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApi.Services;
using WebApi.Utility;
using WepApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            try
            {
                var user = _userService.Authenticate(userParam.KullaniciAdi, userParam.Sifre);

                if (user == null)
                    return BadRequest(new { message = "Kullanici veya şifre hatalı!" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            try
            {
                bool isUserExist;
                isUserExist = _userService.IsUserExist(user);

                if (isUserExist)
                    return BadRequest("Kullanıcı adı sistemde kayıtlıdır.");

                var users = _userService.Insert(user);

                return Ok(users);
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }
        }
    }
}