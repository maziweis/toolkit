using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResourceToolkit
{
    public class newWebClient : WebClient
    {
        int Timeout = 50000;
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.Timeout = 1000 * Timeout;
            request.ReadWriteTimeout = 1000 * Timeout;
            return request;
        }
    }

    public class TempModel
    {
        /// <summary>
        /// 资源Model
        /// </summary>
        public class Resource
        {
            /// <summary>
            /// 
            /// </summary>
            public Guid? ID { get; set; }

            /// <summary>
            /// 学段
            /// </summary>
            public int? SchoolStage { get; set; }

            /// <summary>
            /// 年份
            /// </summary>
            public int? Year { get; set; }

            /// <summary>
            /// 年级
            /// </summary>
            public int? Grade { get; set; }

            /// <summary>
            /// 学科
            /// </summary>
            public int? Subject { get; set; }

            /// <summary>
            /// 版本
            /// </summary>
            public int? Edition { get; set; }

            /// <summary>
            /// 册别
            /// </summary>
            public int? BookReel { get; set; }

            /// <summary>
            /// 目录
            /// </summary>
            public int? Catalog { get; set; }

            /// <summary>
            /// 资源类型[下拉选项1] 
            /// </summary>
            public int? ResourceType { get; set; }

            /// <summary>
            /// 资源级别
            /// </summary>
            public int? ResourceLevel { get; set; }

            /// <summary>
            /// 教学步骤--教学环节
            /// </summary>
            public int? TeachingStep { get; set; }

            /// <summary>
            /// 教学模块
            /// </summary>
            public int? TeachingModules { get; set; }

            /// <summary>
            /// 教学形式
            /// </summary>
            public int? TeachingStyle { get; set; }

            /// <summary>
            /// 适用对象
            /// </summary>
            public int? Applicable { get; set; }

            /// <summary>
            /// 是否被审核过。0为否，1为是
            /// </summary>
            public int? IsVerify { get; set; }

            /// <summary>
            /// 教材名称
            /// </summary>
            public int? StandardBook { get; set; }

            /// <summary>
            /// 是否删除了默认为0,1代表已经删除
            /// </summary>
            public int? IsDelete { get; set; }

            /// <summary>
            /// 下载次数
            /// </summary>
            public int? DownCounts { get; set; }

            /// <summary>
            /// 浏览数
            /// </summary>
            public int? ScanCounts { get; set; }

            /// <summary>
            /// 文件大小
            /// </summary>
            public string ResourceSize { get; set; }

            /// <summary>
            /// 省份
            /// </summary>
            public string ProvinceName { get; set; }

            /// <summary>
            /// 是否推荐
            /// </summary>
            public int? IsRecommend { get; set; }

            /// <summary>
            /// 访问权限
            /// </summary>
            public int? Purview { get; set; }

            /// <summary>
            /// 上传时间
            /// </summary>
            public DateTime? UploadDate { get; set; }

            /// <summary>
            /// 上传用户
            /// </summary>
            public string UploadUser { get; set; }

            /// <summary>
            /// 文件类型
            /// </summary>
            public string FileType { get; set; }

            /// <summary>
            /// 缩略图路径---URL地址
            /// </summary>
            public string BreviaryImgUrl { get; set; }

            /// <summary>
            /// 素材ID
            /// </summary>
            public Guid? MaterialID { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public double? Number { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 资源类型—2级
            /// </summary>
            public int? ResourceStyle { get; set; }

            /// <summary>
            /// 关键字
            /// </summary>
            public string KeyWords { get; set; }

            /// <summary>
            /// 文件ID外键
            /// </summary>
            public Guid? FileID { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 来源
            /// </summary>
            public string ComeFrom { get; set; }

            /// <summary>
            /// 上传返回的文件名
            /// </summary>
            public string FileReturnName { get; set; }

            /// <summary>
            /// 评价量
            /// </summary>
            public int? AppraisedCounts { get; set; }

            /// <summary>
            /// 收藏量
            /// </summary>
            public int? CollectCounts { get; set; }

            /// <summary>
            /// 版权信息
            /// </summary>
            public int Copyright { get; set; }

            /// <summary>
            /// 版权信息
            /// </summary>
            public string CopyrightName { get; set; }

            /// <summary>
            /// 课例类型
            /// </summary>
            public int LessonType { get; set; }

            /// <summary>
            /// 判断是否有返回数据
            /// </summary>
            public bool IsSuccess { set; get; }

            /// <summary>
            /// 排序字段
            /// </summary>
            public int Sort { set; get; }

            public int TimeSpan { get; set; }
            /// <summary>
            /// 资源大类
            /// </summary>
            public int ResourceClass { get; set; }
        }


        public class ErrorClass
        {
            /// <summary>
            /// 错误页签
            /// </summary>
            public string Sheet { get; set; }
            /// <summary>
            /// 错误序号
            /// </summary>
            public int? No { get; set; }

            /// <summary>
            /// 错误名称
            /// </summary>
            public string NameError { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrorMsg { get; set; }
        }
    }
}
