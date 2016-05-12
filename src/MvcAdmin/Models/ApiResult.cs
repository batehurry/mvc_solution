using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin.Models
{
    public class ApiResult : ApiResult<object>
    {
        public ApiResult(ApiResultType status = ApiResultType.OK, string message = null, object data = null, ApiResultPage page = null) : base(status, message, data, page) { }

        /// <summary>
        /// 创建ApiResult
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <param name="page">分页信息</param>
        /// <returns></returns>
        public static ApiResult Create(ApiResultType status = ApiResultType.OK, string message = null, object data = null, ApiResultPage page = null) => new ApiResult(status, message, data, page);
    }

    public class ApiResult<T> : ActionResult
    {
        /// <summary>
        /// 是否成功(此属性忽略序列化)
        /// </summary>
        [JsonIgnore]
        public bool OK => this.Status == ApiResultType.OK;

        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty("status")]
        public ApiResultType Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }

        /// <summary>
        /// 数据/数据对象
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        [JsonProperty("page")]
        public ApiResultPage Page { get; set; }

        protected JsonSerializerSettings SerializerSettings;

        public ApiResult(ApiResultType status = ApiResultType.OK, string message = null, T data = default(T), ApiResultPage page = null)
        {
            this.Status = status;
            this.Message = message;
            this.Data = data;
            this.Page = page;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            InitSerialization(context);
        }

        protected void InitSerialization(ControllerContext context)
        {
            //if (Data == null)
            //{
            //    new EmptyResult().ExecuteResult(context);
            //    return;
            //}
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/html";
            if (SerializerSettings == null)
            {
                SetSerializerSettings();
            }
            response.Write(JsonConvert.SerializeObject(this, Formatting.None, SerializerSettings));
        }

        protected virtual void SetSerializerSettings()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                //空值的属性不序列化
                NullValueHandling = NullValueHandling.Ignore,
                //Json 中存在的属性，实体中不存在的属性不反序列化
                //MissingMemberHandling = MissingMemberHandling.Ignore,

                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd hh:mm" }
                }
            };
        }
    }

    /// <summary>
    /// 分页信息
    /// </summary>
    public class ApiResultPage
    {
        /// <summary>
        /// 初始化分页信息
        /// </summary>
        /// <param name="index">当前页码：默认1</param>
        /// <param name="size">页面大小：默认10</param>
        public ApiResultPage(int index = 1, int size = 10)
        {
            this.Index = index;
            this.Size = size;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("index")]
        public int Index;

        /// <summary>
        /// 每一页大小
        /// </summary>
        [JsonProperty("size")]
        public int Size;

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty("count")]
        public int Count;

        /// <summary>
        /// 总数量
        /// </summary>
        [JsonProperty("total")]
        public int Total;
    }

    public enum ApiResultType
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        None = 0,

        /// <summary>
        /// 成功
        /// </summary>
        OK = 1,

        /// <summary>
        /// 失败
        /// </summary>
        FAIL = -1,

        /*------------------------------------------- 接口拦截 -------------------------------------------*/

        /// <summary>
        /// 非法请求
        /// </summary>
        IllegalRequest = -1000,

        /*------------------------------------------- 登录验证 -------------------------------------------*/

        /// <summary>
        /// 未登录
        /// </summary>
        NoLogin = -2000,

        /*------------------------------------------- 签名验证 -------------------------------------------*/

        /// <summary>
        /// 签名验证失败
        /// </summary>
        SignVerifyFailed = -3000,

        /*------------------------------------------- 授权验证 -------------------------------------------*/

        /// <summary>
        /// 接口未授权
        /// </summary>
        NoAuthorize = -4000
    }
}