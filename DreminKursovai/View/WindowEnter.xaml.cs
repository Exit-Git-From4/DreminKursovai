using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DreminKursovai.VMTools;

namespace DreminKursovai.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WindowEnter : Window
    {
        private const string password = "123";
        public WindowEnter()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb.Text == password)
            {
                WindowWithGoods windowWithgoods = new WindowWithGoods();
                windowWithgoods.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не правельный пароль");
            }

        }
    }
}