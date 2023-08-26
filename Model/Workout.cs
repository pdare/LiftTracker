using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftTracker.Model
{
    internal class Workout
    {
        public string date { get; set; }
        public string workout { get; set; }
        public List<LiftTemplate> exercises { get; set; }
    }
}
