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
    public class EquipmentTypeDB
    {
        DBConnection conn;
        private EquipmentTypeDB(DBConnection connection)
        {
            this.conn = connection;
        }
        public List<EquipmentType> SearchManufacturer(string searchh)
        {
            List<EquipmentType> list = new();
            string GH = $"SELECT * FROM EquipmentType et";
            if (conn.OpenConnection())
            {
                using (var mv = conn.CreateCommand(GH))
                {
                    mv.Parameters.Add(new MySqlParameter("searchh", $"%{searchh}%"));
                    using (var m = mv.ExecuteReader())
                    {
                        while (m.Read())
                        {
                            var equipmentType = new EquipmentType();
                            equipmentType.Id = m.GetInt32("Id");
                            equipmentType.Title = m.GetString("Title");
                        }
                    }
                    conn.CloseConnection();
                }
            }
            return list;
        }
        public bool Insert(EquipmentType equipmentType)
        {
            bool result = false;
            if (conn == null)
                return result;
            if (conn.OpenConnection())
            {
                MySqlCommand cmd = conn.CreateCommand("insert into `EquipmentType` Values(0,@Title); select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", equipmentType.Title));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        equipmentType.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            conn.CloseConnection();
            return result;
        }
        internal List<EquipmentType> SelectAll()
        {
            List<EquipmentType> list = new List<EquipmentType>();
            if (conn == null) return list; if (conn.OpenConnection())
            {
                var command = conn.CreateCommand("SELECT `Id`,`Title` FROM EquipmentType et");
                try
                {
                    MySqlDataReader m = command.ExecuteReader();
                    while (m.Read())
                    {
                        int id = m.GetInt32(0);
                        string title = string.Empty;
                        if (!m.IsDBNull(1))
                            title = m.GetString("Title");

                        list.Add(new EquipmentType()
                        {
                            Id = id,
                            Title = title
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            conn.CloseConnection();
            return list;
        }
        static EquipmentTypeDB db;
        public static EquipmentTypeDB GetDB()
        {
            if (db == null)
                db = new EquipmentTypeDB(DBConnection.GetDbConnection());
            return db;
        }
        internal bool Update(EquipmentType edit)
        {
            bool result = false;
            if (conn == null)
                return result;

            if (conn.OpenConnection())
            {
                var mc = conn.CreateCommand($"update `EquipmentType` set `Title`=@Title where `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            conn.CloseConnection();
            return result;
        }
        internal bool Remove(EquipmentType selectedEquipmentType)
        {
            bool result = false;
            if (conn == null)
                return result;

            if (conn.OpenConnection())
            {
                var mc = conn.CreateCommand($"delete from `EquipmentType` where `id` = {selectedEquipmentType.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            conn.CloseConnection();
            return result;
        }

    }
}
