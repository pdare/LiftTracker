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
using LiftTracker.Model;

namespace LiftTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Lift> lifts;
        List<LiftBlock> liftBlocks = new List<LiftBlock>();
        List<WorkoutTemplate> workoutTemplates;
        
        public MainWindow()
        {
            DateTime currentDate = DateTime.Now;
            

            InitializeComponent();
            CurrentDateLbl.Content = currentDate;

            //read lifts json and fill the lift selection combo box
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\JSON\lifts.json");
            string sFilePath = System.IO.Path.GetFullPath(sFile);
            
            var parseJson = new ReadJson(sFilePath);
            lifts = parseJson.UseUserDefinedJsonObj();

            foreach (var lift in lifts)
            {
                LiftsCBox.Items.Add(lift.name);
            }

            LiftsCBox.SelectedIndex = 0;

            //read workout template json and fill the workout selection combo box
            sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\JSON\workout_templates.json");
            sFilePath = System.IO.Path.GetFullPath(sFile);

            parseJson = new ReadJson(sFilePath);
            workoutTemplates = parseJson.UseWorkoutTemplateJsonObj();

            foreach (var workout in workoutTemplates)
            {
                WorkoutTemplatesCBox.Items.Add(workout.name);
            }

            WorkoutTemplatesCBox.SelectedIndex = 0;
        }

        private void AddLiftBtn_Click(object sender, RoutedEventArgs e)
        {
            string tempLiftName = "";
            if (LiftsCBox.SelectedValue != null ) { tempLiftName = LiftsCBox.SelectedValue.ToString(); }
            else { tempLiftName = "No lift name found"; }

            LiftBlock block = new LiftBlock(tempLiftName);

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
            //block.UpdateSetLbl();
        }

        private void UseTemplateBtn_Click(object sender, RoutedEventArgs e)
        {

            ucPanel.Children.Clear();
            
            string tempWorkoutName = "";
            if (WorkoutTemplatesCBox.SelectedValue != null ) { tempWorkoutName = WorkoutTemplatesCBox.SelectedValue.ToString(); }
            else { tempWorkoutName = "No workout found"; }
            
            foreach (var workout in workoutTemplates)
            {
                if (workout.name == tempWorkoutName)
                {
                    
                    foreach (var lift in workout.lifts)
                    {
                        LiftBlock block = new LiftBlock(lift);

                        foreach (var lft in lifts)
                        {
                            if (lft.name.Equals(block.LiftName))
                            {
                                block.NumberOfSets = lft.sets;
                                block.NumberOfReps = lft.reps;
                                block.Weight = lft.weight;
                                break;
                            }
                        }

                        liftBlocks.Add(block);
                        ucPanel.Children.Add(block);
                    }
                    break;
                }
            }
        }
    }
}
