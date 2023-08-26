using LiftTracker.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LiftTracker.test
{
    class ReadJson
    {
        private readonly string _sampleJsonFilePath;

        public ReadJson(string sampleJsonFilePath)
        {
            _sampleJsonFilePath = sampleJsonFilePath;
        }

        public List<LiftTemplate> UseUserDefinedJsonObj()
        {
            //using StreamReader reader = new(_sampleJsonFilePath);
            var json = File.ReadAllText(_sampleJsonFilePath);
            List<LiftTemplate> lifts = JsonSerializer.Deserialize<List<LiftTemplate>>(json);
            //var lift = JsonSerializer.Deserialize<Lift>(json);
            return lifts;
        }

        public List<WorkoutTemplate> UseWorkoutTemplateJsonObj()
        {
            //using StreamReader reader = new(_sampleJsonFilePath);
            var json = File.ReadAllText(_sampleJsonFilePath);
            List<WorkoutTemplate> workoutTemplates = JsonSerializer.Deserialize<List<WorkoutTemplate>>(json);
            //var lift = JsonSerializer.Deserialize<Lift>(json);
            return workoutTemplates;
        }

        public List<Workout> UseWorkoutJsonObj()
        {
            var json = File.ReadAllText(_sampleJsonFilePath);
            List<Workout> workouts = JsonSerializer.Deserialize<List<Workout>>(json);
            return workouts;
        }
    }
}
