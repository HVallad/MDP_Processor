using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MDPProcessor.Properties;

namespace MDPProcessor.Core
{
    public class PythonEngine
    {
        public static void GeneratePolicy(string transitionPath, string rewardsPath, string outputPath, float discountFactor, int iterations)
        {
            if (File.Exists("script.py"))
            {
                File.Delete("script.py");
            }
            File.WriteAllBytes("script.py", Resources.template_script);
            string pythonCommand = $"/C py script.py -tf ^\"{transitionPath}^\" -rf ^\"{rewardsPath}^\" -of ^\"{outputPath}^\" -di ^\"{discountFactor}^\" -it ^\"{iterations}^\"";

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = pythonCommand;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

        }
    }
}
