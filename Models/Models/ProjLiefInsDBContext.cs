using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class ProjLiefInsDBContext: DbContext
    {
        public ProjLiefInsDBContext()
        {
        }

        public ProjLiefInsDBContext(DbContextOptions<ProjLiefInsDBContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 人保APP用户信息表(APP登陆表)
        /// </summary>
        public virtual DbSet<LifeInUserInfo> LifeInUserInfos { get; set; }

        /// <summary>
        /// 项目 人员信息表
        /// </summary>
        public virtual DbSet<ProjectPoint> ProjectPoints { get; set; }

        /// <summary>
        /// 积分表
        /// </summary>
        public virtual DbSet<IntegralInfo> IntegralInfos { get; set; }

        /// <summary>
        /// 积分记录表
        /// </summary>
        public virtual DbSet<IntegralRecord> IntegralRecords { get; set; }

        /// <summary>
        /// 文件上传表
        /// </summary>
        public virtual DbSet<FileUpload> FileUploads { get; set; }

        /// <summary>
        /// 附件表
        /// </summary>
        public virtual DbSet<FileImg> FileImgs { get; set; }

        /// <summary>
        /// 消息表
        /// </summary>
        public virtual DbSet<PersonNew> PersonNews { get; set; }

        /// <summary>
        /// 二维码信息表
        /// </summary>
        public virtual DbSet<ProjectQRinfo> ProjectQRinfos { get; set; }

        /// <summary>
        /// 单位登陆表(后台登录)
        /// </summary>
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

        /// <summary>
        /// 小游戏安全找茬
        /// </summary>
        public virtual DbSet<Imagelist> Imagelist { get; set; }
    }
}
