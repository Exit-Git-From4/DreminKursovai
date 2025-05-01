using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DreminKursovai.View
{
    /// <summary>
    /// Логика взаимодействия для WindowAddEditEquipment.xaml
    /// </summary>
    public partial class WindowAddEditEquipment : Window
    {
        public WindowAddEditEquipment(Model.Equipment book)
        {
            InitializeComponent();
        }
    }
}
