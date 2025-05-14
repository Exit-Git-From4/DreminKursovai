using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.Model;
using DreminKursovai.View;
using DreminKursovai.DB;


using System.Collections.ObjectModel;
using System.Windows;

namespace DreminKursovai.VMTools
{
    internal class WindowEquipmetTypeOptionsMvvm : BaseVM
    {
        private EquipmentTypeOptions selectedEquipmentTypeOptions;
        private ObservableCollection<EquipmentTypeOptions> equipmentTypeOptions = new();
        private string search;

        public ObservableCollection<EquipmentTypeOptions> EquipmentTypeOptions
        {
            get => equipmentTypeOptions;
            set
            {
                equipmentTypeOptions = value;
                Signal();
            }
        }
        public EquipmentTypeOptions SelectedEquipmentTypeOptions
        {
            get => selectedEquipmentTypeOptions;
            set
            {
                selectedEquipmentTypeOptions = value;
                Signal();
            }
        }

        public CommandMvvm Add {  get; set; }
        public CommandMvvm Remove { get; set; }
        public CommandMvvm Edit { get; set; }
        public string Search
        {
            get => search;
            set
            {
                search = value;
                SearchEquipmentTypeOptions(search);
            }
        }

        public WindowEquipmetTypeOptionsMvvm()
        {
            SelectAll();
            Edit = new CommandMvvm(() =>
            {
                EquipmentTypeOptions equipment = SelectedEquipmentTypeOptions;
                new ListEquipmentTypeWindow().ShowDialog();
                SelectAll();
            }, () => SelectedEquipmentTypeOptions != null);

            //Remove = new CommandMvvm(() =>
            //{
            //    if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //        EquipmentDB.GetDB().Remove(SelectedEquipmentTypeOptions);
            //    SelectAll();
            //}, () => SelectedEquipmentTypeOptions != null);

            Add = new CommandMvvm(() =>
            {
                EquipmentTypeOptions equipment = new EquipmentTypeOptions();
                new ListEquipmentTypeWindow().ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            equipmentTypeOptions = new ObservableCollection<EquipmentTypeOptions>(EquipmentTypeOptionsDB.GetDB().SelectAll());
        }

        private void SearchEquipmentTypeOptions(string search)
        {
            equipmentTypeOptions = new ObservableCollection<EquipmentTypeOptions>(EquipmentTypeOptionsDB.GetDB().SearchEquipmentTypeOptions(search));
        }
    }
}
