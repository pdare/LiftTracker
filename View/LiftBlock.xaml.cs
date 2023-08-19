﻿using LiftTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiftTracker.View
{
    /// <summary>
    /// Interaction logic for LiftBlock.xaml
    /// </summary>
    public partial class LiftBlock : UserControl
    {
        public static readonly DependencyProperty LiftNameProperty = DependencyProperty.Register("LiftName", typeof(string), typeof(LiftBlock), new PropertyMetadata(string.Empty));
        //public static readonly DependencyProperty NumberOfSetsProperty = DependencyProperty.Register("NumberOfSets", typeof(string), typeof (LiftBlock), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty NumberOfSetsProperty = DependencyProperty.Register("NumberOfSets", typeof(int), typeof(LiftBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty NumberOfRepsProperty = DependencyProperty.Register("NumberOfReps", typeof(int), typeof(LiftBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty WeightProperty = DependencyProperty.Register("Weight", typeof(int), typeof(LiftBlock), new PropertyMetadata(0));
        int currentSetsNum = 0;
        string currentSets = "";
        List<LiftSet> liftSets = new List<LiftSet>();

        public string LiftName
        {
            get { return (string)GetValue(LiftNameProperty); }
            set { SetValue(LiftNameProperty, value); }
        }

        public int NumberOfSets
        {
            get { return (int)GetValue(NumberOfSetsProperty); }
            set { SetValue(NumberOfSetsProperty, value); }
        }

        /*
        public string NumberOfSets
        {
            get { return (string)GetValue(NumberOfSetsProperty); }
            set { SetValue(NumberOfSetsProperty, value); }
        } 
         
         */


        public int NumberOfReps
        {
            get { return (int)GetValue(NumberOfRepsProperty);}
            set { SetValue(NumberOfRepsProperty, value);}
        }

        public int Weight
        {
            get { return (int)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        public LiftBlock()
        {
            
            var viewModel = new LiftBlockVM();
            viewModel.LiftNameVM = "test";
            DataContext = viewModel;
            InitializeComponent();
            currentSets = String.Format("{0}/{1}", currentSetsNum, NumberOfSets);
            SetTrackLbl.Content = currentSets;
        }

        public void UpdateSetLbl()
        {
            currentSets = String.Format("{0}/{1}", currentSetsNum, NumberOfSets);
            SetTrackLbl.Content = currentSets;
        }

        private void AddSetBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSetsNum++;
            currentSets = String.Format("{0}/{1}", currentSetsNum, NumberOfSets);
            SetTrackLbl.Content = currentSets;
            LiftSet Set = new LiftSet();
            Set.LiftReps = NumberOfReps;
            Set.LiftWeight = Weight;
            liftSets.Add(Set);
            ucStack.Children.Add(Set);
            Set.UpdateCurrentTxtBx();
        }
    }
}
