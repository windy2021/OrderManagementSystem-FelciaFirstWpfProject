using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace UI.Model
{
    public class OrderHeaderListViewModel
    {
        public List<OrderHeaderItemViewModel> OrderHeaders { get; set; }
        public OrderHeaderListViewModel()
        {
            OrderHeaders = new List<OrderHeaderItemViewModel>();
        }
    }
    public class OrderHeaderItemViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public OrderState.OrderStates State { get; set; }
        public decimal Total { get; set; }
        public int Item { get; set; }
    }
}
