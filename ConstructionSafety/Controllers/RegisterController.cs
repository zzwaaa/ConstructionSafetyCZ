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
using qcloudsms_csharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ViewModels;
using WisdomDbCore.WisdomModels;

namespace ConstructionSafety.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly WisdomPlatDBContext _context;
        private readonly ProjLiefInsDBContext _life;
        private JwtSettings settings;

        public RegisterController(ILogger<RegisterController> logger,WisdomPlatDBContext context,ProjLiefInsDBContext projLief,IOptions<JwtSettings> options)
        {
            _logger = logger;
            _context = context;
            settings = options.Value;
            _life = projLief;
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel<string>> Register(RegisterViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.Account)
                    ||string.IsNullOrWhiteSpace(viewModel.Password)
                    ||string.IsNullOrWhiteSpace(viewModel.Phone))
                {
                    return ResponseViewModel<string>.Create(Status.FAIL, "请将注册信息填写完整");
                }

                //验证验证码是否正确
                var code = RedisHelper.Get(viewModel.Phone + "JSCZcode");
                if (code != viewModel.Captcha)
                {
                    return ResponseViewModel<string>.Create(Status.FAIL,"验证码错误");
                }
                //手机号不允许重复
                var hasMobile = _life.LifeInUserInfos.Where(u => u.Telephone == viewModel.Phone).SingleOrDefault();
                if (hasMobile!=null)
                {
                    return ResponseViewModel<string>.Create(Status.ERROR,"该手机号已被注册，不可重复注册");
                }
                ////根据手机号去项目中匹配该人员信息是否存在 如不存在则 不给予注册
                //var personInfo = await _context.UserInfos.Where(u => u.Telephone == viewModel.Phone && u.Deleted == 0).FirstOrDefaultAsync();
                //if (personInfo==null)
                //{
                //    return ResponseViewModel<string>.Create(Status.FAIL,Message.FAIL,"没有获取到您的个人信息，无法完成当前注册！");
                //}
                var now = DateTime.Now;
                var perId = SecurityManage.GuidUpper();
                LifeInUserInfo userInfo = new LifeInUserInfo
                {
                    DeleteMark = 0,
                    CreateTime = now,
                    Telephone = viewModel.Phone,
                    UserInfoId = SecurityManage.GuidUpper(),
                    UserName=viewModel.Phone,
                    Password = TokenValidate.StrConversionMD5(viewModel.Password).ToUpper(),
                    PersonId=perId,
                    //Avatar=personInfo.Avatar
                };
                await _life.LifeInUserInfos.AddAsync(userInfo);
                await _life.SaveChangesAsync();
                return ResponseViewModel<string>.Create(Status.SUCCESS,Message.SUCCESS,"注册成功，请重新登录");
            }
            catch (Exception ex)
            {
                _logger.LogError("注册：" + ex.Message + "\r\n" + ex.StackTrace, ex);
                return ResponseViewModel<string>.Create(Status.ERROR, "注册失败！" + ex.Message + ex.StackTrace);
            }
        }



        /// <summary>
        /// 获取校验码是否正确
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseViewModel<string>> CheckVerifiPwdCode(string phone,string code,string pwd) 
        {
            try
            {
                object codeNew = await RedisHelper.GetAsync(phone+ "JSCZcode");
                if (codeNew!=null && code.Equals(codeNew.ToString()))
                {
                    var query = await _life.LifeInUserInfos
                              .Where(s => s.Telephone == phone && s.DeleteMark == 0)
                              .FirstOrDefaultAsync();
                    if (query==null)
                    {
                        return ResponseViewModel<string>.Create(Status.ERROR,"修改密码失败");
                    }
                   
                    query.Password = TokenValidate.StrConversionMD5(pwd).ToUpper();
                    _life.LifeInUserInfos.UpdateRange(query);
                    await _life.SaveChangesAsync();
                    return ResponseViewModel<string>.Create(Status.SUCCESS, Message.SUCCESS);
                }
                else
                {
                    return ResponseViewModel<string>.Create(Status.ERROR, "验证码输入错误！");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("验证码：" + ex.Message + "\r\n" + ex.StackTrace, ex);
                return ResponseViewModel<string>.Create(Status.ERROR, "验证码输入错误！");
            }
        
        }























        /// <summary>
        /// 发送手机验证码
        /// machuanlong
        /// 2019-04-22
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseViewModel<string>> SendVerifiCode(string phone)
        {
            try
            {
                int appid = 1400082495;
                string appkey = "40aceebea6c48b82ef448315c1228bc2";
                var templateId = 105140;
                SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
                string smsSign = "";
                Random random = new Random();
                var code = random.Next(1000, 9999).ToString();
                var result = ssender.sendWithParam("86", phone,
                    templateId, new[] { code, "10" }, smsSign, "", "");
                await RedisHelper.SetAsync(phone + "JSCZcode", code, 600);
                return ResponseViewModel<string>.Create(Status.SUCCESS, Message.SUCCESS, "验证码发送成功！请注意查收");
            }
            catch (Exception ex)
            {
                _logger.LogError("发送短信验证码：" + ex.Message + "\r\n" + ex.StackTrace, ex);
                return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR, "验证码发送失败！请稍后重试");

            }
        }




        /// <summary>
        /// 获取验证码是否正确
        /// css
        /// 2019-06-20
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseViewModel<string>> CheckVerifiCode(string phone, string code)
        {
            try
            {
                object codeNew = await RedisHelper.GetAsync(phone + "JSCZcode");
                if (codeNew != null && code.Equals(codeNew.ToString()))
                {
                    return ResponseViewModel<string>.Create(Status.SUCCESS, Message.SUCCESS, "验证成功");
                }
                else
                {
                    return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR, "验证失败");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("验证码：" + ex.Message + "\r\n" + ex.StackTrace, ex);
                return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR, "验证失败，请稍后再试");
            }
        }
    }
}
