using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.Model;
using MySqlConnector;

namespace DreminKursovai.DB
{
    public class OrderEquipmentDB
    {
        DBConnection connection;
        private OrderEquipmentDB(DBConnection connection)
        {
            this.connection = connection;
        }
        public List<OrderEquipment> SearchOrder(string search)
        {
            List<OrderEquipment> orderEquipments  new();
            List<Order> order = new();
            List<Equipment> equip = new();

            string JJ = $"SELECT * FROM OrderEquipment oe JOIN `Order` o ON oe.OrderId = o.Id JOIN Equipment e ON oe.EquipmentId = e.Id";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(JJ))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var orderEquipment = new OrderEquipment();
                            orderEquipment.OrderId = dr.GetInt32("OrderId");
                            orderEquipment.EquipmentId = dr.GetInt32("EquipmentId");

                            var orders = order.FirstOrDefault(s => s.Id == orderEquipment.OrderId);
                            if (orders == null)
                            {
                                orders = new Order();
                                orders.Id = orderEquipment.OrderId;
                                orders.OrderDate = dr.GetDateOnly("OrderDate");
                                order.Add(orders);
                            }

                            var equips = equip.FirstOrDefault(c => c.Id == orderEquipment.EquipmentId);
                            if (equips == null)
                            {
                                equips = new Equipment();
                                equips.Id = orderEquipment.EquipmentId;
                                equips.Title = dr.GetString("Title");
                                equip.Add(equips);
                            }
                            orderEquipment.Order = orders;
                            orderEquipment.Equipment = equips;
                            orderEquipment.Add(orderEquipment);
                        }
                    }
                    connection.CloseConnection();
                }
            }
            return equipmentTypeOptions;
        }
    }
}
