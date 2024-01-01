using AngularAuthYtAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using AngularAuthYtAPI.Context;
using Microsoft.EntityFrameworkCore;
using AngularAuthYtAPI.Helpers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AngularAuthYtAPI.Models.Dto;
using AngularAuthYtAPI.Utility;
using AngularAuthYtAPI.Models.ViewModel;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        public UserController(AppDbContext context, IConfiguration config, IEmailService emailService)
        {
            _authContext = context;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            try
            {
                if (userObj == null)
                    return BadRequest();

                var user = await _authContext.Users
                    .FirstOrDefaultAsync(x => x.Username == userObj.Username);

                if (user == null)
                    return NotFound(new { Message = "User not found!" });

                if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
                {
                    return BadRequest(new { Message = "Password is Incorrect" });
                }

                user.Token = CreateJwt(user);
                var newAccessToken = user.Token;
                var newRefreshToken = CreateRefreshToken();
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
                await _authContext.SaveChangesAsync();

                return Ok(new TokenApiDto()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] UserModel userObj)
        {
            try
            {


                if (userObj == null)
                    return BadRequest();

                // check email
                if (await CheckEmailExistAsync(userObj.Email))
                    return BadRequest(new { Message = "Email Already Exist" });

                //check username
                if (await CheckUsernameExistAsync(userObj.Username))
                    return BadRequest(new { Message = "Username Already Exist" });

                if (!await CheckValidReferal(userObj.Referal))
                    return BadRequest(new { Message = "Invalid Referral ID" });

                //var passMessage = CheckPasswordStrength(userObj.Password);
                //if (!string.IsNullOrEmpty(passMessage))
                //    return BadRequest(new { Message = passMessage.ToString() });

                var randomPassword = RandomString(8, true);
                var userCount = await _authContext.Users.CountAsync();
                var user = new User()
                {
                    Email = userObj.Email,
                    FirstName = userObj.FirstName,
                    LastName = userObj.LastName
                };
                user.Username = $"GET100{userCount}";

                user.Password = PasswordHasher.HashPassword(randomPassword);
                user.Role = "User";
                user.Token = "";
                await _authContext.AddAsync(user);
                await _authContext.SaveChangesAsync();

                var parentId = _authContext.Members.FirstOrDefault(x => x.Name.ToLower().Trim() == userObj.Referal.ToLower().Trim()).Id;

                var member = new Member()
                {
                    Name = user.Username,
                    ParentId = parentId,
                    JoiningDate = DateTime.Now,
                    PlanId = userObj.PlanType,
                    UserId = user.Id
                };
                await _authContext.AddAsync(member);
                await _authContext.SaveChangesAsync();

                var emailModel = new EmailModel(userObj.Email, "User Registered", EmailBodyUserInfo.EmailStringBodyUserInfo(userObj.Username, randomPassword));
                _emailService.SendEmail(emailModel);
                return Ok(new
                {
                    Status = 200,
                    Message = "User Registered. Please check your mailbox"
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private Task<bool> CheckEmailExistAsync(string? email)
            => _authContext.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckUsernameExistAsync(string? username)
            => _authContext.Users.AnyAsync(x => x.Email == username);

        private Task<bool> CheckValidReferal(string referal)
            => _authContext.Users.AnyAsync(x => x.Username.ToLower().Trim() == referal.ToLower().Trim());

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.Username}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _authContext.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }


        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity?.Name;
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");
            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _authContext.SaveChangesAsync();
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            try
            {
                var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user is null)
                {
                    return NotFound(new { StatusCode = 404, Message = "Email does not exists" });
                }
                var tokenByte = RandomNumberGenerator.GetBytes(64);
                var emailToken = Convert.ToBase64String(tokenByte);
                user.ResetPasswordToken = emailToken;
                user.ResetPasswordTokenExpiryTime = DateTime.Now.AddMinutes(15);
                var emailModel = new EmailModel(email, "Reset Password", EmailBodyResetPassword.EmailStringBodyResetPassword(email, emailToken));
                _emailService.SendEmail(emailModel);
                _authContext.Entry(user).State = EntityState.Modified;
                await _authContext.SaveChangesAsync();
                return Ok(new { StatusCode = 200, Message = "Email Sent" });

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto restPasswordDto)
        {
            var newToaken = restPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _authContext.Users.FirstAsync(x => x.Email == restPasswordDto.Email);
            if (user is null)
            {
                return NotFound(new { StatusCode = 404, Message = "User does not exists" });
            }

            var tokencode = user.ResetPasswordToken;
            var emailTokenExpiry = user.ResetPasswordTokenExpiryTime;

            if (tokencode != restPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new { StatusCode = 400, Message = "Token is expired" });
            }

            user.Password = PasswordHasher.HashPassword(restPasswordDto.NewPassword);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new { StatusCode = 200, Message = "Password Reset Successful" });
        }
    }



}
