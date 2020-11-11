using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProjectLogUploadModel
    {
        /// <summary>
        /// 实验空间用户账号
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 实验名称：需要完全匹配实验空间申报的实验名称
        /// </summary>
        public string projectTitle { get; set; }
        /// <summary>
        /// 子实验名称，适用于一个实验中包含多个子实验项目
        /// </summary>
        public string childProjectTitle { get; set; }
        /// <summary>
        /// 实验状态：1 - 完成；2 - 未完成
        /// </summary>
        public int? status { get; set; }
        /// <summary>
        /// 实验成绩：0 ~100，百分制
        /// </summary>
        public int? score { get; set; }
        /// <summary>
        /// 实验开始时间
        /// </summary>
        public DateTime? startDate { get; set; }
        /// <summary>
        /// 实验结束时间
        /// </summary>
        public DateTime? endDate { get; set; }
        /// <summary>
        /// 实验用时：非零整数，单位分钟
        /// </summary>
        public int? timeUsed { get; set; }
        //public int? issuerId { get; set; }
        /// <summary>
        /// 实验报告（PDF、DOC等）：通过附件上传服务获取到的附件ID
        /// </summary>
        public int? attachmentId { get; set; }
    }
}