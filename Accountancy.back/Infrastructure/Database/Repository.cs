using System.Linq;

namespace Accountancy.Infrastructure.Database
{
    public interface IRepository
    {
        IQueryable<T> Query<T>() where T : class;
        T Get<T>(int id) where T : class, IEntity<int>;
        T GetOrDefault<T>(int id) where T : class, IEntity<int>;
        int AddAndSave<T>(T item) where T : class;
        void Save();
    }

    public class Repository : IRepository
    {
        private readonly DatabaseContext _databaseContext;
        
        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _databaseContext.Set<T>();
        }

        public T Get<T>(int id) where T : class, IEntity<int>
        {
            return Query<T>().Single(x => x.Id == id);
        }

        public T GetOrDefault<T>(int id) where T : class, IEntity<int>
        {
            return Query<T>().SingleOrDefault(x => x.Id == id);
        }

        public int AddAndSave<T>(T item) where T : class
        {
            _databaseContext.Set<T>().Add(item);
            return _databaseContext.SaveChanges();
        }

        public void Save()
        {
            _databaseContext.SaveChanges();
        }
    }
}