using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Setting", menuName = "Scriptable Objects/Networking/Game Setting")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.0.0"; //todo Use GameManager.Version

    public string GetGameVersion
    { get { return _gameVersion; } }

    //todo Need a way to generate more unique id and save it for future
    //may be System.Guid help? - we can discuss it later
    [SerializeField] private string _nickName = "User";

    //todo generate nickname only when there is no nickname in save data
    //todo on generating new nickname make sure to save it
    public string GetNickName
    {
        get
        {
            int val = Random.Range(0, 9999);
            return _nickName + val.ToString();
        }
    }

    [SerializeField] private int _maxPlayers = 4; //! 3 or 4 need both room size for now leave it!

    public int MaxPlayers
    { get { return _maxPlayers; } }

    //todo Create a public method to load/reload all gameData for future ref, it can be access through GameManager.Instance.GameSettings
}