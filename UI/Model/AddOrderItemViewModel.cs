using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class AddOrderItemListViewModel
    {
        public List<AddOrderItemViewModel> StockItems { get; set; }
        public AddOrderItemListViewModel()
        {
            StockItems = new List<AddOrderItemViewModel>();
        }
    }
    public class AddOrderItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}
