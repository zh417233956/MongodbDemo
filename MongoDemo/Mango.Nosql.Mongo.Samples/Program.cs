using Mango.Nosql.Mongo.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Mango.Nosql.Mongo.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "mongodb://192.168.50.138:27020";
            var mongoRepository = new MongoRepository(url);

            //var log = new Logs
            //{
            //    Project = "misapi2018",
            //    HostId = "192.168.4.144:8008",
            //    LogName = "CatchErrorLog",
            //    Level = Level.Debug,
            //    UserId = 58988,
            //    Url = "http://misapi2018ali.517api.cn:8110/api/House/GetHouse_List_V1",
            //    RawUrl = "/api/House/GetHouse_List_V1",
            //    UrlReferrer = "",
            //    IP = "175.161.71.161, 111.202.96.71",
            //    OtherMsg = new Dictionary<string, string>(){
            //        { "content-type", "application/json; charset=utf-8"},
            //        { "method", "post"},
            //    },
            //    Msg = "请求通道在等待 00:00:59.9990335 以后答复时超时。增加传递给请求调用的超时值，或者增加绑定上的 SendTimeout 值。分配给此操作的时间可能已经是更长超时的一部分"
            //};
            //for (int i = 0; i < 100; i++)
            //{                
            //    var addresult = mongoRepository.Add(log);
            //    log.Id = Guid.NewGuid().ToString("N");
            //    log.UserId += 1;
            //    log.CreateTime = DateTime.Now;
            //}

            //Console.WriteLine("插入成功，开始查询:");

            //var getResult = mongoRepository.ToList<Logs>(w => w.LogName == "CatchErrorLog", 10);

            var getResult = mongoRepository.PageList<Logs>(w => w.LogName == "CatchErrorLog", s => s.Desc(b => b.CreateTime), 1, 10);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(getResult));
            Console.ReadKey();
        }
    }

    [Mongo("test", "mongologs")]
    public class Logs : MongoEntity
    {
        public Logs()
        {
            Exception = new Exception();
            OtherMsg = new Dictionary<string, string>();
            CreateTime = DateTime.Now;
        }
        public string Project { get; set; }
        public string HostId { get; set; }
        public Level Level { get; set; }
        public string LogName { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public string RawUrl { get; set; }
        public string UrlReferrer { get; set; }
        public string IP { get; set; }
        public Exception Exception { get; set; }
        public string Msg { get; set; }
        public IDictionary<string, string> OtherMsg { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

    }
    /// <summary>
    /// DEBUG|INFO|WARN|ERROR|FATAL
    /// </summary>
    public enum Level
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
