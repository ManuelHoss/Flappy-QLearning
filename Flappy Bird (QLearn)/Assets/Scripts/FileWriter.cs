using System.Collections.Generic;
using System.IO;

public static class FileWriter
{
    private static string PATH = "stats" + Path.DirectorySeparatorChar;
    private static string FILE_ENDING = ".txt";

    public static void append(string txt, string fileName)
    {
        string file = PATH + fileName + FILE_ENDING;
        if (!File.Exists(file))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine(fileName);
            }
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = File.AppendText(file))
        {
            sw.WriteLine(txt);
        }
    }

    public static void clearFile(string fileName)
    {
        if (!Directory.Exists(PATH))
        {
            Directory.CreateDirectory(PATH);
        }

        string file = PATH + fileName + FILE_ENDING;
        if (File.Exists(file))
        {
            File.Delete(file);
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine(fileName);
            }
        }
    }

    public static void WriteTable(Dictionary<State, Dictionary<BirdAI.Action, double>> qLearningTable, string fileName)
    {
        string file = PATH + fileName + FILE_ENDING;
        // Create a file to write to.
        using (StreamWriter sw = File.CreateText(file))
        {
            sw.WriteLine(fileName);
            foreach (KeyValuePair<State, Dictionary<BirdAI.Action, double>> entry in qLearningTable)
            {
                foreach (KeyValuePair<BirdAI.Action, double> value in entry.Value)
                {
                    sw.WriteLine(entry.Key.ToString() + ": " + value.Key.ToString() + " = " + value.Value);
                }
            }
        }
    }
}
