using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Assets.Scripts;

public class XMLParser {

    public static string fileName = "SARSA.xml";
    //public static string fileName = "QLEARNING.xml";

    //Serialize Classifier
    public static void Searialize(List<QValue> classifier)
    {
        using (FileStream fileStream = File.Open(fileName, FileMode.Create)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, classifier);
            fileStream.Close();
        }
    }

    //Deserialize Classifier
    public static List<QValue> DeSerialize()
    {
        List<QValue> qValues = null;
        if (!File.Exists(fileName)) return new List<QValue>();
        using (FileStream fileStream = File.Open(fileName, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            qValues = (List<QValue>)formatter.Deserialize(fileStream);
        }
        return qValues;
    }

}
