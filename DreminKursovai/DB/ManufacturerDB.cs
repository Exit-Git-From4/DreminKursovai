using DreminKursovai.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DreminKursovai.DB
{
    public class ManufacturerDB
    {
        DBConnection connection;
        private ManufacturerDB(DBConnection connection)
        {
            this.connection = connection;
        }

        public List<Manufacturer> SearchManufacturer(string searchh)
        {
            List<Manufacturer> list = new List<Manufacturer>();
            string GH = $"SELECT * FROM Manufacturer m";
            if (connection.OpenConnection())
            {
                using (var mv = connection.CreateCommand(GH))
                {
                    mv.Parameters.Add(new MySqlParameter("searchh", $"%{searchh}%"));
                    using (var m = mv.ExecuteReader())
                    {
                        while (m.Read())
                        {
                            var manufacturer = new Manufacturer();
                            manufacturer.Id = m.GetInt32("Id");
                            manufacturer.Title = m.GetString("Title");
                            manufacturer.Сountry = m.GetString("Сountry");
                        }
                    }
                    connection.CloseConnection();
                }
            }
            return list;
        }
        public bool Insert(Manufacturer manufacturer)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Manufacturer` Values(0,@Title,@Сountry); select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", manufacturer.Title));
                cmd.Parameters.Add(new MySqlParameter("Сountry", manufacturer.Сountry));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        manufacturer.Id = id;
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
            connection.CloseConnection();
            return result;
        }
        internal List<Manufacturer> SelectAll()
        {
            List<Manufacturer> list = new List<Manufacturer>();
            if (connection == null) return list; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT `Id`,`Title` , `Сountry` FROM Manufacturer m");
                try
                {
                    MySqlDataReader m = command.ExecuteReader();
                    while (m.Read())
                    {
                        int id = m.GetInt32(0);
                        string title = string.Empty;
                        if (!m.IsDBNull(1))
                            title = m.GetString("Title");
                        string country = string.Empty;
                        if (!m.IsDBNull(2))
                            country = m.GetString("Сountry");

                        list.Add(new Manufacturer()
                        {
                            Id = id,
                            Title = title,
                            Сountry = country
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return list;
        }
        static ManufacturerDB db;
        public static ManufacturerDB GetDB()
        {
            if (db == null)
                db = new ManufacturerDB(DBConnection.GetDbConnection());
            return db;
        }
        internal bool Update(Manufacturer edit)
        {
            bool result = false;
            if (connection ==null)
                return result;
            
            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Manufacturer` set `Title`=@Title, `Сountry`=@Сountry where `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("Сountry", edit.Сountry));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message );
                }
            }
            connection.CloseConnection() ;
            return result;
        }
        internal bool Remove(Manufacturer selectedManufacturer)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Manufacturer` where `id` = {selectedManufacturer.Id}");
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
            connection.CloseConnection();
            return result;
        }
    }
}
