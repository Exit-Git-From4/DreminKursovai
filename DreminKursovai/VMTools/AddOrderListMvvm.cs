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
            SelectedOrder = order;
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                order.EquipmentTypeId = order.EquipmentType.Id;
                if (SelectedOrder.Id > 0)
                    OrderDB.GetDB().Update(SelectedOrder);
                else OrderDB.GetDB().Insert(SelectedOrder);
                close();
            }, () =>
            SelectedOrder != null &&
            !string.IsNullOrWhiteSpace(SelectedOrder.Title) &&
            !string.IsNullOrWhiteSpace(SelectedOrder.Model) &&
            SelectedOrder.Quantity != null &&
            SelectedOrder.EquipmentType != null
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
