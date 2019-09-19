using Mango.Nosql.Mongo;
using Mango.Nosql.Mongo.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mango.Nosql.MongoTest
{
    [TestClass]
    public class Test
    {
        [TestClass]
        public class MongoDbTest
        {
            public MongoRepository MongoRepository;
            public MongoDbTest()
            {
                var url = "mongodb://192.168.50.138:27020";
                MongoRepository = new MongoRepository(url);
            }
            #region Add

            [TestMethod]
            public void Add_Normal_IsTrue()
            {
                var log = new Logs
                {
                    Project = "misapi2018",
                    HostId = "192.168.4.144:8008",
                    LogName = "CatchErrorLog",
                    Level = Level.Debug,
                    UserId = 58988,
                    Url = "http://misapi2018ali.517api.cn:8110/api/House/GetHouse_List_V1",
                    RawUrl = "/api/House/GetHouse_List_V1",
                    UrlReferrer = "",
                    IP = "175.161.71.161, 111.202.96.71",
                    OtherMsg = new Dictionary<string, string>(){
                        { "content-type", "application/json; charset=utf-8"},
                        { "method", "post"}
                },
                    Msg = "Add于：" + DateTime.Now.ToString()
                };
                var totalCount1 = MongoRepository.Count<Logs>(w => w.LogName == log.LogName);
                for (int i = 0; i < 100 && totalCount1 == 0; i++)
                {
                    var addresult = MongoRepository.Add(log);
                    log.Id = Guid.NewGuid().ToString("N");
                    log.UserId += 1;
                    log.CreateTime = DateTime.Now;
                }
                var totalCount2 = MongoRepository.Count<Logs>(w => w.LogName == log.LogName);

                Assert.AreEqual(totalCount2, 100);
            }

            #endregion

            #region Update

            [TestMethod]
            public void Update_Normal_IsTrue()
            {
                var log = MongoRepository.Get<Logs>(w => w.UserId == 58988);
                log.Msg = "Update修改于：" + DateTime.Now.ToString();
                var result = MongoRepository.Update(log);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Update_Where_IsTrue()
            {
                var Msg = "Update_Where修改于：" + DateTime.Now.ToString();
                var result = MongoRepository.Update<Logs>(w => w.UserId == 58988, u => new Logs { Msg = Msg });

                Assert.IsTrue(result > 0);
            }

            #endregion

            #region Delete

            [TestMethod]
            public void Delete_Normal_IsTrue()
            {
                var log = new Logs
                {
                    LogName = "OtherLog",
                    Msg = "Delete_log_Add于：" + DateTime.Now.ToString()
                };
                MongoRepository.Add(log);

                var result = MongoRepository.Delete(log);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Delete_Where_IsTrue()
            {
                var log = new Logs
                {
                    LogName = "Other2Log",
                    Msg = "Delete_Where_log_Add于：" + DateTime.Now.ToString()
                };
                MongoRepository.Add(log);

                var result = MongoRepository.Delete<Logs>(a => a.LogName == log.LogName);

                Assert.IsTrue(result > 0);
            }

            #endregion

            #region Get
            [TestMethod]
            public void Get_Normal_IsTrue()
            {
                var mrright = MongoRepository.Get<Logs>(a => true);

                Assert.IsNotNull(mrright);
            }

            [TestMethod]
            public void Get_Selector_IsTrue()
            {
                var logName = MongoRepository.Get<Logs, string>(a => true, a => a.LogName);

                Assert.IsNotNull(logName);
            }

            [TestMethod]
            public void Get_OrderBy_IsTrue()
            {
                var logName = MongoRepository.Get<Logs>(a => true, a => a.Desc(b => b.CreateTime));

                Assert.IsNotNull(logName);
            }

            [TestMethod]
            public void Get_SelectorOrderBy_IsTrue()
            {
                var log = MongoRepository.Get<Logs, Logs>(a => true, a => new Logs { LogName = a.LogName, Msg = a.Msg }, a => a.Desc(b => b.CreateTime));

                Assert.IsNotNull(log.LogName);
            }
            #endregion

            #region ToList
            [TestMethod]
            public void ToList_Normal_IsTrue()
            {
                var log = MongoRepository.ToList<Logs>(a => true);

                Assert.IsTrue(log.Any());
            }

            [TestMethod]
            public void ToList_Top_IsTrue()
            {
                var log = MongoRepository.ToList<Logs>(a => true, 10);

                Assert.AreEqual(log.Count, 10);
            }

            [TestMethod]
            public void ToList_Orderby_IsTrue()
            {
                var log = MongoRepository.ToList<Logs>(a => true, a => a.Desc(b => b.CreateTime).Desc(b => b.UserId), 10);

                Assert.AreEqual(log.Count, 10);
            }

            [TestMethod]
            public void ToList_Selector_IsTrue()
            {
                var log = MongoRepository.ToList<Logs, string>(a => true, a => a.LogName);

                Assert.IsTrue(log.Any());
            }

            [TestMethod]
            public void ToList_Selector_Orderby_IsTrue()
            {
                var log = MongoRepository.ToList<Logs, string>(a => true, a => a.LogName, a => a.Desc(b => b.CreateTime));

                Assert.IsTrue(log.Any());
            }

            [TestMethod]
            public void ToList_Selector_Orderby_Top_IsTrue()
            {
                var log = MongoRepository.ToList<Logs, string>(a => true, a => a.LogName, a => a.Desc(b => b.CreateTime), 100);

                Assert.IsTrue(log.Any());
            }

            #endregion

            #region PageList
            [TestMethod]
            public void PageList_Normal_IsTrue()
            {
                var log = MongoRepository.PageList<Logs>(a => true, 1, 10);

                Assert.AreEqual(log.Items.Count, 10);
                Assert.AreEqual(log.HasNext, true);
                Assert.AreEqual(log.HasPrev, false);
            }

            [TestMethod]
            public void PageList_Orderby_IsTrue()
            {
                var log = MongoRepository.PageList<Logs>(a => true, a => a.Desc(b => b.LogName), 2, 10);

                Assert.AreEqual(log.Items.Count, 10);
                Assert.AreEqual(log.HasNext, true);
                Assert.AreEqual(log.HasPrev, true);
            }

            [TestMethod]
            public void PageList_Selector_IsTrue()
            {
                var log = MongoRepository.PageList<Logs, string>(a => true, a => a.LogName, 2, 10);

                Assert.AreEqual(log.Items.Count, 10);
                Assert.AreEqual(log.HasNext, true);
                Assert.AreEqual(log.HasPrev, true);
            }

            [TestMethod]
            public void PageList_Selector_OrderBy_IsTrue()
            {
                var log = MongoRepository.PageList<Logs, string>(a => true, a => a.LogName, a => a.Desc(b => b.CreateTime), 2, 20);

                Assert.AreEqual(log.Items.Count, 20);
                Assert.AreEqual(log.HasNext, true);
                Assert.AreEqual(log.HasPrev, true);
            }

            #endregion

            #region Exists

            [TestMethod]
            public void Exists_Normal_IsTrue()
            {
                var isExist = MongoRepository.Exists<Logs>(a => a.LogName == "CatchErrorLog");

                Assert.IsTrue(isExist);
            }

            [TestMethod]
            public void Exists_Normal_IsFalse()
            {
                var isExist = MongoRepository.Exists<Logs>(a => a.LogName == "ErrorLogs");

                Assert.IsFalse(isExist);
            }

            #endregion

            #region Others

            [TestMethod]
            public void AddIfNotExist_Normal_IsTrue()
            {
                var totalCount = MongoRepository.Count<Logs>(w => w.LogName == "CatchErrorLog");

                Assert.AreEqual(totalCount, 100);
            }
            #endregion
        }

        [Mongo("unittest", "logs")]
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
}
