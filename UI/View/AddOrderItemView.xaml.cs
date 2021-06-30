using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controllers;
using UI.Model;

namespace UI
{
    /// <summary>
    /// Interaction logic for AddOrderItemView.xaml
    /// </summary>
    public partial class AddOrderItemView : Page
    {
        int orderHeaderId;
        public AddOrderItemView(AddOrderViewModel modelFromPrevPage)
        {
            InitializeComponent();
            orderHeaderId = modelFromPrevPage.Id;
            OrderController orderController = new OrderController();
            var orderItems = orderController.GetStockItems();

            var items = orderItems.Select((item) => new AddOrderItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                InStock = item.InStock,
            }).ToList();
            AddOrderItemListViewModel model = new AddOrderItemListViewModel();
            model.StockItems = items;
            DataContext = model;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                //this item only contained the selected item
                AddOrderItemViewModel item = btn.DataContext as AddOrderItemViewModel;
                if(item != null)
                {
                    try
                    {
                        OrderController controller = new OrderController();
                        string text = quantity.Text.ToString();
                        int.TryParse(text, out int qty);

                        //this is now an updated orderHeader
                        var orderHeader = controller.UpsertOrderItem(orderHeaderId, item.Id, qty);
                        //we have to make this as OrderHeaderItemViewModel to pass it to the AddOrderView page
                        AddOrderViewModel orderHeaderModel = new AddOrderViewModel()
                        {
                            Id = orderHeader.Id,
                            DateTime = orderHeader.dateTime,
                            Total = orderHeader.TotalOrder,
                            OrderItems = orderHeader.OrderItems,
                            State = orderHeader.OrderState
                        };
                        if(item.InStock < qty)
                        {
                            MessageBox.Show($"There are currently not enough items in stock. Requested: {qty}.\nIn stock: {item.InStock}.\nThis order might be rejected if there is not enough stock on hand when the order is being processed.");
                        }
                        NavigationService.Navigate(new AddOrderView(orderHeaderModel));
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }
        }
    }
}