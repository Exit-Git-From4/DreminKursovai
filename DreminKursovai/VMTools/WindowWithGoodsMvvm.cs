using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DreminKursovai.DB;
using DreminKursovai.Model;
using DreminKursovai.View;

namespace DreminKursovai.VMTools
{
    internal class WindowWithGoodsMvvm : BaseVM
    {
        private Equipment selectedEquipment;
        private ObservableCollection<Equipment> equipments = new();
        private string search;

        public ObservableCollection<Equipment> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                Signal();
            }
        }
        public Equipment SelectedEquipment
        {
            get => selectedEquipment;
            set
            {
                selectedEquipment = value;
                Signal();
            }
        }
        public CommandMvvm Add { get; set; }
        public CommandMvvm Update { get; set; }
        public CommandMvvm Remove { get; set; }
        public CommandMvvm OpenListParametrsEquipment { get; set; }
        public CommandMvvm OpenListManufacturer {  get; set; }
        public CommandMvvm OpenListOrders { get; set; }
        public string Search
        {
            get => search;
            set
            {
                search = value;
                SearchEquipment(search);
            }
        }

        public WindowWithGoodsMvvm()
        {
            SelectAll();
            Update = new CommandMvvm(() =>
            {
                Equipment book = SelectedEquipment;
                new WindowAddEditEquipment(book).ShowDialog();
                SelectAll();
            }, () => SelectedEquipment != null);

            Remove = new CommandMvvm(() =>
            {
                if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    EquipmentDB.GetDb().Remove(SelectedEquipment);
                SelectAll();
            }, () => SelectedEquipment != null);

            Add = new CommandMvvm(() =>
            {
                Equipment book = new Equipment();
                new WindowAddEditEquipment(book).ShowDialog();
                SelectAll();
            }, () => true);

            OpenListParametrsEquipment = new CommandMvvm(() =>
            {
                new ParameterListWindow().ShowDialog();
                SelectAll();
            }, () => true);

            Update = new CommandMvvm(SelectAll, () => true);
        }

        private void SelectAll()
        {
            Equipments = new ObservableCollection<Equipment>(EquipmentDB.GetDb().SelectAll());
        }
        private void SearchEquipment(string search)
        {
            Equipments = new ObservableCollection<Equipment>(EquipmentDB.GetDb().SearchEquipment(search));
        }
    }
}
