using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Util;

namespace Core.SaveLoadSystem
{
    public static class SaveLoad
    {
        private const string _fileName = ".insanity";

        public static void Save<T>(string key, T objectToSave)
        {
            string path = Application.persistentDataPath + "/Saved Gamedata/";
            Directory.CreateDirectory(path);
            BinaryFormatter formatter = new BinaryFormatter();

            using FileStream fileStream = new(path + key + _fileName, FileMode.Create);
            formatter.Serialize(fileStream, objectToSave);
        }

        public static T Load<T>(string key, T defaultVal)
        {
            string path = Application.persistentDataPath + "/Saved Gamedata/";
            BinaryFormatter formatter = new();
            T returnValue = default;
            string filePath = path + key + _fileName;

            if (File.Exists(filePath))
            {
                using FileStream fileStream = new(filePath, FileMode.Open);
                returnValue = (T)formatter.Deserialize(fileStream);
            }
            else
            {
                returnValue = defaultVal;
            }

            return returnValue;
        }

        public static bool HaveData(string key)
        {
            string path = Application.persistentDataPath + "/Saved Gamedata/" + key + _fileName;
            return File.Exists(path);
        }

        public static void DeleteKey(string key)
        {
            string path = Application.persistentDataPath + "/Saved Gamedata/" + key + _fileName;
            File.Delete(path);
        }

#if UNITY_EDITOR

        [MenuItem("GameData/DeleteGameData")]
        public static void PermanentlyDeleteGameData()
        {
            string path = Application.persistentDataPath + "/Saved Gamedata/";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            directoryInfo.Delete(true);
            Directory.CreateDirectory(path);
            "GameData deleted".Log();
        }

#endif
    }
}