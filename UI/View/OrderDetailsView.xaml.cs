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
using UI.Model;
using Controllers;

namespace UI
{
    /// <summary>
    /// Interaction logic for OrderDetailsView.xaml
    /// </summary>
    public partial class OrderDetailsView : Page
    {
        public OrderDetailsView(OrderHeaderItemViewModel model)
        {
            InitializeComponent();
            OrderController controller = new OrderController();
            //map model to AddOrderViewModel? or just assign it straight to the DataContext?
            //if this is an object from domain, how can i take it out from the DataContext in the Process button click?
            var model1 = controller.GetOrderHeaderById(model.Id);
            AddOrderViewModel orderHeader = new AddOrderViewModel()
            {
                Id = model1.Id,
                DateTime = model1.dateTime,
                Total = model1.TotalOrder,
                State = model.State,
                OrderItems = model1.OrderItems
            };
            DataContext = orderHeader;
            if(model.State.ToString() == "Pending")
            {
                button1.Visibility = Visibility.Visible;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string action = (sender as Button).Content.ToString();
                if (action == "Orders")
                {
                    NavigationService.Navigate(new OrdersView());
                }
                else if (action == "Process")
                {
                    AddOrderViewModel order = btn.DataContext as AddOrderViewModel;

                    OrderController orderController = new OrderController();

                    orderController.ProcessOrder(order.Id);
                    //controller.ProcessOrder(dataContext.Id);
                    NavigationService.Navigate(new OrdersView());
                }
            }
        }
    }
}
