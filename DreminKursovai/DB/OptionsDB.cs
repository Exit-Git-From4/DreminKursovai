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
    internal class OptionsDB
    {
        DBConnection connection;
        private OptionsDB(DBConnection connection)
        {
            this.connection = connection;
        }

        public List<Options> SearchOptions(string search)
        {
            List<Options> list = new List<Options>();
            string gh = $"SELECT * FROM Options et";
            if (connection.OpenConnection())
            {
                using (var mv = connection.CreateCommand(gh))
                {
                    mv.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var m = mv.ExecuteReader())
                    {
                        while (m.Read())
                        {
                            var equipmenttype = new Options();
                            equipmenttype.Id = m.GetInt32("Id");
                            equipmenttype.Title = m.GetString("Title");
                        }
                    }
                    connection.CloseConnection();
                }
            }
            return list;
        }
        public bool Insert(Options equipmenttype)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Options` Values(0, @Title); select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", equipmenttype.Title));

                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        equipmenttype.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            connection.CloseConnection();
            return result;
        }
        internal List<Options> SelectAll()
        {
            List<Options> list = new List<Options>();
            if (connection == null) return list; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT `Id` , `Title` FROM EquipmentType et");
                try
                {
                    MySqlDataReader m = command.ExecuteReader();
                    while (m.Read())
                    {
                        int id = m.GetInt32(0);
                        string title = string.Empty;
                        if (!m.IsDBNull(1))
                            title = m.GetString("Title");

                        list.Add(new Options()
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
            connection.CloseConnection();
            return list;
        }
        static OptionsDB db;
        public static OptionsDB GetDB()
        {
            if (db == null)
                db = new OptionsDB(DBConnection.GetDbConnection());
            return db;
        }
        internal bool Update(Options edit)
        {
            bool result = false;
            if (connection == null) return result;
            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Options` set `Title`=@Title where `Id` = {edit.Id}");
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
            connection.CloseConnection();
            return result;
        }
        internal bool Remove(Options selectedOptions)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Options` where `id` = {selectedOptions.Id}");
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
