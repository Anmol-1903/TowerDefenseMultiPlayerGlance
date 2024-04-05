
using UnityEngine;

public abstract class ScriptableObjectSingelton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0)
                {
                    Debug.LogError("Singelton Scriptable Object -> Instance -> result lenght is 0 for type " + typeof(T).ToString() + ".");
                    return null;
                }
                if (results.Length > 1)
                {
                    Debug.LogError("Singelton Scriptable Object -> Instance -> result lenght is greater than 1 " + typeof(T).ToString() + ".");
                    return null;
                }
                _instance = results[0];
            }
            return _instance;
        }
    }
}
