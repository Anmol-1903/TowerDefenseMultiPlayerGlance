using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Master Manager", menuName = "Scriptable Objects/Networking/Master Manager")]
public class MasterManager : ScriptableObjectSingelton<MasterManager>
{
    [SerializeField] private GameSettings _gameSettings;
    public static GameSettings GameSettings { get { return Instance._gameSettings; } }
}
