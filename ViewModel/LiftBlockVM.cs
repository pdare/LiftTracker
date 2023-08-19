using LiftTracker.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LiftTracker.ViewModel
{
    class LiftBlockVM : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _liftNameVM;
        public string LiftNameVM { get => _liftNameVM; set => SetProperty(ref _liftNameVM, value); }
        public int CurrentSetNum { get; set; }
        public int MaxSetNum { get; set; }

        public List<LiftSet> LiftSets { get; set; }

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
