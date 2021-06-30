using System;
using Domain;
using DataAccess;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Controllers
{
    public class OrderController
    {
        /// <summary>
        /// A private field type Repository
        /// </summary>
        private readonly Repository _repository;
        public OrderController()
        {
            _repository = new Repository();
        }
        /// <summary>
        /// Call the GetOrderHeaders() in Repository
        /// </summary>
        /// <returns>IEnumerable<OrderHeader></returns>
        public IEnumerable<OrderHeader> GetOrderHeaders()
        {
            return _repository.GetOrderHeaders();
        }
        /// <summary>
        /// Call the InsertNewOrderHeader() in Repository
        /// </summary>
        /// <returns>Orderheader</returns>
        public OrderHeader CreateNewOrderHeader()
        {
            return _repository.InsertOrderHeader();
        }
        /// <summary>
        /// Call the GetStockItems() in Repository
        /// </summary>
        /// <returns>IEnumerable<StockItem></returns>
        public IEnumerable<StockItem> GetStockItems()
        {
            return _repository.GetStockItems();
        }
        /// <summary>
        /// Call the UpsertOrderItem() in Repository
        /// </summary>
        /// <param name="orderHeaderId"></param>
        /// <param name="stockItemId"></param>
        /// <param name="quantity"></param>
        /// <returns>OrderHeader</returns>
        public OrderHeader UpsertOrderItem(int orderHeaderId, int stockItemId, int quantity)
        {
            //call GetStockItem
            StockItem stockItem = _repository.GetStockItemById(stockItemId);
            //call GetOrderHeader
            OrderHeader orderHeader = _repository.GetOrderHeaderById(orderHeaderId);
            //call AddOrderItem
            OrderItem orderItem = orderHeader.AddOrderItem(stockItem.Id, stockItem.Price, stockItem.Name, quantity);
            //call UpsertOrderItem
            _repository.UpsertOrderItem(orderItem);
            //return updated OrderHeader object	
            return orderHeader;
        }
        /// <summary>
        /// Call the GetOrderHeaderById() in Repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OrderHeader</returns>
        public OrderHeader GetOrderHeaderById(int id)
        {
            OrderHeader orderHeader = _repository.GetOrderHeaderById(id);
            return orderHeader;
        }
        /// <summary>
        /// Call the SubmitOrder() in Repository
        /// </summary>
        /// <param name="orderHeaderId"></param>
        /// <returns>OrderHeader</returns>
        public OrderHeader SubmitOrder(int orderHeaderId)
        {
            //call GetOrderHeader method in OrderRepo to get the details of order header 
            OrderHeader orderHeader = _repository.GetOrderHeaderById(orderHeaderId);
            //call Submit method of OrderHeader 
            orderHeader.Submit();
            //call UpdateOrderState method in OrderRepo
            _repository.UpdateOrderState(orderHeader);
            //return updated OrderHeader object
            return orderHeader;
        }
        /// <summary>
        /// Call the DeleteOrderHeaderAndOrderItems() in Repository
        /// </summary>
        /// <param name="orderHeaderId"></param>
        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
             _repository.DeleteOrderHeaderAndOrderItems(orderHeaderId);
        }
        /// <summary>
        /// Processing the OrderHeader and call the UpdateOrderState() in Repository
        /// </summary>
        /// <param name="orderHeaderId"></param>
        /// <returns></returns>
        public OrderHeader ProcessOrder(int orderHeaderId)
        {
            var order = _repository.GetOrderHeaderById(orderHeaderId);
            try
            {
                _repository.UpdateStockItemAmount(order);
                order.Complete();
            }
            catch (SqlException ex)
            {
                //Check Constraint Violation
                //The UPDATE statement conflicted with the CHECK constraint "CK_StockItems"
                //Checked in the error statement in SQL server, the number of this error is 547
                if (ex.Number == 547)
                {
                    order.Reject();
                }
            }
            _repository.UpdateOrderState(order);
            return order;
        }
    }
}
