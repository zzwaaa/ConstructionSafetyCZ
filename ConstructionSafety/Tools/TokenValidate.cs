using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ViewModels;

namespace ConstructionSafety.Tools
{
    public class TokenValidate : ISecurityTokenValidator
    {
        //private string _secret;
        //public TokenValidate(string secret)
        //{
        //    _secret = secret;
        //}
        //public bool CanValidateToken => true;

        //public int MaximumTokenSizeInBytes { get; set; }

        //public bool CanReadToken(string securityToken)
        //{
        //    return true;
        //}

        public static string StrConversionMD5(string str)
        {
            MD5 md5 = MD5.Create();

            byte[] c = System.Text.Encoding.Default.GetBytes(str);

            byte[] b = md5.ComputeHash(c);//用来计算指定数组的hash值

            //将每一个字节数组中的元素都tostring，在转成16进制
            string newStr = null;
            for (int i = 0; i < b.Length; i++)
            {
                newStr += b[i].ToString("x2");  //ToString(param);//传入不同的param可以转换成不同的效果
            }
            return newStr;
        }


        ///// <summary>
        ///// 此处，如果需要，则需要配置，目前没有token验证
        ///// </summary>
        ///// <param name="securityToken"></param>
        ///// <param name="validationParameters"></param>
        ///// <param name="validatedToken"></param>
        ///// <returns></returns>
        //public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        //{

        //    try
        //    {
        //        ClaimsPrincipal principal;
        //        validatedToken = null;
        //        var token = new JwtSecurityToken(securityToken);

        //        #region 验签
        //        var rawHeader = token.RawHeader;
        //        var rawPayload = token.RawPayload;
        //        var rawSignature = token.RawSignature;
        //        var payload = token.Payload;
        //        //var bytesSignature = Encoding.UTF8.GetBytes(rawSignature);
        //        var keyBytes = Encoding.UTF8.GetBytes(_secret);
        //        //if (_secret == "9POS^YLo*9TWQ5KQ82#cKzU5La37@rHnOCK#sfBs9XshTXmgZtP2EZ*tvTPxsR6rp@Jt%RXmsbUKQ0lo28NN&BrLsAuD%dLjj1p")
        //        //{
        //        using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
        //        {

        //            // Compute the hash of the input file.
        //            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHeader + '.' + rawPayload));
        //            char[] padding = { '=' };
        //            var hashValueSignature = Convert.ToBase64String(hashValue, Base64FormattingOptions.None)
        //                .TrimEnd(padding).Replace('+', '-').Replace('/', '_');

        //            if (rawSignature != hashValueSignature)
        //            {
        //                // 签名被篡改
        //                return new ClaimsPrincipal();
        //            }
        //            else
        //            {
        //                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
        //                string tokenResult = "";
        //                // var citycode = "320400";
        //                var roleId = (from t in payload where t.Key == nameof(TokenClaimTypeEnum.RoleId) select t.Value).FirstOrDefault() ?? "";
        //                var userId = (from t in payload where t.Key == nameof(TokenClaimTypeEnum.UserId) select t.Value).FirstOrDefault() ?? "";
        //                var systemCode = (from t in payload where t.Key == nameof(TokenClaimTypeEnum.SystemCode) select t.Value).FirstOrDefault() ?? "";
        //                //var country = (from t in payload where t.Key == nameof(TokenClaimTypeEnumPat.NikeName) select t.Value).FirstOrDefault() ?? "";
        //                //var city = (from t in payload where t.Key == nameof(TokenClaimTypeEnumPat.NikeName) select t.Value).FirstOrDefault() ?? "";
        //                //var openId = (from t in payload where t.Key == nameof(TokenClaimTypeEnumPat.NikeName) select t.Value).FirstOrDefault() ?? "";
        //                //var province = (from t in payload where t.Key == nameof(TokenClaimTypeEnumPat.NikeName) select t.Value).FirstOrDefault() ?? "";
        //                //var loginTime = (from t in payload where t.Key == nameof(TokenClaimTypeEnumPat.NikeName) select t.Value).FirstOrDefault() ?? "";


