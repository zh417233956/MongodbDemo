using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = "test";
            var collection = "dotnettest";
            var db = new MongoClient("mongodb://192.168.50.138:27020").GetDatabase(database);
            var coll = db.GetCollection<TestMongo>(collection);

            var entity = new TestMongo
            {
                Name = "Mr.Right",
                Amount = 100,
                CreateDateTime = DateTime.Now
            };

            //coll.InsertOneAsync(entity).ConfigureAwait(false);
            //coll.InsertOne(entity);
            
            var list = coll.Find(m => 1 == 1).ToList();
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            Console.WriteLine("操作成功");
            Console.ReadKey();

        }
    }

    public class TestMongo : MongoEntity
    {

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public decimal Amount { get; set; }

        public string Name { get; set; }

    }
    public abstract class MongoEntity
    {
        private string _id;
        protected MongoEntity()
        {
            _id = Guid.NewGuid().ToString("N");
        }

        [BsonElement("_id")]
        public string Id
        {
            set => _id = value;
            get
            {
                _id = _id ?? Guid.NewGuid().ToString("N");
                return _id;
            }
        }
    }
}
