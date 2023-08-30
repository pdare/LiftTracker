using LiftTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
        public static readonly DependencyProperty WeightProperty = DependencyProperty.Register("Weight", typeof(float), typeof(LiftBlock), new PropertyMetadata(0.0f));
        public int currentSetsNum = 0;
        string currentSets = "";
        public List<LiftSet> liftSets = new List<LiftSet>();
        private double bgHeight = 0.0d;
        private double baseHeight = 120.0d;
        private double heightMod = 65.0d;

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

        public int NumberOfReps
        {
            get { return (int)GetValue(NumberOfRepsProperty);}
            set { SetValue(NumberOfRepsProperty, value);}
        }

        public float Weight
        {
            get { return (float)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        public LiftBlock(string liftname)
        {
            
            var viewModel = new LiftBlockVM();
            viewModel.LiftNameVM = liftname;
            DataContext = viewModel;
            
            InitializeComponent();
            bgHeight = grid.Height - 25;
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
            this.Height = baseHeight + (heightMod * liftSets.Count);
            grid.Height = baseHeight + (heightMod * liftSets.Count);
            bgTxtBlck.Height = (baseHeight - 25) + (heightMod * liftSets.Count);
            Set.UpdateCurrentTxtBx();
        }

        
        private void RemoveLiftBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);
        }

        public void updateHeight()
        {
            this.Height = baseHeight - (heightMod * liftSets.Count);
            grid.Height = baseHeight - (heightMod * liftSets.Count);
        }
        
    }
}
