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
    class KonfigurationEcvipmentDB
    {
        DBConnection connection;
        private KonfigurationEcvipmentDB(DBConnection db)
        {
            this.connection = db;
        }

        public bool Insert(KonfigurationEcvipment gogo)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `KonfigurationEcvipment` Values(0,@KonfigurationsId , @EquipmentId);select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("KonfigurationsId", gogo.KonfigurationsId));
                cmd.Parameters.Add(new MySqlParameter("EquipmentId", gogo.EquipmentId));
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
        internal List<KonfigurationEcvipment> SelectAll()
        {
            List<KonfigurationEcvipment> result = new List<KonfigurationEcvipment>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT ke.`KonfigurationsId` , ke.`EquipmentId` , k.`Title` AS ktitle, k.`Quantity`, e.`Title` AS etitle, e.`Model`, e.`Value` , e.`ReleaseYear` , e.`ManufacturerId` , e.`EquipmentTypeId` FROM KonfigurationEcvipment ke JOIN Konfigurations k ON ke.KonfigurationsId = k.Id JOIN Equipment e ON ke.EquipmentId = e.Id");
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
    }
}
