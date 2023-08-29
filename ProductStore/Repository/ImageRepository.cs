using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext _dataContext;
        public ImageRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public bool AddImage(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteImage(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateImage(UserDTO user)
        {
                _dataContext.Entry(user).State = EntityState.Modified;
                return Save();
        }
    }
}
