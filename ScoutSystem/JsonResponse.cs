using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutSystem
{
    public class JsonResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public JsonResponse(bool Success, object Data)
        {
            this.Success = Success;
            this.Data = Data;
        }
        public JsonResponse(bool Success, string Message)
        {
            this.Success = Success;
            this.Message = Message;
        }
        public JsonResponse(bool Success)
        {
            this.Success = Success;
        }
    }
}