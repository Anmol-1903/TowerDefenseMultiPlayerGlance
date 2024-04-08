using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NetworkedPrefab
{
    public GameObject Prefab;
    public string Path;
    public NetworkedPrefab(GameObject prefab, string path)
    {
        Prefab = prefab;
        Path = ReturnPathModified(path);
    }
    private string ReturnPathModified(string path)
    {
        int extensionLength = System.IO.Path.GetExtension(path).Length;
        int additionalLength = 10;
        int startIndex = path.ToLower().IndexOf("resource");
        if (startIndex == -1)
        {
            return string.Empty;
        }
        else return path.Substring(startIndex + additionalLength, path.Length - (additionalLength + startIndex + extensionLength));
    }
}
