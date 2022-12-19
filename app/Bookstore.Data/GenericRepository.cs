﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bookstore.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        protected DbContext context;
        protected DbSet<TModel> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TModel>();
        }

        public async Task AddAsync(TModel entity)
        {
            await context.Set<TModel>().AddAsync(entity);
        }

        public void AddOrUpdate(TModel entity)
        {
            context.Set<TModel>().Update(entity);
        }

        public TModel Get(int id)
        {
            return context.Set<TModel>().Find(id);
        }

        public IEnumerable<TModel> GetAll(string includeProperties = "")
        {
            IQueryable<TModel> query = context.Set<TModel>();

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public void Remove(TModel entity)
        {
            context.Set<TModel>().Remove(entity);
        }

        public void Update(TModel entity)
        {
            context.Update(entity);
        }

        public IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            IQueryable<TModel> query = dbSet;

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public PaginatedList<TModel> GetPaginated(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null, int pageIndex = 1, int pageSize = 10,
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            var filters = new List<Expression<Func<TModel, bool>>>();

            if (filter != null)
            {
                filters.Add(filter);
            }

            return GetPaginated(filters, orderBy, pageIndex, pageSize, includeProperties);
        }

        public PaginatedList<TModel> GetPaginated(List<Expression<Func<TModel, bool>>> filters,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null, int pageIndex = 1, int pageSize = 10,
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            IQueryable<TModel> query = dbSet;

            filters.ForEach(x => query = query.Where(x));

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return PaginatedList<TModel>.Create(orderBy(query), pageIndex, pageSize);
            }

            return PaginatedList<TModel>.Create(query, pageIndex, pageSize);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}