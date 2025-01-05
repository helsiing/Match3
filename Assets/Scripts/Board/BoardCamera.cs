#region
using UnityEngine;
#endregion

namespace VoodooMatch3
{
    public class BoardCamera : MonoBehaviour
    {
        public void SetupCamera(int width, int height)
        {
            // TODO: change this to another place
            float cameraX = width / 2 - 0.5f;
            float cameraY = height / 2 - 0.5f;
            Camera.main.transform.position = new Vector3(cameraX, cameraY, Camera.main.transform.position.z);
        }
    }
}