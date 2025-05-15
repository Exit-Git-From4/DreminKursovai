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
        private EquipmentType selectedEquipmentType;
        private ObservableCollection<EquipmentType> equipmentType = new();
        private Options selectOptions;
        private ObservableCollection<Options> otions = new();

        private string search;

        public ObservableCollection<EquipmentType> EquipmentType
        {
            get => equipmentType;
            set
            {
                equipmentType = value;
                Signal();
            }
        }
        public EquipmentType SelectedEquipmentType
        {
            get => selectedEquipmentType;
            set
            {
                selectedEquipmentType = value;
                Signal();
            }
        }
        public ObservableCollection<Options> Options
        {
            get => otions;
            set
            {
                otions = value;
                Signal();
            }
        }
        public Options SelectedEquipmentType
        {
            get => selectedEquipmentType;
            set
            {
                selectedEquipmentType = value;
                Signal();
            }
        }
        public CommandMvvm Add1 {  get; set; }
        public CommandMvvm Remove1 { get; set; }
        public CommandMvvm Remove2 { get; set; }
        public CommandMvvm Add2 { get; set; }
        

        public WindowEquipmetTypeOptionsMvvm()
        {
            SelectAll();
            Add2 = new CommandMvvm(() =>
            {
                EquipmentTypeOptions equipment = new EquipmentTypeOptions();
                new ListEquipmentTypeWindow().ShowDialog();
                SelectAll();
            }, () => true); ;

            Remove1 = new CommandMvvm(() =>
            {
                if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    EquipmentTypeDB.GetDB().Remove(SelectedEquipmentType);
                SelectAll();
            }, () => SelectedEquipmentTypeOptions != null);

            Remove1 = new CommandMvvm(() =>
            {
                if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    OptionsDB.GetDB().Remove(SelectedOptions);
                SelectAll();
            }, () => SelectedEquipmentTypeOptions != null);

            Add1 = new CommandMvvm(() =>
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
        private void SearchEquipmentType(string search)
        {
            equipmentTypeOptions = new ObservableCollection<EquipmentTypeOptions>(EquipmentTypeOptionsDB.GetDB().SearchEquipmentTypeOptions(search));
        }
       
    }
}
