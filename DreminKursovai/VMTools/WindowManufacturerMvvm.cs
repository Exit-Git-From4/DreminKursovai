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
    internal class WindowManufacturerMvvm : BaseVM
    {
        private Manufacturer selectedManufacturer;
        private ObservableCollection<Manufacturer> manufacturers;

        public ObservableCollection<Manufacturer> Manufacturers
        { 
            get => manufacturers;
            set
            {
                manufacturers = value;
                Signal();
            }
        }

        public Manufacturer SelectedManufacturer 
        {
            get => selectedManufacturer;
            set
            {
                selectedManufacturer = value;
                Signal();
            }
        }

        public CommandMvvm Add {  get; set; }
        public CommandMvvm Remove { get; set; }
        public CommandMvvm Edit { get; set; }

        public WindowManufacturerMvvm()
        {
            SelectAll();
            Edit = new CommandMvvm(() =>
            {
                Manufacturer equipment = SelectedManufacturer;
                new ListManufacturerWindow().ShowDialog();
                SelectAll();
            }, () => SelectedManufacturer != null);

            Remove = new CommandMvvm(() =>
            {
                if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    ManufacturerDB.GetDB().Remove(SelectedManufacturer);
                SelectAll();
            }, () => SelectedManufacturer != null);

            Add = new CommandMvvm(() =>
            {
                Manufacturer equipment = new Manufacturer();
                new ListManufacturerWindow().ShowDialog();
                SelectAll();
            }, () => true);
        }
        private void SelectAll()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(ManufacturerDB.GetDB().SelectAll());
        }
    }
}
