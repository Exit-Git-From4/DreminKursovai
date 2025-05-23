﻿using System;
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
        public CommandMvvm OpenEquipmrntTypeWindow { get; set; }
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
                Equipment equipment = SelectedEquipment;
                new WindowAddEditEquipment(equipment).ShowDialog();
                SelectAll();
            }, () => SelectedEquipment != null);

            Remove = new CommandMvvm(() =>
            {
                if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    EquipmentDB.GetDB().Remove(SelectedEquipment);
                SelectAll();
            }, () => SelectedEquipment != null);

            Add = new CommandMvvm(() =>
            {
                Equipment equipment = new Equipment();
                new WindowAddEditEquipment(equipment).ShowDialog();
                SelectAll();
            }, () => true);

            OpenEquipmrntTypeWindow = new CommandMvvm(() =>
            {
                new ListEquipmentTypeWindow().ShowDialog();
                SelectAll();
            }, () => true);
            OpenListManufacturer = new CommandMvvm(() =>
            {
                Equipment equipment = new Equipment();
                new ListEquipmentTypeWindow().ShowDialog();
                SelectAll();
            }, () => true);
            OpenListManufacturer = new CommandMvvm(() =>
            {
                Equipment equipment = new Equipment();
                new ListManufacturerWindow().ShowDialog();
                SelectAll();
            }, () => true);
            OpenListOrders = new CommandMvvm(() =>
            {
                Equipment equipment = new Equipment();
                new OrderList().ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            Equipments = new ObservableCollection<Equipment>(EquipmentDB.GetDB().SelectAll());
        }
        private void SearchEquipment(string search)
        {
            Equipments = new ObservableCollection<Equipment>(EquipmentDB.GetDB().SearchEquipment(search));
        }
    }
}
