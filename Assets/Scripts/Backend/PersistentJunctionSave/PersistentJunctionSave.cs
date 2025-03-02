using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Util;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Assets.Scripts.Backend.PersistentJunctionSave
{
    public class PersistentJunctionSave
    {
        public static string JunctionSaveFileName = "rankings.json";

        public static void SaveResult(
            InputParameters input, 
            ResultTrafficSimulation result
        ) {
            var jointResult = new JointInputResult(input, result);
            string json = JsonConvert.SerializeObject(jointResult, Formatting.None);
            Debug.Log($"Saving to {JunctionSaveFileName} value: {json}");
            PersistentFileManager.AppendToFile(JunctionSaveFileName, json);
        }

        public static bool LoadAllResults(
            out (InputParameters, ResultTrafficSimulation)[] results
        ) {
            var jointResults = new List<JointInputResult>();
            string loadedFile;
            var loadedFileSuccess = PersistentFileManager.LoadFromFile(
                JunctionSaveFileName, 
                out loadedFile
            );

            if (!loadedFileSuccess) 
            {
                results = new (InputParameters, ResultTrafficSimulation)[0];
                return false;
            }

            using (var reader = new StringReader(loadedFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var obj = JsonConvert
                            .DeserializeObject<JointInputResult>(line);
                        if (obj == null)
                        {
                            Debug.Log("Found null object whilst deserializing");
                            continue;
                        }
                        
                        jointResults.Add(obj);
                    }
                    catch (JsonException e)
                    {
                        Debug.Log($"Skipping invalid JSON object with err {e.Message}");
                    }
                }
            }

            results = new (InputParameters, ResultTrafficSimulation)[jointResults.Count];

            for (int i = 0; i < jointResults.Count; ++i)
            {
                results[i].Item1 = jointResults[i].Input;
                results[i].Item2 = jointResults[i].Result;
            }

            return true;
        }

        /// <summary>
        /// Load junction configs by efficiency
        /// </summary>
        /// <param name="w_1">Coefficient of AverageWait</param>
        /// <param name="w_2">Coefficient of MaxWait</param>
        /// <param name="w_3">Coefficient of MaxQueueLength</param>
        /// <param name="result">configs ordered in descending order of efficiency</param>
        /// <returns></returns>
        public static bool LoadByEfficiency(
            double w_1,
            double w_2,
            double w_3,
            out List<(double, (InputParameters, ResultTrafficSimulation))> result
        ) {
            result = new List<(double, (InputParameters, ResultTrafficSimulation))>();
            (InputParameters, ResultTrafficSimulation)[] results;
            var success = LoadAllResults(out results);

            if (!success) return false;

            foreach (var x in results)
            {
                double efficiency = 0;
                foreach (var dir in Enum.GetValues(typeof(CardinalDirection)))
                {
                    efficiency += 
                        x.Item2.EfficiencyWithDirection((CardinalDirection) dir, w_1, w_2, w_3);
                }
                result.Add((efficiency, x));
            }

            result.Sort((a, b) => b.Item1.CompareTo(a.Item1));
            return true;
        }

        [Serializable]
        class JointInputResult
        {
            
            public InputParameters Input {get; }
            
            public ResultTrafficSimulation Result { get; }

            public JointInputResult(
                InputParameters input, 
                ResultTrafficSimulation result
            ) {
                Input = input;
                Result = result;
            }
        }
    }

    public class PersistentFileManager
    {
        public static bool WriteToFile(string fileName, string fileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(fullPath, fileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool AppendToFile(string fileName, string fileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.AppendAllText(fullPath, fileContents + "\n");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to append to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
    }
}