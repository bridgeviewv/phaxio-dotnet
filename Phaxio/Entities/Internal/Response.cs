﻿using Newtonsoft.Json;

namespace Phaxio.Entities.Internal
{
    public class Response<T>
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        public Result ToResult()
        {
            return new Result { Success = Success, Message = Message };
        }
    }
}
