using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
[CreateAssetMenu(fileName = "New Master Manager", menuName = "Scriptable Objects/Networking/Master Manager")]
public class MasterManager : ScriptableObjectSingelton<MasterManager>
{
    [SerializeField] private GameSettings _gameSettings;
    public static GameSettings GameSettings { get { return Instance._gameSettings; } }
    private List<NetworkedPrefab> networkedPrefabs = new List<NetworkedPrefab>();
    public static GameObject NetworkInstantiate(GameObject obj, Vector3 position, Quaternion rotation)
    {
        foreach (NetworkedPrefab networkedPrefab in Instance.networkedPrefabs)
        {
            if (networkedPrefab.Prefab == obj)
            {
                if (networkedPrefab.Path != String.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(networkedPrefab.Path, position, rotation);
                    return result;
                }
                else
                {
                    Debug.Log("Path is empty");
                    return null;
                }



            }
        }
        return null;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void PopulateNetworkedPrefabs()
    {
#if UNITY_EDITOR
        Instance.networkedPrefabs.Clear();
        GameObject[] results = Resources.LoadAll<GameObject>("");
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance.networkedPrefabs.Add(new NetworkedPrefab(results[i], path));
            }
        }
#endif
    }
}
