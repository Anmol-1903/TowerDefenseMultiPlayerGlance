using System.Collections;
using System.Collections.Generic;
using Core.SaveLoadSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Setting", menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    public string GetGameVersion
    { get { return Application.version; } }

    //todo Need a way to generate more unique id and save it for future
    //may be System.Guid help? - we can discuss it later
    [SerializeField] private string _nickName = "User";

    //todo generate nickname only when there is no nickname in save data
    //todo on generating new nickname make sure to save it
    public string GetNickName
    {
        get
        {
            _nickName = SaveLoad.Load("NickName", "");
            // Check if _nickName is null or empty
            if (string.IsNullOrEmpty(_nickName))
            {
                // Generate a new nickname
                int val = Random.Range(0, 9999);
                _nickName = "User" + val.ToString();
                SaveNickName(_nickName);
            }
            return _nickName;
        }
    }

    private void SaveNickName(string nickname)
    {
        SaveLoad.Save(nickname, "NickName");
        // Your code to save the nickname to the save data
    }

    [field: SerializeField] public int MaxPlayers { get; private set; }//! 3 or 4 need both room size for now leave it!

    //todo Create a public method to load/reload all gameData for future ref, it can be access through GameManager.Instance.GameSettings
}