using Pt.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Bl.Repository
{
    public class RepositoryBase<T, TId> where T : class, new()
    {
        protected internal static Mycontext dbContext;

        public virtual List<T> GetAll()
        {
            try
            {
                dbContext = new Mycontext();
                return dbContext.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual T GetById(TId id)
        {
            try
            {
                dbContext = new Mycontext();

                return dbContext.Set<T>().Find(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public virtual int Insert(T entity)
        {
            try
            {
                dbContext = dbContext ?? new Mycontext();
                dbContext.Set<T>().Add(entity);
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public virtual int Delete(T entity)
        {
            try
            {
                dbContext = dbContext ?? new Mycontext();
                dbContext.Set<T>().Remove(entity);
                return dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public  virtual int Update()
        {
            try
            {
                dbContext = dbContext ?? new Mycontext();
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
