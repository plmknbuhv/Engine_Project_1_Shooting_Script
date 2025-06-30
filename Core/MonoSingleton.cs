using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<T>();

                if (!_instance)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    _instance = obj.GetComponent<T>();
                }
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}