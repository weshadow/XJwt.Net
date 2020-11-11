using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// ILabX封装方法
    /// </summary>
    public interface ILabX
    {
        /// <summary>
        /// 用户验证
        /// </summary>
        /// <returns></returns>
        object UserValidate(UserModel model);
        /// <summary>
        /// 实验状态
        /// </summary>
        /// <returns></returns>
        object ResultUpload(ResultUploadModel model);
        /// <summary>
        /// 回传实验结果
        /// </summary>
        /// <returns></returns>
        object ProjectLogUpload(ProjectLogUploadModel model);
        /// <summary>
        /// 附件上传
        /// </summary>
        /// <returns></returns>
        object ProjectLogAttachmentUpload(ProjectLogAttachmentUploadModel model);
    }
    /// <summary>
    /// 业务封装
    /// </summary>
    public class ILabBusiness : ILabX
    {
        LabAPI labApi = new LabAPI();
        public object ProjectLogAttachmentUpload(ProjectLogAttachmentUploadModel model)
        {
            throw new NotImplementedException();
        }

        public object ProjectLogUpload(ProjectLogUploadModel model)
        {
            //必要参数添加
            var param = new Dictionary<string, object>
            {
                {"username",model.username },
                {"projectTitle",model.projectTitle },
                {"status",model.status },
                {"score",model.score },
                {"startDate",model.startDate.Value.GetTimeStamp() },
                {"endDate",model.endDate.Value.GetTimeStamp() },
                {"timeUsed",model.timeUsed },
                {"issuerId",ILabXConfig.ISSUERID }
            };
            //可选参数添加
            //子项目名称
            if (!string.IsNullOrEmpty(model.childProjectTitle))
            {
                param.Add("childProjectTitle", model.childProjectTitle);
            }
            //上传附件ID
            if (model.attachmentId.HasValue)
            {
                param.Add("attachmentId", model.attachmentId);
            }
            return labApi.Call<string>(CallMethod.POST, LabAPI.API.ProjectLogUpload, param);
        }

        public object ResultUpload(ResultUploadModel model)
        {
            var param = new Dictionary<string, object> {
                {"username",model.username },
                {"issuerId",ILabXConfig.ISSUERID }
            };
            return labApi.Call<string>(CallMethod.POST, LabAPI.API.ResultUpload, param);
        }

        public object UserValidate(UserModel model)
        {
            var nonce = labApi.GenerateNonce;
            var cnonce = labApi.GenerateCNonce;
            //密码加密
            var password = EncryptHelper.SHA256EncryptByte(
                        nonce +
                        EncryptHelper.SHA256EncryptByte(model.password).ToString().ToUpper() +
                        cnonce)
                    .ToString()
                    .ToUpper();
            var param = new Dictionary<string, object> {
                {"username",model.username },
                {"password",password },
                {"nonce",nonce },
                {"cnonce",cnonce}
            };
            return labApi.Call<string>(CallMethod.POST, LabAPI.API.UserValidate, param);
        }
    }
}