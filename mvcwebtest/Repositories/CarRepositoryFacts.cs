using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NHibernate;
using NHibernate.Context;
using FizzWare.NBuilder;
using mvcweb.App;
using mvcweb.Models;
using mvcweb.Repositories;

namespace mvcwebtest.Facts
{
    public class TestFixture : IDisposable
    {
        public ISessionFactory SessionFactory { get; set; }

        public TestFixture()
        {
        }

        public void Init(ISessionFactory s)
        {
            SessionFactory = s;
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        public void Dispose()
        {
            ISession session = CurrentSessionContext.Unbind(SessionFactory);
            session.Close();
        }
    }

    public class CarRepositoryFacts : NHibernateFixtureBase, IUseFixture<TestFixture>, IDisposable
    {
        private TestFixture o;
        private ICarRepository r;

        public CarRepositoryFacts()
            : base("mvcweb", "8270bb15-56d3-47c8-9ef8-a0e3007687cd.mysql.sequelizer.com", 3306, "testdb", "wdiwtcozisthkiwp ", "HB6j7L7kZuf4sBvr7vpP3WkUbVLEVfVKnHkvsC3pjiby7MK2uCrdenQUqjij8zxU")
        {
            r = new CarRepository();
            NHibernateUtils.SessionFactory = SessionFactory;
            PrepareData();
        }

        public void SetFixture(TestFixture x)
        {
            x.Init(SessionFactory);
            o = x;
        }

        private void PrepareData()
        {
            using (ISession s = SessionFactory.OpenSession())
            {
                using (ITransaction tx = s.BeginTransaction())
                {
                    List<Car> l = Builder<Car>.CreateListOfSize(100).Build().ToList();
                    foreach (Car c in l)
                    {
                        s.Save(c);
                    }

                    tx.Commit();
                }
            }
        }

        [Fact]
        public void GetAll()
        {
            Dictionary<string, object> d = r.GetAll(1, 0);

            Assert.IsType(typeof(string), d["item_msg"]);
            Assert.IsType(typeof(int), d["hasnext"]);
            Assert.IsType(typeof(int), d["hasprev"]);
            Assert.IsType(typeof(int), d["nextpage"]);
            Assert.IsType(typeof(int), d["prevpage"]);
            Assert.IsType(typeof(List<Car>), d["list"]);

            Assert.Equal("1 to 100 of 100", d["item_msg"]);
            Assert.Equal(0, d["hasnext"]);
            Assert.Equal(0, d["hasprev"]);
            Assert.Equal(2, d["nextpage"]);
            Assert.Equal(0, d["prevpage"]);
            Assert.Equal(100, ((IList<Car>)d["list"]).Count);

            d = r.GetAll(2, 10);

            Assert.Equal("11 to 20 of 100", d["item_msg"]);
            Assert.Equal(1, d["hasnext"]);
            Assert.Equal(1, d["hasprev"]);
            Assert.Equal(3, d["nextpage"]);
            Assert.Equal(1, d["prevpage"]);
            Assert.Equal(10, ((IList<Car>)d["list"]).Count);
        }

        [Fact]
        public void GetFilterBy()
        {
            Dictionary<string, object> d = r.GetFilterBy(0, "10", 1, 0);

            Assert.IsType(typeof(string), d["item_msg"]);
            Assert.IsType(typeof(int), d["hasnext"]);
            Assert.IsType(typeof(int), d["hasprev"]);
            Assert.IsType(typeof(int), d["nextpage"]);
            Assert.IsType(typeof(int), d["prevpage"]);
            Assert.IsType(typeof(List<Car>), d["list"]);

            Assert.Equal("1 to 2 of 2", d["item_msg"]);
            Assert.Equal(0, d["hasnext"]);
            Assert.Equal(0, d["hasprev"]);
            Assert.Equal(2, d["nextpage"]);
            Assert.Equal(0, d["prevpage"]);
            Assert.Equal(2, ((IList<Car>)d["list"]).Count);

            d = r.GetFilterBy(0, "1", 2, 10);

            Assert.Equal("11 to 20 of 20", d["item_msg"]);
            Assert.Equal(0, d["hasnext"]);
            Assert.Equal(1, d["hasprev"]);
            Assert.Equal(3, d["nextpage"]);
            Assert.Equal(1, d["prevpage"]);
            Assert.Equal(10, ((IList<Car>)d["list"]).Count);
        }

        [Fact]
        public void Get()
        {
			Car c = r.Get(100);
            Assert.NotNull(c);
            Assert.Equal("Make100", c.Make);
            Assert.Equal("Model100", c.Model);
        }

        [Fact]
        public void Add()
        {
            Car c = Builder<Car>.CreateNew().Build();
            r.Add(c);
            Assert.Equal(101, c.ID);
            Assert.Equal("Make1", c.Make);
            Assert.Equal("Model1", c.Model);
            Assert.Equal("Colour1", c.Colour);
        }

        [Fact]
        public void Delete()
        {
			IList<Car> l = Builder<Car>.CreateListOfSize(5).Build();

            using (ISession s = SessionFactory.OpenSession())
            {
                int count = s.QueryOver<Car>().ToRowCountQuery().FutureValue<int>().Value;
                Assert.Equal(100, count);

                using (ITransaction tx = s.BeginTransaction())
                {
                    foreach (Car x in l)
                    {
                        s.Save(x);
                    }

                    tx.Commit();

                    count = s.QueryOver<Car>().ToRowCountQuery().FutureValue<int>().Value;
                    Assert.Equal(105, count);
                }
            }

            r.Delete(l[3].ID);
            r.Delete(l[4].ID);

            using (ISession s = SessionFactory.OpenSession())
            {
                int count = s.QueryOver<Car>().ToRowCountQuery().FutureValue<int>().Value;
                Assert.Equal(103, count);
            }
        }

        [Fact]
        public void Edit()
        {
			Car c = r.Get(99);
            Assert.NotNull(c);

            c.Colour = "Brown";
            c.Doors = 4;
            c.Make = "Honda";
            c.Model = "City";
            c.Price = 109000;
            c.Year = 2012;

            r.Edit(c);

            Car a = r.Get(99);
            Assert.NotNull(a);
            Assert.Equal("Brown", a.Colour);
            Assert.Equal(4, a.Doors);
            Assert.Equal("Honda", a.Make);
            Assert.Equal("City", a.Model);
            Assert.Equal(109000, a.Price);
            Assert.Equal(2012, a.Year);
        }

        [Fact]
        public void GetItemMessage()
        {
			string s = r.GetItemMessage(0, "10", 1, 20);
            Assert.Equal(s, "1 to 2 of 2");
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}