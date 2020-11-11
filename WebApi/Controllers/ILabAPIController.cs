using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// ILabAPI
    /// </summary>
    [Route("[controller]")]
    public class ILabAPIController : ApiController
    {
        public ILabX labX = new ILabBusiness();
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public object Login(UserModel model)
        {
            if (string.IsNullOrEmpty(model.username))
                return ResultModel.FAIL(3, "username缺失");
            if (string.IsNullOrEmpty(model.password))
                return ResultModel.FAIL(3, "password缺失");
            return labX.UserValidate(model);
        }
        /// <summary>
        /// 获取实验状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public object ResultUpload(ResultUploadModel model)
        {
            if (string.IsNullOrEmpty(model.username))
                return ResultModel.FAIL(2, "username缺失");
            //if (string.IsNullOrEmpty(model.issuerId))
                //return ResultModel.FAIL(2, "issuerId缺失");
            return labX.ResultUpload(model);
        }
        /// <summary>
        /// 回传实验结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public object ProjectLogUpload(ProjectLogUploadModel model)
        {
            if (string.IsNullOrEmpty(model.username))
                return ResultModel.FAIL(1, "username缺失");
            if (string.IsNullOrEmpty(model.projectTitle))
                return ResultModel.FAIL(1, "实验名称缺失");
            if (!model.status.HasValue)
                return ResultModel.FAIL(1, "实验状态缺失");
            if (!model.score.HasValue)
                return ResultModel.FAIL(1, "实验成绩缺失");
            if (!model.startDate.HasValue)
                return ResultModel.FAIL(1, "实验开始时间缺失");
            if (!model.endDate.HasValue)
                return ResultModel.FAIL(1, "实验结束时间缺失");
            if (!model.timeUsed.HasValue)
                return ResultModel.FAIL(1, "实验用时缺失");
            //if (!model.issuerId.HasValue)
            //    return ResultModel.FAIL(1, "平台接入编号缺失");

            return labX.ProjectLogUpload(model);
        }
        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public object ProjectLogAttachmentUpload(ProjectLogAttachmentUploadModel model)
        {
            if (string.IsNullOrEmpty(model.filename))
                return ResultModel.FAIL(1, "文件名缺失");
            
            return labX.ProjectLogAttachmentUpload(model);
        }
    }
}
