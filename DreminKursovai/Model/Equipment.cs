using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreminKursovai.Model
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public decimal Value { get; set; }
        public int ReleaseYear { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
    //🐷🐷🐷
}
