using System.ComponentModel;

namespace ShopBridge.Models
{
    public class ResJsonOutput
    {
        public ResJsonOutput()
        {
            Status = new Status();
        }
        public object Data { get; set; }
        public Status Status { get; set; }
    }

    public class Status
    {
        [DefaultValue(false)]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }

    public class KeyValue
    {
        public KeyValue() { }

        public KeyValue(string _Key, string _Value)
        {
            Key = _Key;
            Value = _Value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class RequestById
    {
        public long ProductId { get; set; }
    }
}
