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
        private int _currentSetNum;
        private int _maxSetNum;

        public string LiftNameVM { get => _liftNameVM; set => SetProperty(ref _liftNameVM, value); }
        public int CurrentSetNum { get => _currentSetNum; set => SetProperty(ref _currentSetNum, value); }
        public int MaxSetNum { get => _maxSetNum; set=> SetProperty(ref _maxSetNum, value); }

        public List<LiftSet> LiftSets { get; set; }

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
