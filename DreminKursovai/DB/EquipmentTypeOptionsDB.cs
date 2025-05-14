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
            List<EquipmentTypeOptions> equipmentTypeOptions = new();
            List<EquipmentType> equipmentTypes = new();
            List<Options> options = new();

            string JJ = $"SELECT * FROM EquipmentTypeOptions eto JOIN EquipmentType et ON eto.EquipmentTypeId = et.Id JOIN Options o ON eto.OptionsId = o.Id";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(JJ))
                {
                    mc.Parameters.Add(new MySqlParameter("search",$"%{search}"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var equipmentTypeOption = new EquipmentTypeOptions();
                            equipmentTypeOption.OptionsId = dr.GetInt32("OptionsId");
                            equipmentTypeOption.EquipmentTypeId = dr.GetInt32("EquipmentTypeId");

                            var equipmentType = equipmentTypes.FirstOrDefault(s => s.Id== equipmentTypeOption.EquipmentTypeId);
                            if (equipmentType == null)
                            {
                                equipmentType = new EquipmentType();
                                equipmentType.Id = equipmentTypeOption.EquipmentTypeId;
                                equipmentType.Title = dr.GetString("Title");
                            }

                            var option = options.FirstOrDefault(c => c.Id == equipmentTypeOption.OptionsId);
                            if (option == null)
                            {
                                option = new Options();
                                option.Id = equipmentTypeOption.OptionsId;
                                option.Title = dr.GetString("Title");
                            }
                            equipmentTypeOption.Options = option;
                            equipmentTypeOption.EquipmentType = equipmentType;
                            equipmentTypeOptions.Add(equipmentTypeOption);
                        }
                    }
                    connection.CloseConnection();
                }
            }
            return equipmentTypeOptions;
        }
        public bool Insert(EquipmentTypeOptions igogo)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `EquipmentTypeOptions` Values(0,@EquipmentTypeId,@OptionsId);");
                cmd.Parameters.Add(new MySqlParameter("EquipmentTypeId", igogo.EquipmentTypeId));
                cmd.Parameters.Add(new MySqlParameter("OptionsId", igogo.OptionsId));
                try
                {
                   cmd.ExecuteScalar();
                }
                catch (Exception ex) {MessageBox.Show(ex.Message); }

            }
            connection.CloseConnection();
            return result;
        }
        internal List<EquipmentTypeOptions> SelectAll()
        {
            List<EquipmentTypeOptions> result = new List<EquipmentTypeOptions> ();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT eto.`EquipmentTypeId` AS etoequipmenttypeId , et.`Title` AS ettitle , eto.`OptionsId` AS etooptionsid , o.`Title` AS otitle FROM EquipmentTypeOptions eto JOIN EquipmentType et ON eto.EquipmentTypeId = et.Id JOIN Options o ON eto.OptionsId = o.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int equipmentTypeId = dr.GetInt16("etoequipmenttypeId");
                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("ettitle");

                        int optionsId = dr.GetInt16("etooptionsid");
                        string titlee = string.Empty;
                        if (!dr.IsDBNull(3))
                            titlee = dr.GetString("otitle");

                        EquipmentType equipmentType = new EquipmentType()
                        {
                            Id = equipmentTypeId,
                            Title = title,
                        };

                        Options options = new Options()
                        {
                            Id = optionsId,
                            Title = titlee,
                        };

                        result.Add(new EquipmentTypeOptions()
                        {
                            EquipmentTypeId = equipmentTypeId,
                            OptionsId = optionsId
                        });
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            connection.CloseConnection();
            return result;
        }
        static EquipmentTypeOptionsDB db;
        public static EquipmentTypeOptionsDB GetDB()
        {
            if (db == null)
                db = new EquipmentTypeOptionsDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Remove(EquipmentTypeOptions SelectedEquipmentTypeOptions)
        {
            bool result = false;
            if (connection == null)
                return result;

            //if (connection.OpenConnection())
            //{
            //    var mc = connection.CreateCommand($"delete from `Equipment` where `id` = {SelectedEquipmentTypeOptions.Id}");
            //    try
            //    {
            //        mc.ExecuteNonQuery();
            //        result = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
            connection.CloseConnection();
            return result;
        }
    }
}
