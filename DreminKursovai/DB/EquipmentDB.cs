using DreminKursovai.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreminKursovai.DB
{
    public class EquipmentDB
    {
        DBConnection connection;
        private EquipmentDB(DBConnection db)
        {
            this.connection = db;
        }
        public List<Equipment> SearchEquipment(string search)
        {
            List<Equipment> result = new();

            string GG = $"";
            return result;
        }
        
    }
}
