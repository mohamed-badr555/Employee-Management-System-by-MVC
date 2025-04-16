﻿using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T :ModelBase
    {
        private protected readonly AppDbContext _dbContext;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            //_dbContext.Set<T>().Add(entity);
            _dbContext.Add(entity);//EF  3.1 
            //return _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
            //return _dbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            _dbContext.Update(entity);
            //return _dbContext.SaveChanges();
        }
        public T Get(int id)
        {
            return _dbContext.Find<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
            else
            return  _dbContext.Set<T>().AsNoTracking().ToList();
        }

     
    }
}
