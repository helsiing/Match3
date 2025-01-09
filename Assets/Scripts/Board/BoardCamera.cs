#region
using UnityEngine;
#endregion

namespace VoodooMatch3
{
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField]
        private float yOffset = 1.5f;
        public void SetupCamera(int width, int height)
        {
            float cameraX = width / 2;
            float cameraY = height / 2 + yOffset;
            Camera.main.transform.position = new Vector3(cameraX, cameraY, Camera.main.transform.position.z);
        }
    }
}