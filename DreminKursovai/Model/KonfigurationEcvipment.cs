using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreminKursovai.Model
{
    public class KonfigurationEcvipment
    {
        public int Id { get; set; }
        public int KonfigurationsId { get; set; }
        public Konfiguration Konfigurationn { get; set; }    
        public int EquipmentId { get; set; } 
        public Equipment Equipmentt { get; set; }
    }
}
