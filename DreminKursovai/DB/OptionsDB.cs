using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.Model;
using MySqlConnector;
using System.Windows;

namespace DreminKursovai.DB
{
    internal class OptionsDB
    {
        DBConnection conn;
        private OptionsDB(DBConnection connection)
        {
            this.conn = connection;
        }
        public List<Options> SearchManufacturer(string searchh)
        {
            List<Options> list = new List<Options>();
            string GH = $"SELECT * FROM Options o";
            if (conn.OpenConnection())
            {
                using (var mv = conn.CreateCommand(GH))
                {
                    mv.Parameters.Add(new MySqlParameter("searchh", $"%{searchh}%"));
                    using (var m = mv.ExecuteReader())
                    {
                        while (m.Read())
                        {
                            var options = new Options();
                            options.Id = m.GetInt32("Id");
                            options.Title = m.GetString("Title");
                        }
                    }
                    conn.CloseConnection();
                }
            }
            return list;
        }
        public bool Insert(Options options)
        {
            bool result = false;
            if (conn == null)
                return result;
            if (conn.OpenConnection())
            {
                MySqlCommand cmd = conn.CreateCommand("insert into `Options` Values(0,@Title); select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", options.Title));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        options.Id = id;
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
        internal List<Options> SelectAll()
        {
            List<Options> list = new List<Options>();
            if (conn == null) return list; if (conn.OpenConnection())
            {
                var command = conn.CreateCommand("SELECT `Id`,`Title` FROM Options o");
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
            conn.CloseConnection();
            return list;
        }
        static OptionsDB db;
        public static OptionsDB GetDb()
        {
            if (db == null)
                db = new OptionsDB(DBConnection.GetDbConnection());
            return db;
        }
        internal bool Update(Options edit)
        {
            bool result = false;
            if (conn == null)
                return result;

            if (conn.OpenConnection())
            {
                var mc = conn.CreateCommand($"update `Options` set `Title`=@Title where `Id` = {edit.Id}");
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
        internal bool Remove(Options selectedOptions)
        {
            bool result = false;
            if (conn == null)
                return result;

            if (conn.OpenConnection())
            {
                var mc = conn.CreateCommand($"delete from `Options` where `id` = {selectedOptions.Id}");
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
