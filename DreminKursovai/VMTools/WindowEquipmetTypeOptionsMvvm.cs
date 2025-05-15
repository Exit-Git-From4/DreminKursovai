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
        private ObservableCollection<EquipmentType> equipmentType;

        private Options selectOptions;
        private ObservableCollection<Options> options;

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
            get => options;
            set
            {
                options = value;
                Signal();
            }
        }
        public Options SelectedOptions
        {
            get => selectOptions;
            set
            {
                selectOptions = value;
                Signal();
            }
        }
        public CommandMvvm AddEquipmetType {  get; set; }
        public CommandMvvm RemoveEquipmetType { get; set; }
        public CommandMvvm RemoveOptions { get; set; }
        public CommandMvvm AddOptions { get; set; }
        

        public WindowEquipmetTypeOptionsMvvm()
        {
            SelectAll();
            AddEquipmetType = new CommandMvvm(() =>
            {
                EquipmentType equipment = new EquipmentType();
                new WindowAddEquipmentType().ShowDialog();
                SelectAll();
            }, () => true); ;

            //RemoveEquipmetType = new CommandMvvm(() =>
            //{
            //    if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //        EquipmentTypeDB.GetDB().Remove(SelectedEquipmentType);
            //    SelectAll();
            //}, () => SelectedEquipmentType != null);

            //RemoveOptions = new CommandMvvm(() =>
            //{
            //    if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //        OptionsDB.GetDB().Remove(SelectedOptions);
            //    SelectAll();
            //}, () => SelectedOptions != null);

            AddOptions = new CommandMvvm(() =>
            {
                EquipmentTypeOptions equipment = new EquipmentTypeOptions();
                new ListEquipmentTypeWindow().ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            EquipmentType = new ObservableCollection<EquipmentTypeOptions>(EquipmentTypeDB.GetDBb().SelectAll());
            Options = new ObservableCollection<Options>(OptionsDB.GetDb().SelectAll());

        }
    }
}
