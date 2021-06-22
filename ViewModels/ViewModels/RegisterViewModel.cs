using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
   public class RegisterViewModel
    {
        public int ID { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
    }
}
