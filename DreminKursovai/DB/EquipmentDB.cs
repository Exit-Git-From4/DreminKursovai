using DreminKursovai.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            List<Equipment> equipments = new();
            List<Manufacturer> manufacturers = new();
            List<EquipmentType> equipmentTypes = new();

            string GG = $"SELECT * FROM Equipment e JOIN Manufacturer m ON e.ManufacturerId = m.Id JOIN EquipmentType et ON e.EquipmentTypeId = et.Id";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(GG))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var equipment = new Equipment();
                            equipment.Id = dr.GetInt32("Id");
                            equipment.Model = dr.GetString("Model");
                            equipment.Value = dr.GetDecimal("Value");
                            equipment.ReleaseYear = dr.GetInt32("ReleasYear");
                            equipment.ManufacturerId = dr.GetInt32("ManufacturerId");
                            equipment.EquipmentTypeId = dr.GetInt32("EquipmentTypeId");

                            var manufacturer = manufacturers.FirstOrDefault(s => s.Id == equipment.ManufacturerId);
                            if (manufacturer == null)
                            {
                                manufacturer= new Manufacturer();
                                manufacturer.Id = equipment.ManufacturerId;
                                manufacturer.Title = dr.GetString("Title");
                                manufacturer.Country = dr.GetString("Country");
                                manufacturers.Add(manufacturer);
                            }
                            var equipmentType = equipmentTypes.FirstOrDefault(d => d.Id == equipment.ManufacturerId);
                            if (equipmentType == null)
                            {
                                equipmentType= new EquipmentType();
                                equipmentType.Id = equipment.EquipmentTypeId;
                                equipmentType.Title = dr.GetString("Title");
                                equipmentTypes.Add(equipmentType);
                            }
                            equipments.Add(equipment);
                        }
                    }
                    connection.CloseConnection();
                }
            }
            return equipments;
        }
        public bool Insert(Equipment gogo)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Equipment` Values(0,@Model,@Value,@ReleaseYear,@ManufacturerId,@EquipmentTypeId);select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Model", gogo.Model));
                cmd.Parameters.Add(new MySqlParameter("Value", gogo.Value));
                cmd.Parameters.Add(new MySqlParameter("ReleaseYear", gogo.ReleaseYear));
                cmd.Parameters.Add(new MySqlParameter("ManufacturerId", gogo.ManufacturerId));
                cmd.Parameters.Add(new MySqlParameter("EquipmentTypeId", gogo.EquipmentTypeId));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        gogo.Id = id;
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
        internal List<Equipment> SelectAll()
        {
            List<Equipment> result = new List<Equipment>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT e.`Id` , `Model`, `Value` , `ReleaseYear` , `ManufacturerId` , `EquipmentTypeId` , m.`Title` , m.`Сountry` , `eq.`Title` FROM Equipment e JOIN Manufacturer m ON e.ManufacturerId = m.Id JOIN EquipmentType et ON e.EquipmentTypeId = et.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string model = string.Empty;
                        if (!dr.IsDBNull(1))
                            model = dr.GetString("Model");
                        decimal value = 0;
                        if (!dr.IsDBNull(2))
                            value = dr.GetDecimal("Value");
                        int releaseYear = 0;
                        if (!dr.IsDBNull(3))
                            releaseYear = dr.GetInt16("ReleaseYear");
                        int manufacturerId = dr.GetInt16("ManufacturerId");
                        int equipmentTypeId = dr.GetInt16("EquipmentTypeId");

                        string title = string.Empty;
                        if (!dr.IsDBNull(6))
                            title = dr.GetString("Title");
                        string country = string.Empty;
                        if (!dr.IsDBNull(7))
                            country = dr.GetString("Сountry");

                        string Titles = string.Empty;
                        if (!dr.IsDBNull(8))
                            Titles = dr.GetString("Title");

                        result.Add(new Equipment()
                        {
                            Id = id,
                            Model = model,
                            Value = value,
                            ReleaseYear = releaseYear,
                            ManufacturerId = manufacturerId,
                            EquipmentTypeId = equipmentTypeId,
                        });

                        Manufacturer manufacturer = new Manufacturer()
                        {
                            Id = manufacturerId,
                            Title = title,
                            Country = country,
                        };

                        EquipmentType equipmentType = new EquipmentType()
                        {
                            Id = equipmentTypeId,
                            Title = title,
                        };
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
        static EquipmentDB db;
        public static EquipmentDB GetDb()
        {
            if (db == null)
                db = new EquipmentDB(DBConnection.GetDbConnection());
            return db;
        }

        internal void Remove(Equipment selectedEquipment)
        {
            throw new NotImplementedException();
        }
    }

}
