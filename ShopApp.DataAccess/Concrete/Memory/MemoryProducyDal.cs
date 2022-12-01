using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.Memory
{
    public class MemoryProductDal : IProductDal
    {
        public void Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            var products = new List<Product>()
            {
                new Product (){ProductId=1,Name="Iphone 12 Pro",ImageUrl="1.jpg",Price=1000},
                new Product (){ProductId=2,Name="Iphone 11 Pro",ImageUrl="2.jpg",Price=500},
                new Product (){ProductId=3,Name="Iphone 13 Pro",ImageUrl="3.jpg",Price=2000},
                new Product (){ProductId=4,Name="Iphone 14 Pro",ImageUrl="4.jpg",Price=3000},
            };
            return products;
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetOne(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
