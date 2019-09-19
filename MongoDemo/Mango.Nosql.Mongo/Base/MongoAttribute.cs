using System;

namespace Mango.Nosql.Mongo.Base
{
    #region Mongo实体标签
    /// <inheritdoc />
    /// <summary>
    /// Mongo实体标签
    /// </summary>
    public class MongoAttribute : Attribute
    {
        public MongoAttribute(string database, string collection)
        {
            Database = database;
            Collection = collection;
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// 集合名称
        /// </summary>
        public string Collection { get; }

    }
    #endregion
}
