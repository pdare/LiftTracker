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
using System.Text.Json;
using Python.Runtime;
using LiftTracker.Client;

namespace LiftTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<LiftTemplate> lifts;
        List<LiftBlock> liftBlocks = new List<LiftBlock>();
        List<WorkoutTemplate> workoutTemplates;
        
        public MainWindow()
        {
            DateTime currentDate = DateTime.Now;

            

            InitializeComponent();
            CurrentDateLbl.Content = currentDate.ToString("MM-dd-yyyy");

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

            CallClient callClient = new CallClient();
            TestLabel.Content = callClient.CheckConnection();

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
        }

        private void UseTemplateBtn_Click(object sender, RoutedEventArgs e)
        {

            ucPanel.Children.Clear();
            
            string tempWorkoutName = "";
            if (WorkoutTemplatesCBox.SelectedValue != null ) { tempWorkoutName = WorkoutTemplatesCBox.SelectedValue.ToString(); }
            else { tempWorkoutName = "No workout found"; }
            CurrentWorkoutTB.Text = tempWorkoutName;
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

        private void SaveWorkoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> workoutDic = new Dictionary<string, object>();
            workoutDic.Add("date", CurrentDateLbl.Content.ToString());
            workoutDic.Add("workout", CurrentWorkoutTB.Text.ToString());
            Dictionary<string, object> exercisesDic = new Dictionary<string, object>();

            List<Dictionary<string, int>> dicListReps = new List<Dictionary<string, int>>();
            List<Dictionary<string, float>> dicListWeight = new List<Dictionary<string, float>>();

            int iter = 1;
            foreach (var lifts in liftBlocks) 
            {
                Dictionary<string, int> repsDic = new Dictionary<string, int>();
                Dictionary<string, float> weightDic = new Dictionary<string, float>();
                for (int i = 0; i < lifts.liftSets.Count; i++)
                {
                    repsDic.Add(String.Format("set {0}", i+1), int.Parse(lifts.liftSets[i].CurrentRepsTxtBx.Text.ToString()));
                    weightDic.Add(String.Format("set {0}", i+1), float.Parse(lifts.liftSets[i].CurrentWeightTxtBx.Text.ToString()));
                }
                exercisesDic.Add("name", lifts.liftNameTxtBlck.Text.ToString());
                exercisesDic.Add("number of sets", lifts.currentSetsNum);
                exercisesDic.Add("reps", repsDic.ToDictionary(entry => entry.Key, entry => entry.Value));
                exercisesDic.Add("weight", weightDic.ToDictionary(entry => entry.Key, entry => entry.Value));
                workoutDic.Add(String.Format("exercise {0}", iter), exercisesDic.ToDictionary(entry => entry.Key, entry => entry.Value));
                exercisesDic.Clear();
                iter++;
            }
            string data = JsonSerializer.Serialize(workoutDic, new JsonSerializerOptions() { WriteIndented = true });
            string data_for_server = JsonSerializer.Serialize(workoutDic, new JsonSerializerOptions { WriteIndented = false });
            string filename = String.Format("Workout {0}", DateTime.Now.ToString("HH mm MM-dd-yyyy"));
            File.WriteAllText(filename, data);

            CallClient callClient = new CallClient();
            bool save_to_server = callClient.SaveWorkout(19752, data_for_server);
            if (save_to_server) { TestLabel.Content = "workout saved to server"; }
            else { TestLabel.Content = "failed to save to server"; }
        }

        private void GetLiftBtn_Click(object sender, RoutedEventArgs e)
        {
            CallClient callClient = new CallClient();
            string temp = callClient.GetLift(19752, "2024-05-04", 1, "Rack Pull");
            TestLabel.Content = temp;
        }

        private void GetWorkoutBtn_Click(object sender, RoutedEventArgs e)
        {
            CallClient callClient = new CallClient();
            string temp = callClient.GetWorkout(19752, "2024-05-10", "borscht");
            TestLabel.Content = temp;
        }

        private void RemoveLiftBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
