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
        private string search;
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
        public CommandMvvm Delete { get; set; }

       

        public OrderListMvvm()
        {
            SelectAll();
            Add = new CommandMvvm(() =>
            {
                Order order = SelectedOrder == null ? new() : SelectedOrder;
                new AddOrder(order).ShowDialog();
                SelectAll();
            }, () => true);

            Delete = new CommandMvvm(() =>
            {
                
                OrderDB.GetDB().Remove(SelectedOrder);
                SelectAll();
            }, () => SelectedOrder != null);
        }

        public string Search
        {
            get => search;
            set
            {
                search = value;
                SearchOrder(search);
            }
        }

        private void SelectAll()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDB().SelectAll());
            for (int i = 0; i < Orders.Count; i++)
                {
                if (Orders[i].OrderStatus)
                    Orders[i].orderstatuss = "Заказан";
                else
                    Orders[i].orderstatuss = "Не заказан";
            }
        }
        private void SearchOrder(string search)
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDB().SearchOrder(search));
        }
    }
}
