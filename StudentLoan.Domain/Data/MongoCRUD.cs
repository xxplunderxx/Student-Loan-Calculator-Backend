using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLoan.Domain.Data
{
    public class MongoCRUD
    {
        private IMongoDatabase db;

        // creates a connection to the db
        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        // inserts a record into the db
        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        // gets all records from the database
        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        // gets the records specifically for one user
        public T LoadRecordByusername<T>(string table, string username)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("username", username);

            return collection.Find(filter).First();
        }

        // inserts user if one does not exist otherwise it replaces that user
        public void UpsertRecord<T>(string table, string user, T record)
        {
            var collection = db.GetCollection<T>(table);

            var result = collection.ReplaceOne(
                new BsonDocument("username", user),
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        // deletes a user with a specified username
        public void DeleteRecord<T>(string table, string username)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("username", username);
            collection.DeleteOne(filter);
        }
    }
}
