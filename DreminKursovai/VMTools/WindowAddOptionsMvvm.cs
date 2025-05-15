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
    public class WindowAddOptionsMvvm : BaseVM
    {
        private Options selectedOptions;
        private ObservableCollection<Options> options = new();

        public ObservableCollection<Options> EquipmentTypes { get => options; set { options = value; Signal(); } }
        public Options SelectedOptions { get => selectedOptions; set { selectedOptions = value; Signal(); } }

        public CommandMvvm Save { get; set; }

        public WindowAddOptionsMvvm(Options options)
        {
            SelectedOptions = options;
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                if (SelectedOptions.Id > 0)
                    OptionsDB.GetDb().Update(SelectedOptions);
                else
                    OptionsDB.GetDb().Insert(SelectedOptions);
                close();
            }, () =>
            SelectedOptions != null &&
            !string.IsNullOrWhiteSpace(SelectedOptions.Title) &&
            SelectedOptions.Title.Length <= 255
            );
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        private void SelectAll()
        {
            EquipmentTypes = new ObservableCollection<Options>(OptionsDB.GetDb().SelectAll());
        }
    }
}
