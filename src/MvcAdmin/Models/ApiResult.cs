using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcAdmin.Models
{
    public class ApiResult<T> : ActionResult
    {
        public ServerStatus code = ServerStatus.Fail ;
        public string msg = "调用失败";
        public T data;

        protected JsonSerializerSettings SerializerSettings;
        public override void ExecuteResult(ControllerContext context)
        {
            InitSerialization(context);
        }

        protected void InitSerialization(ControllerContext context)
        {
            if (data == null)
            {
                new EmptyResult().ExecuteResult(context);
                return;
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/html";//application/json
            if (SerializerSettings == null)
            {
                SetSerializerSettings();
            }
            response.Write(JsonConvert.SerializeObject(data, Formatting.None, SerializerSettings));
        }

        protected virtual void SetSerializerSettings()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd hh:mm" }
                }
            };
        }
    }
}