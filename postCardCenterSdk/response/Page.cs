using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.response
{
    class Page<T>
    {
        /// <summary>
        /// 详细数据
        /// </summary>
        [JsonProperty("page")]        
        public List<T> Detail { get; set; }
        /// <summary>
        /// 每一页的大小
        /// </summary>
        [JsonProperty("pageSize")]
        public long PageSize { get; set; }
        /// <summary>
        /// 数据的总数
        /// </summary>
        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        [JsonProperty("pageCount")]
        public long PageCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
    }
}
