using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Game Setting", menuName = "Scriptable Objects/Networking/Game Setting")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.0.0";
    public string GetGameVersion { get { return _gameVersion; } }
    [SerializeField] private string _nickName = "User";
    public string GetNickName
    {
        get
        {
            int val = Random.Range(0, 9999);
            return _nickName + val.ToString();
        }
    }

    [SerializeField] private int _maxPlayers = 4;
    public int MaxPlayers { get { return _maxPlayers; } }
}
