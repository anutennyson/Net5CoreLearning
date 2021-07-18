using Catalog_Final.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog_Final.Repositories
{
    //This repository is for handling all the action with Mongo db
    //We are going to implement same interface that way we will have every methods that are required
    //for this function to work
    public class MongoDBItemsRepository : IItemsRepository
    {
        //we need to use MongoDB client and we have to inject that depedency into our solution
        //All of your documents will be grouped into a collections and you can have one or more collections
        //in your databases

        private const string databaseName = "catalog";
        private const string collectionName = "items";

        //Below line is saying that we have a collection of item documents
        private readonly IMongoCollection<Items> itemsCollections;
        private readonly FilterDefinitionBuilder<Items> filterBuilder = Builders<Items>.Filter;

        public MongoDBItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollections = database.GetCollection<Items>(collectionName);
        }

        //Sync Starts
        //public void CreateItem(Items item)
        //{
        //    itemsCollections.InsertOne(item);
        //}
        //SynchEnds
        //Async Starts
        public async Task CreateItemAsync(Items item)
        {
            await itemsCollections.InsertOneAsync(item);
        }
        //Async Ends

        //Sync Starts
        //public void DeleteItem(Guid Id)
        //{
        //    var filter = filterBuilder.Eq(item => item.Id, Id);
        //    itemsCollections.DeleteOne(filter);
        //}

        //public Items GetItem(Guid id)
        //{
        //    var filter = filterBuilder.Eq(item => item.Id, id);
        //    return itemsCollections.Find(filter).SingleOrDefault();
        //}

        //public IEnumerable<Items> GetItems()
        //{
        //    return itemsCollections.Find(new BsonDocument()).ToList();
        //}

        //public void UpdateItem(Items item)
        //{
        //    var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
        //    itemsCollections.ReplaceOne(filter, item);
        //}
        //Sync Ends

        //Async Starts
        public async Task DeleteItemAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(item => item.Id, Id);
            await itemsCollections.DeleteOneAsync(filter);
        }

        public async Task<Items> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await itemsCollections.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Items>> GetItemsAsync()
        {
            return await itemsCollections.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Items item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollections.ReplaceOneAsync(filter, item);
        }
        //Async Ends
    }
}
