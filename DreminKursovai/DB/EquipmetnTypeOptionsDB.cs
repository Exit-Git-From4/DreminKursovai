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
        private EquipmentTypeOptionsDB(DBConnection db)
        {
            this.connection = db;
        }
        public bool Insert(EquipmentTypeOptions igogo)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `EquipmentTypeOptions` Values(0,@OptionsId,@EquipmentTypeId);select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("OptionsId", igogo.OptionsId));
                cmd.Parameters.Add(new MySqlParameter("EquipmentTypeId", igogo.EquipmentTypeId));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }
        //internal List<EquipmentTypeOptions> SelectAll()
        //{
        //    List<EquipmentTypeOptions>result = new List<EquipmentTypeOptions>();
        //    if(connection == null) return result;if (connection.OpenConnection())
        //    {
        //        var command = connection.CreateCommand("")
        //    }
        //}
    }
}
