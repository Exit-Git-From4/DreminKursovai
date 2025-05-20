using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreminKursovai.Model
{
    //Заказ
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Model { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public bool OrderStatus { get; set; }
        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}
