using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace Core
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        public UnityAction OnGameStart { get; set; }
        public UnityAction<bool> OnGameStop { get; set; }
    }
}