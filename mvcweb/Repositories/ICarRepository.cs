using System;
using System.Collections.Generic;
using mvcweb.Models;

namespace mvcweb.Repositories
{
    public interface ICarRepository
    {
        Dictionary<string, object> GetAll(int pageNum, int pageSize);
        Dictionary<string, object> GetFilterBy(int find, string keyword, int pageNum, int pageSize);
        Car Get(int id);
        void Add(Car car);
        void Delete(int id);
        void Edit(Car car);
        string GetItemMessage(int find, string keyword, int pageNum, int pageSize);
    }
}