using UnityEngine;
using System.Collections;
using System;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (wasDestroyed) return null;

            if (instance == null)
            {
                instance = FindInstance() ?? CreateInstance();
            }

            return instance;
        }
    }

    private static bool wasDestroyed;

    private static T FindInstance()
    {
        return FindObjectOfType<T>();
    }

    private static T CreateInstance()
    {
        var gameObject = new GameObject(typeof(T).ToString());
        gameObject.transform.SetAsFirstSibling();
        return gameObject.AddComponent<T>();
    }

    private void OnDestroy()
    {
        wasDestroyed = true;
    }
}
