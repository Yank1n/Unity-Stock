using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindOrCreateIntance();
            }
            return _instance;
        }
    }

    private static T FindOrCreateIntance()
    {
        var instance = GameObject.FindObjectOfType<T>();

        if (instance != null)
        {
            return instance;
        }

        var name = typeof(T).Name + " Singleton";

        var containerGameObject = new GameObject(name);

        var singletonComponent = containerGameObject.AddComponent<T>();

        return singletonComponent;
    }
}