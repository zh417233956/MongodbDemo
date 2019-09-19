﻿using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mango.Nosql.Mongo.Base
{
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
