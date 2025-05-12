using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DreminKursovai.Model;
using MySqlConnector;

namespace DreminKursovai.DB
{
    public class EquipmentTypeOptionsDB
    {
        DBConnection connection;
        private EquipmentTypeOptionsDB(DBConnection connectionn)
        {
            this.connection = connectionn;
        }
        
        public List<EquipmentTypeOptions> SearchEquipmentTypeOptions(string search)
        {
            List<EquipmentType> equipmentType = new();
            List<Options> options = new();

            string inegr = $""
        }
    }
}
