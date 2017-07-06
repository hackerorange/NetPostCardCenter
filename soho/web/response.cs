using System.Collections.Generic;

namespace soho.web
{
    public class StandardResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string key { get; set; }
    }

    public abstract class AbstractResponse : StandardResponse
    {
    }

    public class BodyResponse<T> : AbstractResponse
    {
        public T body { get; set; }
    }

    public class PageResponse<T> : AbstractResponse
    {
        public List<T> page { get; set; }
    }
}