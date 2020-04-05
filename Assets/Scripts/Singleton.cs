using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    static T _instance;
    public static T Instance
    {
        get { return _instance; }
    }
    // Use this for initialization
    void Start()
    {
        if (_instance != null)
        {
            Debug.LogError("Trying to initialize existing singleton of class: " + typeof(T).ToString());
            return;
        }

        _instance = this as T;
    }
}
