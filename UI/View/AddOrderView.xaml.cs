using Controllers;
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
using Domain;

namespace UI
{
    /// <summary>
    /// Interaction logic for AddOrderView.xaml
    /// </summary>
    public partial class AddOrderView : Page
    {
        public AddOrderView()
        {
            InitializeComponent();
            OrderController orderController = new OrderController();
            var orderHeader = orderController.CreateNewOrderHeader();
            AddOrderViewModel model = new AddOrderViewModel()
            {
                Id = orderHeader.Id,
                DateTime = orderHeader.dateTime,
                State = orderHeader.OrderState,
                Total = orderHeader.TotalOrder
            };
            DataContext = model;
        }
        //call GetOrderHeadeById, pass it all to the DataContex here
        public AddOrderView(AddOrderViewModel model)
        {
            InitializeComponent();
            OrderController orderController = new OrderController();
            List<StockItem> stockItems = orderController.GetStockItems().ToList();

            AddOrderViewModel orderViewModel = new AddOrderViewModel()
            {
                Id = model.Id,
                DateTime = model.DateTime,
                Total = model.Total,
                State = model.State,
                OrderItems = model.OrderItems
            };
            DataContext = orderViewModel;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Button btn = sender as Button;
            if (btn != null)            {
                
                string action = (sender as Button).Content.ToString();
                if (action == "Add Items")
                {
                    AddOrderViewModel model = btn.DataContext as AddOrderViewModel;

                    if (model != null)
                    {
                        //this model is now list of StockItems (AddOrderItemViewModel)
                        NavigationService.Navigate(new AddOrderItemView(model));
                    }
                }
                else if (action == "Submit")
                {
                    try
                    {
                        AddOrderViewModel dataContext = btn.DataContext as AddOrderViewModel;
                        int orderHeaderId = dataContext.Id;
                        OrderController controller = new OrderController();
                        controller.SubmitOrder(orderHeaderId);
                        if (dataContext.OrderItems != null)
                        {
                            NavigationService.Navigate(new OrdersView());
                        } 
                        else
                        {
                            MessageBox.Show("Must have at least one Order Item");
                        }                        
                    }
                    catch(Exception msg)
                    {
                        MessageBox.Show(msg.ToString());
                    }
                }
                else if (action == "Cancel")
                {
                    AddOrderViewModel dataContext = btn.DataContext as AddOrderViewModel;
                    int orderHeaderId = dataContext.Id;
                    OrderController controller = new OrderController();
                    controller.DeleteOrderHeaderAndOrderItems(orderHeaderId);
                    NavigationService.Navigate(new OrdersView());
                }
            }
        }
    }
}
