using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.VMTools;

namespace DreminKursovai.Model
{
    //Заказ
    public class Order// : BaseVM
    {
        private EquipmentType equipmentType;

        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Model { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public bool OrderStatus { get; set; }
        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get => equipmentType; set => equipmentType = value; }//Signal(); } }
    }
}