        //                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5ypehTWQ5KQ82#cKzU5La37@rHnOCK#sfBs9XshTXmgZtP2EZ*tvTPxsR6rp@Jt%RXmsbUKQ0lo28NN&BrLsAuD%dLjj1p"));
        //                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //                var claims = new Claim[] {
        //                    new Claim(nameof(TokenClaimTypeEnum.UserId),userId.ToString()),
        //                    new Claim(nameof(TokenClaimTypeEnum.RoleId),roleId.ToString()),
        //                    new Claim(nameof(TokenClaimTypeEnum.SystemCode),headImgUrl.ToString()),
        //                    new Claim(nameof(TokenClaimTypeEnumPat.Country),country.ToString()),
        //                    new Claim(nameof(TokenClaimTypeEnumPat.City),city.ToString()),
        //                    //new Claim(nameof(TokenClaimTypeEnumPat.OpenId),openId?.ToString()),
        //                    new Claim(nameof(TokenClaimTypeEnumPat.Province),province.ToString()),
        //                    new Claim(nameof(TokenClaimTypeEnumPat.LoginTime),loginTime.ToString())
        //                    };
        //                identity.AddClaims(claims);
        //                principal = new ClaimsPrincipal(identity);

        //            }
        //        }
        //        return principal;
        //        //}
        //        #endregion
        //        return new ClaimsPrincipal();
        //    }
        //    catch
        //    {
        //        validatedToken = null;
        //        return new ClaimsPrincipal();
        //    }

        //}

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }
        /// <summary>
        /// 此处，如果需要，则需要配置，目前没有token验证
        /// </summary>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <param name="validatedToken"></param>
        /// <returns></returns>
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ClaimsPrincipal principal;
            try
            {
                //Logger.Info("来了老弟，验证吧");
                validatedToken = null;
                var token = new JwtSecurityToken(securityToken);

                //获取到Token的一切信息
                //new Claim(nameof(ClaimTypeEnum.Account), user.Account),
                //    new Claim(nameof(ClaimTypeEnum.RoleId), roleId),
                //    new Claim(nameof(ClaimTypeEnum.UserId), user.UserId)
                var payload = token.Payload;
               // var roleId = (from t in payload where t.Key == nameof(TokenClaimTypeEnum.RoleId) select t.Value).FirstOrDefault() ?? "";
                var userId = (from t in payload where t.Key == nameof(TokenClaimTypeEnum.UserId) select t.Value).FirstOrDefault() ?? "";
                var expObj = (from t in payload where t.Key == "exp" select t.Value).FirstOrDefault();
                long exp;
                if (long.TryParse(expObj?.ToString(), out exp))
                {
                    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    if (exp <= now)
                    {
                        return new ClaimsPrincipal();
                    }
                }
                else
                {
                    return new ClaimsPrincipal();
                }
                var projectInfoId = (from t in payload where t.Key == ClaimTypeClasses.ProjectInfoId select t.Value).FirstOrDefault() ?? "";
                //var email = (from t in payload where t.Key == nameof(ClaimTypeEnum.Email) select t.Value).FirstOrDefault();
                //var systemCode = (from t in payload where t.Key == nameof(TokenClaimTypeEnum.SystemCode) select t.Value).FirstOrDefault() ?? "";
                var issuer = token.Issuer;
                var key = token.SecurityKey;
                var audience = token.Audiences;
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(nameof(TokenClaimTypeEnum.UserId), userId?.ToString()));
                //identity.AddClaim(new Claim(nameof(TokenClaimTypeEnum.RoleId), roleId?.ToString()));
                //identity.AddClaim(new Claim(nameof(TokenClaimTypeEnum.SystemCode), systemCode?.ToString()));
                identity.AddClaim(new Claim(ClaimTypeClasses.ProjectInfoId, projectInfoId?.ToString()));
                //identity.AddClaim(new Claim(ClaimTypes.Role,))


                principal = new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                validatedToken = null;
                principal = null;
            }
            return principal;
        }
    }
}
