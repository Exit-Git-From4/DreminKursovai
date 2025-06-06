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
    public class OrderDB
    {
        DBConnection connection;
        private OrderDB(DBConnection db)
        {
            this.connection = db;
        }

        public List<Order> SearchOrder(string search)
        {
            List<Order> equipments = new();
            List<EquipmentType> equipmentTypes = new();
            string GG = $"SELECT * FROM `Order` o JOIN EquipmentType et ON o.EquipmentTypeId = et.Id WHERE o.Title LIKE @search";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(GG))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var equipment = new Order();
                            equipment.Id = dr.GetInt32("Id");
                            equipment.Title = dr.GetString("Title");
                            equipment.Model = dr.GetString("Model");
                            equipment.OrderDate = dr.GetDateTime("OrderDate");
                            equipment.Quantity = dr.GetInt32("Quantity");
                            equipment.OrderStatus = dr.GetBoolean("OrderStatus");
                            equipment.EquipmentTypeId = dr.GetInt32("EquipmentTypeId");

                            var equipmentType = equipmentTypes.FirstOrDefault();
                            if (equipmentType == null)
                            {
                                equipmentType = new EquipmentType();
                                equipmentType.Id = equipment.EquipmentTypeId;
                                equipmentType.Title = dr.GetString("Title");
                                equipmentTypes.Add(equipmentType);
                            }
                            equipment.EquipmentType = equipmentType;
                            equipments.Add(equipment);
                        }
                    }
                    connection.CloseConnection();
                }
            }
            return equipments;
        }

        public bool Insert(Order order)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Order` Values(0,@Title ,@Model , @OrderDate , @Quantity , @OrderStatus ,  @EquipmentTypeId); SELECT LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", order.Title));
                cmd.Parameters.Add(new MySqlParameter("Model", order.Model));
                cmd.Parameters.Add(new MySqlParameter("Quantity", order.Quantity));
                cmd.Parameters.Add(new MySqlParameter("OrderStatus", order.OrderStatus));
                cmd.Parameters.Add(new MySqlParameter("OrderDate", order.OrderDate));
                cmd.Parameters.Add(new MySqlParameter("EquipmentTypeId", order.EquipmentTypeId));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    
                    if (id > 0)
                    {
                        order.Id = id;
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
        internal List<Order> SelectAll()
        {
            List<Order> result = new List<Order>();
            if (connection == null ) return result; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT o.`Id` , o.`Title` AS otitel, o.`Model` AS omodel, o.`OrderDate` AS oorderdate, o.`Quantity` AS oquantity, o.`OrderStatus` AS oorderstatus, o.`EquipmentTypeId` AS oequipmenttypeid, et.`Title`  AS ettitle FROM `Order` o JOIN EquipmentType et ON o.EquipmentTypeId = et.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("otitel");
                        string model = string.Empty;
                        if (!dr.IsDBNull(2))
                            model = dr.GetString("omodel");
                        DateTime orderdate = new DateTime();
                        if (!dr.IsDBNull(3))
                            orderdate = dr.GetDateTime("oorderdate");
                        int quantity = 0;
                        if (!dr.IsDBNull(4))
                            quantity = dr.GetInt32("oquantity");
                        bool donenotdone = true;
                        if (!dr.IsDBNull(5))
                            donenotdone = dr.GetBoolean("oorderstatus");
                        int equipmentTypeId = dr.GetInt32("oequipmenttypeid");

                        string Titles = string.Empty;
                        if (!dr.IsDBNull(7))
                            Titles = dr.GetString("ettitle");

                        EquipmentType equipmentType = new EquipmentType()
                        {
                            Id = equipmentTypeId,
                            Title = Titles,
                        };

                        result.Add(new Order()
                        {
                            Id =id,
                            Title = title,
                            Model = model,
                            OrderDate = orderdate,
                            Quantity = quantity,
                            OrderStatus = donenotdone,
                            EquipmentType = equipmentType,
                            EquipmentTypeId = equipmentTypeId,
                        });
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            connection.CloseConnection();
            return result;
        }
        static OrderDB db;
        public static OrderDB GetDB()
        {
            if (db == null)
                db = new OrderDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Update(Order order)
        {
            bool result = false;
            if(connection == null)
                return result;
            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Order` set `Title`=@Title , `Model`=@Model , `OrderDate`=@OrderDate , `Quantity`=@Quantity , `OrderStatus`=@OrderStatus , `EquipmentTypeId`=@EquipmentTypeId where `Id` = {order.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", order.Title));
                mc.Parameters.Add(new MySqlParameter("Model", order.Model));
                mc.Parameters.Add(new MySqlParameter("OrderDate", order.OrderDate));
                mc.Parameters.Add(new MySqlParameter("Quantity", order.Quantity));
                mc.Parameters.Add(new MySqlParameter("OrderStatus", order.OrderStatus));
                mc.Parameters.Add(new MySqlParameter("EquipmentTypeId", order.EquipmentTypeId));
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            connection.CloseConnection( );
            return result;
        }

        internal bool Remove(Order selectedOrder)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Order` where `id` = {selectedOrder.Id}");
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
