using LiftTracker.test;
using System;
using System.Collections.Generic;
using System.IO;
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
using LiftTracker.View;

namespace LiftTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Lift> lifts;
        List<LiftBlock> liftBlocks = new List<LiftBlock>();
        
        public MainWindow()
        {
            InitializeComponent();
            
            
            var parseJson = new ReadJson("C:\\Users\\insai\\OneDrive\\Documents\\Programming\\LiftTracker\\test\\testjson.json");
            lifts = parseJson.UseUserDefinedJsonObj();

            foreach (var lift in lifts)
            {
                LiftsCBox.Items.Add(lift.name);
            }

            LiftsCBox.SelectedIndex = 0;
        }

        private void AddLiftBtn_Click(object sender, RoutedEventArgs e)
        {

            LiftBlock block = new LiftBlock();
            if (LiftsCBox.SelectedValue != null ) { block.LiftName = LiftsCBox.SelectedValue.ToString(); }
            else { block.LiftName = "No lift name found"; }
            
            foreach (var lift in lifts)
            {
                if (lift.name.Equals(block.LiftName))
                {
                    block.NumberOfSets = lift.sets;
                    block.NumberOfReps = lift.reps;
                    block.Weight = lift.weight;
                }
            }

            liftBlocks.Add(block);
            ucPanel.Children.Add(block);
            block.UpdateSetLbl();
        }
    }
}
