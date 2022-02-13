using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopbridge_base.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Shopbridge_Context _dbcontext;
        private readonly DbSet<T> _entities;

        public Repository(Shopbridge_Context dbcontext)
        {
            _dbcontext = dbcontext;
            _entities = dbcontext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            try
            {
                return _entities.AsEnumerable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                await _entities.AddAsync(entity);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task Insert(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                await _entities.AddRangeAsync(entities);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async virtual Task Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            try
            {
                await this._dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                _entities.Remove(entity);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return _entities;
            }
        }


        public List<T> ExecuteStoredProc(string sql, params object[] parameters)
        {
            try
            {
                var sqlString = new StringBuilder();
                sqlString.Append(sql);
                if (parameters != null && parameters.Length > 0)
                {
                    for (int i = 0; i <= parameters.Length - 1; i++)
                    {
                        var p = parameters[i] as DbParameter;
                        if (p == null)
                            throw new Exception("Not support parameter type");

                        sqlString.Append(i == 0 ? " " : ", ");
                        sqlString.Append("@" + p.ParameterName);
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        {
                            //output parameter
                            sqlString.Append(" output");

                        }
                    }
                }
                var result = _entities.FromSqlRaw(sqlString.ToString(), parameters).ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
