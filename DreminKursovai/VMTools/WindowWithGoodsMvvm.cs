using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreminKursovai.Model;

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
        public CommandMvvm UpdateBook { get; set; }
        public CommandMvvm RemoveBook { get; set; }
        public CommandMvvm AddBook { get; set; }
        public CommandMvvm OpenListAuthor { get; set; }
        public CommandMvvm Update { get; set; }
        //public string Search
        //{
        //    get => search;
        //    set
        //    {
        //        search = value;
        //        SearchBook(search);
        //    }
        //}

        //    public WindowWithGoodsMvvm()
        //    {
        //        SelectAll(); 
        //        UpdateBook = new CommandMvvm(() =>
        //        {
        //            Equipment book = SelectedBook;
        //            new WindowAddEditBook(book).ShowDialog();
        //            SelectAll();
        //        }, () => SelectedBook != null);

        //        RemoveBook = new CommandMvvm(() =>
        //        {
        //            if(MessageBox.Show("ТОЧНО?????","Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //                DBBook.GetDb().Remove(SelectedBook);
        //            SelectAll();
        //        }, () => SelectedBook != null);

        //        AddBook = new CommandMvvm(() =>
        //        {
        //            Equipment book = new Equipment();
        //            new WindowAddEditBook(book).ShowDialog();
        //            SelectAll();
        //        }, () => true);

        //        OpenListAuthor = new CommandMvvm(() => 
        //        {
        //            new WindowListAuthor().ShowDialog();
        //            SelectAll();
        //        }, () => true);

        //        Update = new CommandMvvm(SelectAll, () => true);
        //    }

        //    private void SelectAll()
        //    {
        //        Books = new ObservableCollection<Equipment>(DBBook.GetDb().SelectAll());
        //    }
        //    private void SearchBook(string search)
        //    {
        //        Books = new ObservableCollection<Equipment>(DBBook.GetDb().SearchBook(search));
        //    }
    }
}
