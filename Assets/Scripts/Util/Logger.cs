using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class Logger
    {
        public static void Log(this object message, Object caller = null, string logColorCode = "ffffff", int fontSize = 12)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"<color=#{logColorCode}><size={fontSize}>{message}</size></color> \n called from {caller} ", caller);
#endif
        }

        public static void Log(this object message, Color color, Object caller = null, int fontSize = 12)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            string logColorCode = ColorUtility.ToHtmlStringRGB(color);
            Debug.Log($"<color=#{logColorCode}><size={fontSize}>{message}</size></color> \n called from {caller} ", caller);
#endif
        }

        public static void Log<T>(this T[] message, string logColorCode = "ffffff", int fontSize = 12)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            string msg = "";
            for (int i = 0; i < message.Length; i++)
            {
                msg = msg + message[i].ToString() + ",";
            }
            Debug.Log($"<color=#{logColorCode}><size={fontSize}>{msg}</size></color>");
#endif
        }

        public static void Log<T>(this List<T> message, string logColorCode = "ffffff", int fontSize = 12)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            string msg = "";
            for (int i = 0; i < message.Count; i++)
            {
                msg = msg + message[i].ToString() + ",";
            }
            Debug.Log($"<color=#{logColorCode}><size={fontSize}>{msg}</size></color>");
#endif
        }
    }
}