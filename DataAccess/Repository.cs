using System;
using Microsoft.Data.Sql;
using System.Configuration;
using Domain;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataAccess
{
    public class Repository
    {
        private string _connectionString;
        /// <summary>
        /// Construction of Repository class
        /// </summary>
        /// <param name="connection"></param>
        public Repository(string connection)
        {
            _connectionString = connection;
        }
        public Repository()
        {           
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        /// <summary>
        /// Method to get all the OrderHeader
        /// </summary>
        /// <returns>List<OrderHeader></returns>
        public List<OrderHeader> GetOrderHeaders()
        {
            List<OrderHeader> orderHeaders = new();

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_SelectOrderHeaders", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        OrderHeader orderHeader = new OrderHeader(reader.GetInt32(0), reader.GetDateTime(2), reader.GetInt32(1));
                        orderHeader.AddOrderItem(reader.GetInt32(3), reader.GetDecimal(5), reader.GetString(4), reader.GetInt32(6));
                        orderHeaders.Add(orderHeader);
                    }
                    while (reader.Read())
                    {
                        if (orderHeaders[orderHeaders.Count - 1].Id == reader.GetInt32(0))
                        {
                            orderHeaders[orderHeaders.Count - 1].AddOrderItem(reader.GetInt32(3), reader.GetDecimal(5), reader.GetString(4), reader.GetInt32(6));
                        }
                        else
                        {
                            OrderHeader orderHeader = new OrderHeader(reader.GetInt32(0), reader.GetDateTime(2), reader.GetInt32(1));
                            orderHeader.AddOrderItem(reader.GetInt32(3), reader.GetDecimal(5), reader.GetString(4), reader.GetInt32(6));
                            orderHeaders.Add(orderHeader);
                        }
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }
            }
             return orderHeaders;
        }
        /// <summary>
        /// Method to make a new OrderHeader
        /// </summary>
        /// <returns>OrderHeader</returns>
        public OrderHeader InsertOrderHeader()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                decimal orderHeaderId = 0;
                SqlCommand command = new SqlCommand("sp_InsertOrderHeader", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    orderHeaderId = (decimal)command.ExecuteScalar();
                    
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                    Console.WriteLine(exx.StackTrace);
                }
                return GetOrderHeaderById((int)orderHeaderId);
            }
        }
        /// <summary>
        /// Method to get all the StockItem
        /// </summary>
        /// <returns>IEnumerable<StockItem></returns>
        public IEnumerable<StockItem> GetStockItems()
        {
            List<StockItem> stockItems = new();

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_SelectStockItems", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        decimal price = reader.GetDecimal(2);
                        int inStock =reader.GetInt32(3);
                        StockItem stockItem = new StockItem(id, name, price, inStock);
                        stockItems.Add(stockItem);
                    }
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                    Console.WriteLine(exx.StackTrace);
                }
            }
            return stockItems;
        }
        /// <summary>
        /// Method to get the StockItem by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>StockItem</returns>
        public StockItem GetStockItemById(int id)
        {
            string name = "";
            decimal price = 0;
            int inStock = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_SelectStockItemById", connection);
                command.Parameters.AddWithValue("@id", id);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        name = reader.GetString(1);
                        price = reader.GetDecimal(2);
                        inStock = reader.GetInt32(3);
                    }
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                    Console.WriteLine(exx.StackTrace);
                }
            }
            StockItem stockItem = new StockItem(id, name, price, inStock);
            return stockItem;
        }
        /// <summary>
        /// Method to insert OrderItem to the OrderHeader
        /// </summary>
        /// <param name="orderItem"></param>
        public void UpsertOrderItem(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpsertOrderItem", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderheaderId", orderItem.OrderHeaderId);
                command.Parameters.AddWithValue("@stockItemId", orderItem.StockItemId);
                command.Parameters.AddWithValue("@description", orderItem.Description);
                command.Parameters.AddWithValue("@price", orderItem.Price);
                command.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                try
                {
                    connection.Open();
                    if(orderItem.Quantity <= 0)
                    {
                        throw new Exception("Info: Please enter the Quantity\nInfo: The Quantity must be greater than zero");
                    }
                    
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Method to update OrderState of an OrderHeader
        /// </summary>
        /// <param name="orderHeader"></param>
        public void UpdateOrderState(OrderHeader orderHeader)
        {
            //update the state of an existing order header - execute sp_UpdateOrderState
            //hey felcia, check what's in the orderHeader
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateOrderState", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderheaderId", orderHeader.Id);
                command.Parameters.AddWithValue("@stateId", orderHeader.OrderState);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                    Console.WriteLine(exx.StackTrace);
                }
            }
        }
        /// <summary>
        /// Method to delete an OrderHeader and its OrderItems
        /// </summary>
        /// <param name="orderHeaderId"></param>
        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
            //delete an existing order header and its items - execute sp_DeleteOrderHeaderAndOrderItems
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteOrderHeaderAndOrderItems", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderheaderId", orderHeaderId);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                    Console.WriteLine(exx.StackTrace);
                }
            }
        }
        /// <summary>
        /// Method to delete an OrderItem in an OrderHeader
        /// </summary>
        /// <param name="orderHeaderId"></param>
        /// <param name="stockItemId"></param>
        public void DeleteOrderItem(int orderHeaderId, int stockItemId)
        {
            //delete an existing order item - execute sp_DeleteOrderItem
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteOrderItem", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderheaderId", orderHeaderId);
                command.Parameters.AddWithValue("@stockItemId", stockItemId);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception exx)
                {
                    Console.WriteLine(exx.Message);
                    Console.WriteLine(exx.StackTrace);
                }
            }
        }
        /// <summary>
        /// Method to get an OrderHeader by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OrderHeader</returns>
        public OrderHeader GetOrderHeaderById(int id)
        {
            DateTime orderHeaderDateTime = new DateTime();
            OrderHeader orderHeader1 = new OrderHeader(id, orderHeaderDateTime);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_selectorderheaderbyid", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        orderHeader1.dateTime = reader.GetDateTime(2);
                        if (!reader.IsDBNull(3))
                        {
                            orderHeader1.AddOrderItem(reader.GetInt32(3), reader.GetDecimal(5), reader.GetString(4), reader.GetInt32(6));
                        }
                    }                    
                }
                catch (Exception)
                {
                    throw;
                }
                return orderHeader1;
            }
        }
        /// <summary>
        /// Method to update stock item amount, when an OrderHeader is being processed
        /// </summary>
        /// <param name="order"></param>
        public void UpdateStockItemAmount(OrderHeader order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateStockItemAmount", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    foreach (var orderItem in order.OrderItems)
                    {
                        command.Parameters.Add(new SqlParameter("@id", orderItem.StockItemId));
                        command.Parameters.Add(new SqlParameter("@amount", -orderItem.Quantity));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
