using MongoDB.Bson;
using MongoDB.Driver;
using Syntecxhub_User_Management_System.Models;

namespace Syntecxhub_User_Management_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _user;

        public UserRepository(IMongoDatabase database)
        {
            _user = database.GetCollection<User>("Users");
        }

        public async Task<ObjectId> Create(User user)
        {
            await _user.InsertOneAsync(user);
            return user.Id;
        }

        public async Task Delete(ObjectId objectId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            await _user.DeleteOneAsync(filter);
        }

        public Task<User> Get(ObjectId objectId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var user =  _user.Find(filter).FirstOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _user.Find(_ => true).ToListAsync();
        }

        public async Task Update(ObjectId objectId, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            await _user.ReplaceOneAsync(filter, user);

        }
    }
}
