using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// StockItem class
    /// </summary>
    public class StockItem
    {
        /// <summary>
        /// Constructor of a StockItem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="inStock"></param>
        public StockItem(int id, string name, decimal price, int inStock)
        {
            Id = id;
            Name = name;
            Price = price;
            InStock = inStock;
        }
        public int Id { get; set; }        
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}
