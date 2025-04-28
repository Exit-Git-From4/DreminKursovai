using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreminKursovai.Model
{
    //Заказ
    public class Order
    {
        public int Id { get; set; }
        public DateOnly OrderDate { get; set; }
    }
}
