﻿using CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CRUD.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly Entities _Context;
        private DbSet<T> table;
        public GenericRepository(Entities Entities)
        {
            _Context = new Entities();
            table = _Context.Set<T>();
        }
        public void Delete(int ID)
        {
            T obj = table.Find(ID);
            table.Remove(obj);
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(int TId)
        {
            return table.Find(TId);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            try
            {
                _Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _Context.Entry(obj).State = EntityState.Modified;
        }
        public void UpdateToDelete(T obj)
        {
            table.Attach(obj);
            _Context.Entry(obj).State = EntityState.Modified;
        }
    }
}