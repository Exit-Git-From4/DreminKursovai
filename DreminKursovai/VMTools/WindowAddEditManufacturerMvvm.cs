using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.DB;
using DreminKursovai.Model;
using DreminKursovai.View;

namespace DreminKursovai.VMTools
{
    partial class WindowAddEditManufacturerMvvm : BaseVM
    {
        private Manufacturer selectedManufacturer;
        private ObservableCollection<Manufacturer> manufacturers = new();

        public ObservableCollection<Manufacturer> Manufacturers { get => manufacturers; set { manufacturers = value; Signal(); } }
        public Manufacturer SelectedManufacturer { get => selectedManufacturer; set { selectedManufacturer = value; Signal(); } }

        public CommandMvvm Save { get; set; }

        public WindowAddEditManufacturerMvvm(Manufacturer manufacturer)
        {
            SelectedManufacturer = manufacturer;
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                if (SelectedManufacturer.Id > 0)
                    ManufacturerDB.GetDB().Update(SelectedManufacturer);
                else
                    ManufacturerDB.GetDB().Insert(SelectedManufacturer);
                close();
            }, () =>
            SelectedManufacturer != null &&
            !string.IsNullOrWhiteSpace(SelectedManufacturer.Title) &&
            SelectedManufacturer.Title.Length <= 255 &&
            !string.IsNullOrWhiteSpace(SelectedManufacturer.Сountry) &&
            SelectedManufacturer.Сountry.Length <= 255
            );
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        private void SelectAll()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(ManufacturerDB.GetDB().SelectAll());
        }
        //internal void SetManufacturer(Manufacturer manufacturer)
        //{
        //    SelectedManufacturer = manufacturer;
        //    if (selectedManufacturer.Id == 0)
        //    {
        //        Signal(nameof(SelectedManufacturer));
        //    }
        //}
    }
}
