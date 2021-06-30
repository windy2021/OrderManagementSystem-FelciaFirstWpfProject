using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OrderHeader
    {
        private List<OrderItem> _orderItems = new List<OrderItem>();
        /// <summary>
        /// OrderHeader
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateTime"></param>
        public OrderHeader(int id, DateTime dateTime) : this(id, dateTime, 1) { }
        /// <summary>
        /// OrderHeader
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateTime"></param>
        /// <param name="stateId"></param>
        public OrderHeader(int id, DateTime dateTime, int stateId)
        {
            Id = id;
            this.dateTime = dateTime;
            setState(stateId);
        }
        public DateTime dateTime { get; set; }
        public int Id { get; set; }

        private OrderState _state;

        public OrderState.OrderStates OrderState
        {
            get
            {
                return _state.State;
            }
        }
        /// <summary>
        /// Property to count total of an OrderHeader
        /// </summary>
        public decimal TotalOrder
        {
            get
            {
                decimal total = 0;
                for (int i = 0; i < _orderItems.Count; i++)
                {
                    total = (_orderItems[i].Price * _orderItems[i].Quantity) + total;
                }
                return total;
            }
        }
        /// <summary>
        /// Method to AddOrderItem inside OrderHeader
        /// </summary>
        /// <param name="stockItemId"></param>
        /// <param name="price"></param>
        /// <param name="description"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public OrderItem AddOrderItem(int stockItemId, decimal price, string description, int quantity)
        {
            var item = _orderItems.FirstOrDefault(orderItem => orderItem.StockItemId == stockItemId);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                item = new OrderItem(this, stockItemId, description, price, quantity);
                _orderItems.Add(item);
            }
            return item;
        }
        /// <summary>
        /// Property to get OrderItems
        /// </summary>
        public List<OrderItem> OrderItems
        {
            get
            {
                return _orderItems;
            }
        }
        /// <summary>
        /// Method to set OrderState of an OrderHeader as Complete
        /// </summary>
        public void Complete()
        {
            try
            {
                setState(4);
            }
            catch (InvalidOrderStateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Method to set OrderState of an OrderHeader as Rejected
        /// </summary>
        public void Reject()
        {
            try
            {
                setState(3);
            }
            catch (InvalidOrderStateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Method to set the OrderState of an OrderHeader
        /// </summary>
        /// <param name="stateId"></param>
        private void setState(int stateId)
        {
            switch (stateId)
            {
                case 1:
                    _state = new OrderNew();
                    break;
                case 2:
                    _state = new OrderPending();
                    break;
                case 3:
                    _state = new OrderRejected();
                    break;
                case 4:
                    _state = new OrderComplete();
                    break;
            }
        }
        /// <summary>
        /// Method to submit an OrderHeader
        /// </summary>
        public void Submit()
        {
            try
            {
                setState(2);
            }
            catch (InvalidOrderStateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
