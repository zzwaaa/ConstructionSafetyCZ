using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models
{
    /// <summary>
    /// 用户基本信息表
    /// </summary>
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(32)]
        public string UserInfoId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [MaxLength(100)]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 账号状态：1正常使用，0无法正常使用--智慧工地管理平台状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(11)]
        public string Telephone { get; set; }

        /// <summary>
        /// 最后登录的ip
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登录的时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        [MaxLength(32)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 商户编码-所在企业的社会信用统一编码
        /// </summary>
        [MaxLength(50)]
        public string MerchantCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int? Deleted { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        [MaxLength(32)]
        public string RoleId { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string VerifyReason { get; set; }
        /// <summary>
        /// 统一社会信用代码证路径
        /// </summary>
        public string TradeCodeUrl { get; set; }

        ///// <summary>
        ///// 区属编号
        ///// </summary>
        //public string BelongedTo { get; set; }
        ///// <summary>
        ///// 备案号
        ///// </summary>
        //public string RecordNumber { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        public string ProjectInfoId { get; set; }//项目唯一Id

        /// <summary>
        /// 用户类型 0:项目经理，1:安全员，2:资料员，3:项目总监  -1:其他
        /// </summary>
        public int? UserType { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string APIUrl { get; set; }

        /// <summary>
        /// 模板id，1，2，3，4，5，6，7
        /// </summary>
        //public int? TemplateId { get; set; }

        /// <summary>
        /// 账号状态：1正常使用，0无法正常使用 -- 智安通app状态
        /// </summary>
        public int? SmartStatus { get; set; }
        /// <summary>
        /// qq
        /// </summary>
        public string QQUnionId { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChatUnionId { get; set; }

        /// <summary>
        /// 账号类型 0：项目账号 1企业端账号
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// 智慧工地模板
        /// </summary>
        public int IsTemplate { get; set; }



        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 所属的企业端的后台角色：只有AccountType=1表示企业端账号
        /// </summary>
        [StringLength(40)]
        public string BackstageRoleId { get; set; }
    }
}
