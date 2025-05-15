using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.DB;
using DreminKursovai.Model;

namespace DreminKursovai.VMTools
{
    public  class WindowAddEquipmentTypeMvvm : BaseVM
    {
        private EquipmentType selectedEquipmentType;
        private ObservableCollection<EquipmentType> equipmentTypes = new();

        public ObservableCollection<EquipmentType> EquipmentTypes { get => equipmentTypes; set { equipmentTypes = value; Signal(); } }
        public EquipmentType SelectedEquipmentType { get => selectedEquipmentType; set { selectedEquipmentType = value; Signal(); } }

        public CommandMvvm Save { get; set; }

        public WindowAddEquipmentTypeMvvm(EquipmentType equipmentType)
        {
            SelectedEquipmentType = equipmentType;
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                if (SelectedEquipmentType.Id > 0)
                    EquipmentTypeDB.GetDB().Update(SelectedEquipmentType);
                else
                    EquipmentTypeDB.GetDB().Insert(SelectedEquipmentType);
                close();
            }, () =>
            SelectedEquipmentType != null &&
            !string.IsNullOrWhiteSpace(SelectedEquipmentType.Title) &&
            SelectedEquipmentType.Title.Length <= 255
            );
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        private void SelectAll()
        {
            EquipmentTypes = new ObservableCollection<EquipmentType>(EquipmentTypeDB.GetDB().SelectAll());
        }
    }
}
