using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using mvcweb.Models;
using mvcweb.App;

namespace mvcweb.Repositories
{
    public class CarRepository : ICarRepository
    {
        public Dictionary<string, object> GetAll(int pageNum, int pageSize)
        {
            ISession s = NHibernateUtils.CurrentSession;
            int total = GetTotalCars(s);
            Pager pager = new Pager(total, pageNum, pageSize);

            string item_msg = pager.GetItemMessage();

            int lowerbound = pager.LowerBound;
            int hasnext = pager.HasNext ? 1 : 0;
            int hasprev = pager.HasPrev ? 1 : 0;

            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                { "item_msg", item_msg },
                { "hasnext", hasnext },
                { "hasprev", hasprev },
                { "nextpage", pageNum + 1 },
                { "prevpage", pageNum - 1 }
            };

            IList<Car> cars = s.QueryOver<Car>()
                .Skip(lowerbound)
                .Take(pager.PageSize)
                .List();

            dic.Add("list", cars);

            return dic;
        }

        public Dictionary<string, object> GetFilterBy(int find, string keyword, int pageNum, int pageSize)
        {
            ISession s = NHibernateUtils.CurrentSession;
            ICriteria c = s.CreateCriteria<Car>();
            GetFilterCriteria(c, find, keyword);

            int total = c.List().Count;
            Pager pager = new Pager(total, pageNum, pageSize);

            string item_msg = pager.GetItemMessage();

            int lowerbound = pager.LowerBound;
            int hasnext = pager.HasNext ? 1 : 0;
            int hasprev = pager.HasPrev ? 1 : 0;

            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                { "item_msg", item_msg },
                { "hasnext", hasnext },
                { "hasprev", hasprev },
                { "nextpage", pageNum + 1 },
                { "prevpage", pageNum - 1 }
            };

            c.SetFirstResult(lowerbound);
            c.SetMaxResults(pager.PageSize);

            IList<Car> cars = c.List<Car>();

            dic.Add("list", cars);

            return dic;
        }

        public Car Get(int id)
        {
            ISession s = NHibernateUtils.CurrentSession;
            Car car = s.Get<Car>(id);
            return car;
        }

        public void Add(Car car)
        {
            ISession s = NHibernateUtils.CurrentSession;
            s.Save(car);
        }

        public void Delete(int id)
        {
            ISession s = NHibernateUtils.CurrentSession;
            ITransaction tx = s.BeginTransaction();
            Car car = s.Get<Car>(id);
            s.Delete(car);
            if (tx != null && tx.IsActive)
                tx.Commit();
        }

        public void Edit(Car car)
        {
            ISession s = NHibernateUtils.CurrentSession;
            s.Update(car);
        }

        public string GetItemMessage(int find, string keyword, int pageNum, int pageSize)
        {
            ISession s = NHibernateUtils.CurrentSession;
            int total = 0;
            string item_msg = "";

            if (find == 0 && string.IsNullOrEmpty(keyword))
            {
                total = GetTotalCars(s);
                Pager pager = new Pager(total, pageNum, pageSize);
                item_msg = pager.GetItemMessage();
                return item_msg;
            }

            else
            {
                ICriteria c = s.CreateCriteria<Car>();
                GetFilterCriteria(c, find, keyword);
                total = c.List().Count;
                Pager pager = new Pager(total, pageNum, pageSize);
                item_msg = pager.GetItemMessage();
                return item_msg;
            }
        }

        private int GetTotalCars(ISession s)
        {
            int count = s.QueryOver<Car>()
                .ToRowCountQuery()
                .FutureValue<int>().Value;
            return count;
        }

        private void GetFilterCriteria(ICriteria criteria, int find, string keyword)
        {
            string text = string.Format("%{0}%", keyword);

            // Search by make
            if (find == 1)
            {
                ICriterion qmake = Restrictions.Like("Make", text, MatchMode.Anywhere);
                criteria.Add(qmake);
            }

            // Search by model
            else if (find == 2)
            {
                ICriterion qmodel = Restrictions.Like("Model", text, MatchMode.Anywhere);
                criteria.Add(qmodel);
            }

            // Search by year
            else if (find == 3)
            {
                try
                {
                    int year = Convert.ToInt32(keyword);
                    ICriterion qyear = Restrictions.Eq("Year", year);
                    criteria.Add(qyear);
                }

                catch
                {
                }
            }

            // Search by doors
            else if (find == 4)
            {
                try
                {
                    int doors = Convert.ToInt32(keyword);
                    ICriterion qdoors = Restrictions.Eq("Doors", doors);
                    criteria.Add(qdoors);
                }

                catch
                {
                }
            }

            // Search by colour
            else if (find == 5)
            {
                ICriterion qcolour = Restrictions.Like("Colour", text, MatchMode.Anywhere);
                criteria.Add(qcolour);
            }

            // Search by price
            else if (find == 6)
            {
                double price = Convert.ToDouble(keyword);
                ICriterion qprice = Restrictions.Eq("Price", price);
                criteria.Add(qprice);
            }

            // Search all
            else
            {
                bool isNumeric = Utils.IsNumber(keyword);

                if (!isNumeric)
                {
                    ICriterion qmake = Restrictions.Like("Make", text, MatchMode.Anywhere);
                    ICriterion qmodel = Restrictions.Like("Model", text, MatchMode.Anywhere);
                    ICriterion qcolour = Restrictions.Like("Colour", text, MatchMode.Anywhere);

                    AbstractCriterion exp1 = Restrictions.Or(qmake, qmodel);
                    AbstractCriterion exp2 = Restrictions.Or(exp1, qcolour);

                    criteria.Add(exp2);
                }

                else
                {
                    int year = Utils.GetInt(keyword);
                    int doors = Utils.GetInt(keyword);
                    double price = Utils.GetDouble(keyword);

                    ICriterion qyear = Restrictions.Eq("Year", year);
                    ICriterion qdoors = Restrictions.Eq("Doors", doors);
                    ICriterion qprice = Restrictions.Eq("Price", price);

                    ICriterion qmake = Restrictions.Like("Make", text, MatchMode.Anywhere);
                    ICriterion qmodel = Restrictions.Like("Model", text, MatchMode.Anywhere);
                    ICriterion qcolour = Restrictions.Like("Colour", text, MatchMode.Anywhere);

                    AbstractCriterion exp1 = Restrictions.Or(qyear, qdoors);
                    AbstractCriterion exp2 = Restrictions.Or(exp1, qprice);
                    AbstractCriterion exp3 = Restrictions.Or(exp2, qmake);
                    AbstractCriterion exp4 = Restrictions.Or(exp3, qmodel);
                    AbstractCriterion exp5 = Restrictions.Or(exp4, qcolour);

                    criteria.Add(exp5);
                }
            }
        }
    }
}