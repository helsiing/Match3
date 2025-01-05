using System;
using UnityEngine;

namespace VoodooMatch3.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        _instance = go.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        public void Awake()
        {
            if(_instance == null)
            {
                _instance = this as T;
                transform.parent = null;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}