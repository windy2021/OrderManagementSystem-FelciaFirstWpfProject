using System;

namespace Domain
{
    public class OrderItem
    {
        /// <summary>
        /// Cosntructor of an OrderItem
        /// </summary>
        /// <param name="orderHeader"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        public OrderItem(OrderHeader orderHeader, int id, string description, decimal price, int quantity)
        {
            OrderHeader = orderHeader;
            StockItemId = id;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
        public int OrderHeaderId
        {
            get
            {
                return OrderHeader.Id;
            }
        }
        public int StockItemId { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}
