using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
    public class BindViewModel
    {
        //public string BelongedTo { get; set; }

        //public string RecordNumber { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 阙值
        /// </summary>
        public string Limit { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectInfoId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
    }
}
