using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftTracker.Client
{
    internal class CallClient
    {
        public string CheckConnection()
        {
            Runtime.PythonDLL = @"C:\Python310\python312.dll";
            PythonEngine.Initialize();
            //System.Environment.SetEnvironmentVariable("PYTHONPATH", @"Client\ClientMain.py");
            string temp = "";
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\insai\OneDrive\Documents\Programming\LiftTracker");
                var getLiftScript = Py.Import("Client.ClientMain");
                var script_result = getLiftScript.InvokeMethod("check_connection");
                temp = script_result.ToString();
            }
            PythonEngine.Shutdown();
            if (temp != "valid connection") { return "Could not connect to server"; }

            return "Server Connection Established";
        }

        public bool SaveWorkout(int user_id, string data_for_server)
        {
            string client_response;
            Runtime.PythonDLL = @"C:\Python310\python312.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                var getLiftScript = Py.Import("Client.ClientMain");
                var script_result = getLiftScript.InvokeMethod("send_lifts", new PyObject[] { user_id.ToPython(), data_for_server.ToPython() });
                client_response = script_result.ToString();
            }
            PythonEngine.Shutdown();
            if (client_response.Contains("failed to save workout")) { return false; }

            return true;
        }

        public string GetLift(int user_id, string date, int set_num, string lift_name)
        {
            Runtime.PythonDLL = @"C:\Python310\python312.dll";
            PythonEngine.Initialize();
            string temp;
            using (Py.GIL())
            {
                var getLiftScript = Py.Import("Client.ClientMain");
                var script_result = getLiftScript.InvokeMethod("get_lift", new PyObject[] { user_id.ToPython(), date.ToPython(), set_num.ToPython(), lift_name.ToPython() });
                temp = script_result.ToString();
            }
            PythonEngine.Shutdown();
            return temp;
        }
    }
}
