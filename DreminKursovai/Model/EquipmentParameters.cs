using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreminKursovai.Model
{
    public class EquipmentParameters
    {
        public int Id { get; set; }
        public string Meaning {  get; set; }
        public int EquipmentId { get; set; }
        public int OptionsId { get; set; }
    }
}
