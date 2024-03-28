using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(string fileName, T data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static T Load<T>(string fileName)
    {
        T data;
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = (T)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            data = default(T);
        }
        return data;
    }

    public static void Delete(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
