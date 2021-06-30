using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace UI.Model
{

    public class AddOrderViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Total { get; set; }
        public OrderState.OrderStates State { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
