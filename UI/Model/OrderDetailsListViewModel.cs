using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class OrderDetailsListViewModel
    {
        public List<OrderDetailsViewModel> OrderDetails { get; set; }
        public OrderDetailsListViewModel()
        {
            OrderDetails = new List<OrderDetailsViewModel>();
        }

    }
    public class OrderDetailsViewModel
    {
        public int SKU { get; set; }
        public string Item { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
