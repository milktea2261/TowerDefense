using UnityEngine;
using Cinemachine;

public class CMCamController : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public GameObject activeCameraObj;
    [SerializeField, ReadOnly]private CinemachineTransposer transposer;

    public float scrollSpeed = 10f;
    public Vector2 dstLimit = new Vector2(10, 20);

    void Update()
    {
        GetTranspser();

        if(transposer != null) {
            float y = Input.mouseScrollDelta.y;
            Vector3 offset = transposer.m_FollowOffset;
            offset.y = Mathf.Clamp(offset.y - y * scrollSpeed * Time.unscaledDeltaTime, dstLimit.x, dstLimit.y);
            transposer.m_FollowOffset = offset;
        }
    }

    void GetTranspser() {
        if(cinemachineBrain.ActiveVirtualCamera != null) {
            if(activeCameraObj == null || activeCameraObj != cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject) {
                activeCameraObj = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;//獲取當前激活的攝影機物件

                CinemachineVirtualCamera virtualCamera = activeCameraObj.GetComponent<CinemachineVirtualCamera>();
                CinemachineComponentBase[] cinemachineComponents = virtualCamera.GetComponentPipeline();

                if(cinemachineComponents.Length > 0) {
                    foreach(var component in cinemachineComponents) {
                        if(component is CinemachineTransposer) {
                            transposer = (CinemachineTransposer)component;
                            break;
                        }
                    }
                }
            }
        }

    }
}
