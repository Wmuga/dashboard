using System;
using System.IO;

namespace Dashboard
{
    public static class EnvParser
    {
        public static void Load(string filename)
        {
            if (!File.Exists(filename)) return;

            foreach (var line in File.ReadLines(filename))
            {
                string[] env_var = line.Split('=');
                if (env_var.Length!=2) continue;
                Environment.SetEnvironmentVariable(env_var[0],env_var[1]);
            }
        }
    }
}