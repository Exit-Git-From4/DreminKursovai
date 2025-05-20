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
    public class WindowAddEdiEquipmetMvvm : BaseVM
    {
        private Equipment selectedEquipment;
        private ObservableCollection<Equipment> equipmentList = new();
        private ObservableCollection<EquipmentType> equipmentTypeList = new();
        private ObservableCollection<Manufacturer> manufacturerList = new();

        public ObservableCollection<Equipment> Equipments { get => equipmentList; set { equipmentList = value; Signal(); } }
        public ObservableCollection<EquipmentType> EquipmentTypes { get => equipmentTypeList; set { equipmentTypeList = value; Signal(); } }
        public ObservableCollection<Manufacturer> Manufacturers { get => manufacturerList; set { manufacturerList = value; Signal(); } }
        public Equipment SelectedEquipment{get => selectedEquipment;set{selectedEquipment = value;Signal();} }
        public CommandMvvm Save {  get; set; }

        public WindowAddEdiEquipmetMvvm(Equipment equipment)
        {
            SelectedEquipment = equipment;
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                equipment.EquipmentTypeId = equipment.EquipmentType.Id;
                equipment.ManufacturerId = equipment.Manufacturer.Id;
                if (SelectedEquipment.Id > 0)
                    EquipmentDB.GetDB().Update(SelectedEquipment);
                else
                    EquipmentDB.GetDB().Insert(SelectedEquipment);
                close();
            }, () =>
            SelectedEquipment != null &&
            !string.IsNullOrWhiteSpace(SelectedEquipment.Title) &&
            SelectedEquipment.Value !=null &&
            SelectedEquipment.ReleaseYear < DateTime.Now.Year &&
            SelectedEquipment.Manufacturer != null &&
            SelectedEquipment.EquipmentType != null);
        }

        private void SelectAll()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(ManufacturerDB.GetDB().SelectAll());
            EquipmentTypes =new ObservableCollection<EquipmentType>(EquipmentTypeDB.GetDB().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
