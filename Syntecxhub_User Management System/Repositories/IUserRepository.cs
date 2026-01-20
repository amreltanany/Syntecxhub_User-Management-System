using Syntecxhub_User_Management_System.Models;
using MongoDB.Bson;
namespace Syntecxhub_User_Management_System.Repositories
{
    public interface IUserRepository
    {
        Task<ObjectId> Create(User user);
        Task<User> Get(ObjectId objectId);
        Task<IEnumerable<User>> GetAll();
        Task Update(ObjectId objectId, User user);
        Task Delete(ObjectId objectId);

    }
}
