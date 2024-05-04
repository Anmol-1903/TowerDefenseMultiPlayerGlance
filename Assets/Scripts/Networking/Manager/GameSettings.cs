using System.Collections;
using System.Collections.Generic;
using Core.SaveLoadSystem;
using UnityEngine;

/*
 *
 * Load the Game Settings from file - I/O
 *
 * TODO: Need a better way to generate unique id
 *      - lets divide the nickname in two part: Username_1234456789
 *      - Both part is separate by '_'
 *      - First part will displayed to users and customizable as per user need, it is not neccessary that first will unique
 *      - Second part decide the uniqueness of nickname and it will just hidden from the users, in order to generate the this unique numebe we can use date/time and SystemGuid etc.
 *      - Name Generation will only works when there is no data regrading nickname
 *
 *  TODO: Create methods to load save data from file
 *      - Game Version
 *      - Nickname: name_id
 *      - Settings Data
 *          - Vibration
 *          - SFX
 *          - BG
 *  TODO: Create a Method to Save the Data
 *
 */

namespace Core
{
    [CreateAssetMenu(fileName = "New Game Setting", menuName = "Game/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public string GameVersion => Application.version;

        [SerializeField] private string nickname = "Player";
        [SerializeField, Disable] private string nicknameId = "";
        [field: SerializeField] public int MaxPlayers { get; private set; }//! 3 or 4 need both room size for now leave it!

        public string GetNickName => $"{nickname}_{nicknameId}";
        public string DisplayNickName { get => nickname; set => nickname = value; }

        [SerializeField] private Color[] playerColors;

        public Color[] GetPlayerColors
        { get { return playerColors; } }

        public void LoadData()
        {
            nickname = SaveLoad.Load("Nickname", "Player");
            //nicknameId = SaveLoad.Load("Id", Util.HelperMethods.GenerateUniqueId());
            nicknameId = Util.HelperMethods.GenerateUniqueId();
        }

        public void SaveData()
        {
            SaveLoad.Save("Nickname", nickname);
            // SaveLoad.Save("Id", nicknameId);
        }
    }
}