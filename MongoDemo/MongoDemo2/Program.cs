using MongoDB.Bson.Serialization.Attributes;
using Sikiro.Nosql.Mongo;
using Sikiro.Nosql.Mongo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDemo2
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "mongodb://192.168.50.138:27020";
            var mongoRepository = new MongoRepository(url);

            var u = new User
            {
                Name = "Mr.right",
                BirthDateTime = new DateTime(1990, 1, 1),
                AddressList = new List<string> { "liaoning", "shenyang" },
                Sex = Sex.Man,
                Son = new User
                {
                    Name = "XiaoMu",
                    BirthDateTime = DateTime.Now
                }
            };

            var addresult = mongoRepository.Add(u);

            mongoRepository.Update<User>(a => a.Id == u.Id, a => new User
            {
                Sex = Sex.Man
            });

            var upResulr = mongoRepository.GetAndUpdate<User>(a => a.Id == u.Id, a => new User { Sex = Sex.Man });

            var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
            getResult.Name = "supermr.right";

            mongoRepository.Update(getResult);



            mongoRepository.Exists<User>(a => a.Id == u.Id);

            mongoRepository.Delete<User>(a => a.Id == u.Id);


        }
    }

    [Mongo("test", "dotnettestuser")]
    public class User : MongoEntity
    {
        public string Name { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDateTime { get; set; }

        public User Son { get; set; }

        public Sex Sex { get; set; }

        public List<string> AddressList { get; set; }
    }

    public enum Sex
    {
        Woman,
        Man
    }
}
