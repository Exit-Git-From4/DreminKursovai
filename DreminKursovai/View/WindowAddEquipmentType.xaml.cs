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
using DreminKursovai.DB;
using DreminKursovai.Model;
using DreminKursovai.VMTools;

namespace DreminKursovai.View
{
    /// <summary>
    /// Логика взаимодействия для WindowAddEquipmentType.xaml
    /// </summary>
    public partial class WindowAddEquipmentType : Window
    {
        public WindowAddEquipmentType(EquipmentType equipmentType)
        {
            InitializeComponent();
            var vm = new WindowAddEquipmentTypeMvvm(equipmentType);
            DataContext = vm;
            vm.SetClose(Close);
        }
    }
}
