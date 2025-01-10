using UnityEngine;

namespace VoodooMatch3.Utils
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object being checked.</param>
        /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
        public static T OrNull<T> (this T obj) where T : Object => obj ? obj : null;
        
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T existing = gameObject.GetComponent<T>();
            if (existing != null)
                return existing;

#if UNITY_EDITOR
            return UnityEditor.Undo.AddComponent<T>(gameObject);
#else
			return gameObject.AddComponent<T>();
#endif
        }

        public static string GetPathToParent(this GameObject obj, GameObject parent, bool trimWhiteSpace = false)
        {
            string path = obj.name;
            while (obj.transform.parent != null && obj.transform.parent.gameObject != parent)
            {
                obj = obj.transform.parent.gameObject;
                path = (trimWhiteSpace ? obj.name.Trim() : obj.name) + "/" + path;
            }
            return path;
        }

        public static void DestroyChildObjects(this GameObject obj)
        {
            foreach (Transform child in obj.transform) {
                Object.Destroy(child.gameObject);
            }
        }
    }
}