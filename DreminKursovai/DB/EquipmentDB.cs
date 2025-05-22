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

            string GG = $"SELECT * FROM Equipment e JOIN Manufacturer m ON e.ManufacturerId = m.Id JOIN EquipmentType et ON e.EquipmentTypeId = et.Id WHERE e.Title LIKE @search";
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
                            equipment.Title = dr.GetString("Title");
                            equipment.Model = dr.GetString("Model");
                            equipment.Value = dr.GetDecimal("Value");
                            equipment.ReleaseYear = dr.GetInt32("ReleaseYear");
                            equipment.ManufacturerId = dr.GetInt32("ManufacturerId");
                            equipment.EquipmentTypeId = dr.GetInt32("EquipmentTypeId");

                            var manufacturer = manufacturers.FirstOrDefault(s => s.Id == equipment.ManufacturerId);
                            if (manufacturer == null)
                            {
                                manufacturer= new Manufacturer();
                                manufacturer.Id = equipment.ManufacturerId;
                                manufacturer.Title = dr.GetString("Title");
                                manufacturer.Сountry = dr.GetString("Сountry");
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
                            equipment.EquipmentType = equipmentType;
                            equipment.Manufacturer = manufacturer;
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
                MySqlCommand cmd = connection.CreateCommand("insert into `Equipment` Values(0,@Title,@Model,@Value,@ReleaseYear,@ManufacturerId,@EquipmentTypeId);select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", gogo.Title));
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
                var command = connection.CreateCommand("SELECT e.`Id` AS eid, e.`Title` AS etitle,e.`Model` AS emodel,e.`Value` AS evalue,e.`ReleaseYear` AS ereleaseYear,e.`ManufacturerId` AS emanufacturerId, m.Title AS mtitle, m.`Сountry` AS mcoutry,  e.EquipmentTypeId AS eequipmentTypeId,et.Title AS  ettitle \r\nFROM Equipment e JOIN Manufacturer m ON e.ManufacturerId = m.Id JOIN EquipmentType et ON e.EquipmentTypeId = et.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string titless = string.Empty;
                        if (!dr.IsDBNull(1))
                            titless = dr.GetString("etitle");
                        string model = string.Empty;
                        if (!dr.IsDBNull(2))
                            model = dr.GetString("emodel");
                        decimal value = 0;
                        if (!dr.IsDBNull(3))
                            value = dr.GetDecimal("evalue");
                        int releaseYear = 0;
                        if (!dr.IsDBNull(4))
                            releaseYear = dr.GetInt16("ereleaseYear");
                        int manufacturerId = dr.GetInt16("emanufacturerId");
                        
                        string title = string.Empty;
                        if (!dr.IsDBNull(7))
                            title = dr.GetString("mtitle");
                        string country = string.Empty;
                        if (!dr.IsDBNull(8))
                            country = dr.GetString("mcoutry");
                        int equipmentTypeId = dr.GetInt16("eequipmentTypeId");

                        string Titles = string.Empty;
                        if (!dr.IsDBNull(9))
                            Titles = dr.GetString("ettitle");


                        Manufacturer manufacturer = new Manufacturer()
                        {
                            Id = manufacturerId,
                            Title = title,
                            Сountry = country,
                        };

                        EquipmentType equipmentType = new EquipmentType()
                        {
                            Id = equipmentTypeId,
                            Title = Titles,
                        };

                        result.Add(new Equipment
                        {
                            Id = id,
                            Title = titless,
                            Model = model,
                            Value = value,
                            ReleaseYear = releaseYear,
                            ManufacturerId = manufacturerId,
                            Manufacturer = manufacturer,
                            EquipmentTypeId = equipmentTypeId,
                            EquipmentType = equipmentType
                        });
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
        public static EquipmentDB GetDB()
        {
            if (db == null)
                db = new EquipmentDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Update(Equipment edit)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Equipment` set `Title`=@Title , `Model`=@Model , `Value`=@Value , `ReleaseYear`=@ReleaseYear , `ManufacturerId`=@ManufacturerId , `EquipmentTypeId`=@EquipmentTypeId where `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("Model", edit.Model));
                mc.Parameters.Add(new MySqlParameter("Value", edit.Value));
                mc.Parameters.Add(new MySqlParameter("ReleaseYear", edit.ReleaseYear));
                mc.Parameters.Add(new MySqlParameter("ManufacturerId", edit.ManufacturerId));
                mc.Parameters.Add(new MySqlParameter("EquipmentTypeId", edit.EquipmentTypeId));
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex) {MessageBox.Show(ex.Message); }
            }
            connection.CloseConnection();
            return result;

        }

        internal bool Remove(Equipment selectedEquipment)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Equipment` where `id` = {selectedEquipment.Id}");
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
