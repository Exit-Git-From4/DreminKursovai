using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.Model;
using DreminKursovai.DB;

using Microsoft.VisualBasic;

namespace DreminKursovai.VMTools
{
    public class AddOrderListMvvm : BaseVM
    {
        private Order selectedOrder;
        private ObservableCollection<Order> orderlist = new();
        private ObservableCollection<EquipmentType> equipmentType = new();

        public ObservableCollection<Order> Orders { get => orderlist; set { orderlist = value; Signal(); } }
        public ObservableCollection<EquipmentType> EquipmentTypes { get => equipmentType; set { equipmentType = value; Signal(); } }
        public Order SelectedOrder { get => selectedOrder; set { selectedOrder = value; Signal(); } }
        public CommandMvvm Save {  get; set; }

        public AddOrderListMvvm(Order order)
        {
            SelectedOrder = new Order();
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                SelectedOrder.EquipmentTypeId = SelectedOrder.EquipmentType.Id;
                OrderDB.GetDB().Insert(SelectedOrder);
                close();
            }, () =>
            SelectedOrder != null &&
            !string.IsNullOrWhiteSpace(SelectedOrder.Title) &&
            !string.IsNullOrWhiteSpace(SelectedOrder.Model) &&
            SelectedOrder.Quantity > 0 &&
            SelectedOrder.EquipmentType != null &&
            SelectedOrder.OrderDate <= DateTime.Now
            );
        }
        private void SelectAll()
        {
            EquipmentTypes = new ObservableCollection<EquipmentType>(EquipmentTypeDB.GetDB().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
