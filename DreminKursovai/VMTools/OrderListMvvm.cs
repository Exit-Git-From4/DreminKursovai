using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.DB;
using System.Windows;
using DreminKursovai.Model;
using DreminKursovai.View;

namespace DreminKursovai.VMTools
{
    internal class OrderListMvvm : BaseVM
    {
        private Order selectedOrder;
        private ObservableCollection<Order> orders = new();

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                Signal();
            }
        }
        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                Signal();
            }
        }

        public CommandMvvm Add {  get; set; }
        public OrderListMvvm()
        {
            Add = new CommandMvvm(() =>
            {
                Order order = SelectedOrder;
                new AddOrder(order).ShowDialog();
                SelectAll();
            }, () => true);
        }
        

        private void SelectAll()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll());
        }
    }
}
