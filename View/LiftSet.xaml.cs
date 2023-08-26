using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiftTracker.View
{
    /// <summary>
    /// Interaction logic for LiftSet.xaml
    /// </summary>
    public partial class LiftSet : UserControl
    {
        public static readonly DependencyProperty LiftWeightProperty = DependencyProperty.Register("LiftWeight", typeof(float), typeof(LiftSet), new PropertyMetadata(0.0f));
        public static readonly DependencyProperty LiftRepProperty = DependencyProperty.Register("LiftReps", typeof(int), typeof(LiftSet), new PropertyMetadata(0));

        public float LiftWeight
        {
            get { return (float)GetValue(LiftWeightProperty); }
            set { SetValue(LiftWeightProperty, value);}
        }

        public int LiftReps
        {
            get { return (int)GetValue(LiftRepProperty); }
            set { SetValue (LiftRepProperty, value);}
        }

        public LiftSet()
        {
            InitializeComponent();
            CurrentRepsTxtBx.Text = LiftReps.ToString();
            CurrentWeightTxtBx.Text = LiftWeight.ToString();
        }

        public void UpdateCurrentTxtBx()
        {
            CurrentRepsTxtBx.Text = LiftReps.ToString();
            CurrentWeightTxtBx.Text = LiftWeight.ToString();
        }

        private void CurrentRepsTxtBx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void CurrentRepsTxtBx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back) 
            {
                e.Handled = false;
            }
            else
            { e.Handled = true; }
        }

        private void CurrentWeightTxtBx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9..]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void CurrentWeightTxtBx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                e.Handled = false;
            }
            else
            { e.Handled = true; }
        }
    }
}
