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
using Domain;
using UI.Model;


namespace UI
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : Page
    {
        /// <summary>
        /// OrdersView Page
        /// </summary>
        public OrdersView()
        {
            InitializeComponent();

            OrderController orderController = new OrderController();
            var orderHeaders = orderController.GetOrderHeaders();

            var items = orderHeaders.Select((order) => new OrderHeaderItemViewModel()
            {
                Id = order.Id,
                DateTime = order.dateTime,
                State = order.OrderState,
                Total = order.TotalOrder,
                Item = order.OrderItems.Count()
            }).ToList();
            OrderHeaderListViewModel model = new OrderHeaderListViewModel();
            model.OrderHeaders = items;

            DataContext = model;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string action = (sender as Button).Content.ToString();
                if (action == "Add Orders")
                {
                    NavigationService.Navigate(new AddOrderView());
                }
            }
        }
        //Details button
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                OrderHeaderItemViewModel model = btn.DataContext as OrderHeaderItemViewModel;
                if (model != null)
                {
                    OrderDetailsView page = new OrderDetailsView(model);
                    NavigationService.Navigate(new OrderDetailsView(model));
                }
                    
            }
        }
    }
}
