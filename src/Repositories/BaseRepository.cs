using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T: class
    {
        protected BlogContext db;
        private PropertyInfo IdPropertyInfo;
        public BaseRepository(BlogContext db) {
            this.db = db;
            this.InitIdProperty();
        }

        public virtual IQueryable<T> Get() {
            return this.db.Set<T>().AsQueryable();
        }

        public async virtual Task<T> Get(int id) {
            return await this.db.Set<T>().FindAsync(id);
        }
        public async virtual Task<int> Count() {
            return await this.db.Set<T>().CountAsync();
        }
        public async virtual Task<T> Create(T entity) {
            var savedEntity = this.db.Set<T>().Add(entity);
            await this.db.SaveChangesAsync();
            return savedEntity.Entity;
        }

        public async virtual Task Update(T entity) {
            this.db.Entry(entity).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }

        public async virtual Task Delete(T entity) {
            this.db.Set<T>().Remove(entity);
            await this.db.SaveChangesAsync();
        }

        private void InitIdProperty() {
            var type = typeof(T);
            var propertyIdName = type.Name + "Id";
            this.IdPropertyInfo = typeof(T).GetProperties().FirstOrDefault(prop => prop.Name == propertyIdName);
        }
        
        private bool CompareId(T entity, int id) {
            var entityId = (int)IdPropertyInfo.GetValue(entity);
            return entityId == id;
        }
        
    }
}
