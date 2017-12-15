using System;
using System.Diagnostics;

namespace TraceRoute.API
{
    /// <summary>
    /// Static class to abstract away the complexity of making Bash Command
    /// calls from ASP.NET Core 2.0.
    /// </summary>
    public static class BashHelper
    {
        /// <summary>
        /// Attempt to run the specified Bash command.
        /// </summary>
        /// <returns>Standard output returned from command.</returns>
        /// <param name="cmd">Bash command with arguments.</param>
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
