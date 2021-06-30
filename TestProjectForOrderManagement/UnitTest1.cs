using NUnit.Framework;
using Domain;
using DataAccess;
using System.Configuration;
using System.Collections.Generic;

namespace TestProjectForOrderManagement
{
    public class Tests
    {
        private Repository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new Repository();
            //string _connectionString = "Data Source=DESKTOP-U9LP02C\\SQLEXPRESS;Initial Catalog=OrderManagementDbTestData;" + "Integrated Security=true";
            //_repository = new Repository(_connectionString);
        }

        [Test]
        public void TestGetOrderHeaders()
        {
            IEnumerable<OrderHeader> orderHeaders = _repository.GetOrderHeaders();
            Assert.IsTrue(orderHeaders != null);
        }

        [Test]
        public void TestAddOrderHeader()
        {
            OrderHeader orderHeader = _repository.InsertOrderHeader();
            string state = "New";
            Assert.AreEqual(orderHeader.OrderState.ToString(), state);
            Assert.IsNotNull(orderHeader);
            
        }

        [Test]
        public void TestDeleteOrderItem()
        {
            OrderHeader order = _repository.InsertOrderHeader();
            OrderItem item = new OrderItem(order, 1, "Table", 100, 2);
            _repository.UpsertOrderItem(item);
            _repository.DeleteOrderItem(order.Id, 1);
            //_repository.DeleteOrderHeaderAndOrderItems(order.Id);
            OrderHeader order1 = _repository.GetOrderHeaderById(order.Id);
            Assert.IsTrue(order1.OrderItems.Count == 0);
        }
    }
}