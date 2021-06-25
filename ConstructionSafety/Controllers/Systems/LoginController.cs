using Common.Common;
using ConstructionSafety.Tools;
using MCUtil.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ViewModels;

namespace ConstructionSafety.Controllers.Systems
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
       // private readonly WisdomPlatDBContext _context;
        private JwtSettings settings;
        private readonly ProjLiefInsDBContext _life;

        public LoginController(ILogger<LoginController> logger,/* WisdomPlatDBContext context,*/ProjLiefInsDBContext projLief,IOptions<JwtSettings> options)
        {
            _logger = logger;
            //_context = context;
            settings = options.Value;
            _life = projLief;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel<object>> Login(LoginViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Account) || string.IsNullOrWhiteSpace(viewModel.Password))
            {
                return ResponseViewModel<object>.Create(Status.FAIL,"账号或密码不能为空！");
            }
            try
            {
                //根据账号 查出该用户
                var user = _life.LifeInUserInfos.Where(u => u.UserName == viewModel.Account && u.DeleteMark == 0).FirstOrDefault();
                if (user == null)
                {
                    return ResponseViewModel<object>.Create(Status.FAIL,"账号不存在");
                }
                if (user.Password != TokenValidate.StrConversionMD5(viewModel.Password).ToUpper())
                {
                    return ResponseViewModel<object>.Create(Status.FAIL,"密码错误");
                }
                var claims = new Claim[] {
                            new Claim(nameof(TokenClaimTypeEnum.UserId),user.UserInfoId),
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    settings.Issuer,
                    settings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddDays(1),
                    creds);
                var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

                //登陆成功后 根据手机号对用户信息进行默认添加
                var perId = _life.LifeInUserInfos.Where(s => s.UserName == viewModel.Account).Select(s=>s.PersonId).FirstOrDefault();
                //var projPersonId = await _life.ProjectPoints.Where(s => s.PersonId == perId).FirstOrDefaultAsync();
                //if (projPersonId == null)
                //{
                //    var userInfo = _context.UserInfos
                //               .Where(s => s.Deleted == 0 && s.Status == 1 && s.Telephone == viewModel.Account).FirstOrDefault();
                //    var projInfos = await _context.ProjectInfo
                //                   .Where(p => p.ProjectInfoId == userInfo.ProjectInfoId).ToListAsync();
                //    var projName = projInfos.Select(p => p.ProjectName).FirstOrDefault();

                //    var projectIds = projInfos.Select(s => s.ProjectInfoId).FirstOrDefault();
                //    ProjectPoint project = new ProjectPoint
                //    {
                //        ProjectInfoId = projectIds,
                //        ProjectName = projName,
                //        PersonId = perId,
                //        PersonName = userInfo.Name,
                //        AllRanks = "0",
                //        ProjectRanks = "0"
                //    };
                //    _life.ProjectPoints.Add(project);
                //    IntegralInfo info = new IntegralInfo
                //    {
                //        IntegralNums = 0,
                //        PersonId = project.PersonId,
                //        ProjectInfoId = projectIds,
                //        UnIntegralNums = 0
                //    };
                //    _life.IntegralInfos.Add(info);
                //    await _life.SaveChangesAsync();

                //}

                var result = new PersonInfoViewModel
                {
                    userInfos = user,
                    PersonId = perId
                };

                return ResponseViewModel<object>.Create(0, "登录成功", result, 0, "bearer " + tokenResult);
            }
            catch (Exception ex)
            {
                _logger.LogError("用户登陆：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR,"登录失败");
            }
        }
    }
}
