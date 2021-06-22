using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models
{
    public class ProjectInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 监督备案号
        /// </summary>
        public string RecordNumber { get; set; }
        /// <summary>
        /// 区属编号
        /// </summary>
        public string BelongedTo { get; set; }
        public string ProjectInfoId { get; set; }//项目唯一Id
        /// <summary>
        /// 项目开始时间
        /// </summary>
        public DateTime? ProjectStartDateTimne { get; set; }
        /// <summary>
        /// 项目结束时间
        /// </summary>
        public DateTime? ProjectEndDateTimne { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目地址
        /// </summary>
        public string ProjectAddress { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string LongitudeCoordinate { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string LatitudeCoordinate { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectLeader { get; set; }
        /// <summary>
        /// 附近国控点名字
        /// </summary>
        public string NearPotinName { get; set; }
        /// <summary>
        /// 国控点Id
        /// </summary>
        public int? PotinId { get; set; }
        /// <summary>
        /// 安监图片url
        /// </summary>
        public string ProImgUrl { get; set; }
        /// <summary>
        /// 安监地址
        /// </summary>
        public string PlatUrl { get; set; }

        /// <summary>
        /// 监督机构
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 工程类别
        /// </summary>
        public string ProjectCategory { get; set; }

        /// <summary>
        /// 备案通过时间
        /// </summary>
        public DateTime? RecordDate { get; set; }

        /// <summary>
        /// 项目面积
        /// </summary>
        public decimal ProjectArea { get; set; }

        /// <summary>
        /// 项目造价
        /// </summary>
        //[Column("ProjectCost", TypeName = "MONEY")]
        public decimal ProjectCost { get; set; }
        /// <summary>
        /// 城市名称（简短）
        /// </summary>
        public string CityShortName { get; set; }

        /// <summary>
        /// 项目简称
        /// </summary>
        public string ProjectAbbreviation { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 全景图地址
        /// </summary>
        public string PanoramagramUrl { get; set; }

        /// <summary>
        /// 智慧工地批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 是否是智慧工地
        /// </summary>
        public int IsSmartSite { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeleteMark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        public string ProjectUrl { get; set; }
        public string VideoUrl { get; set; }
        /// <summary>
        /// 是否可以添加绑定省系统
        /// 0：不可以，1：可以
        /// </summary>
        public int IsCanConnect { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 项目区域
        /// </summary>
        public string Region { get; set; }
    }
}
