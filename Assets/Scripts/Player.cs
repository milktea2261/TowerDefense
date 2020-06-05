using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private bool isWin = false;
    public bool isDarkFaction = false;
    public float money = 100;
    public Unit castle;

    public float moveSpeed = 2f;

    public LayerMask selectableLayer;
    public ISelectable selectedObj = null;

    public Vector2 xMoveRange = new Vector2(-100,100);
    public Vector2 zMoveRange = new Vector2(-100, 100);

    private void Start() {
        castle.SetFaction(isDarkFaction);
        castle.name = name + "'s castle";
        castle.OnDead += LoseGame;
        LevelManager.Instance.waveManager.OnAllEnemiesClear += WinGame;
    }

    private void LoseGame(Unit castle) {
        Debug.Log("You Lose");
        isWin = false;
        LevelManager.Instance.GameOver(isWin);
    }
    private void WinGame() {
        Debug.Log("You Win");
        isWin = true;
        LevelManager.Instance.GameOver(isWin);
    }

    void Update()
    {
        if(Application.isEditor) {
            Debug.Log("Is Editor");
        }
        if(Application.isMobilePlatform) {
            Debug.Log("Is Mobile");
        }
        if(Application.isPlaying) {
            Debug.Log("Is playing");
        }
        if(Application.isConsolePlatform) {
            Debug.Log("Is Console");
        }

#if UNITY_EDITOR
        Debug.Log("editor platform");
#endif
#if UNITY_STANDALONE
        Debug.Log("standalone platform");
#endif
#if UNITY_ANDROID
        Debug.Log("android platform");
#endif



        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 pos = transform.position;
        pos += input * moveSpeed * Time.unscaledDeltaTime;
        pos.x = Mathf.Clamp(pos.x, xMoveRange.x, xMoveRange.y);
        pos.z = Mathf.Clamp(pos.z, zMoveRange.x, zMoveRange.y);
        transform.position = pos;

        //滑鼠在UI物件上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (LevelManager.Instance.buildingSystem.HasPreviewObject)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                LevelManager.Instance.buildingSystem.ConfirmPreview();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                LevelManager.Instance.buildingSystem.CancelPreview();
            }
            return;
        }

        //傳遞滑鼠事件
        if (Input.GetMouseButtonDown(0))
        {
            MouseSelect();
            return;
        }
    }
    private void MouseSelect() 
    {
        Ray mousrRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mousrRay, out RaycastHit hitInfo, 100f, selectableLayer))
        {
            //標記選取
            ISelectable selectable = hitInfo.collider.GetComponent<ISelectable>();
            if(selectable != null) {
                if(selectable != selectedObj) {
                    if(selectedObj != null) {
                        CancelSelect();
                    }
                    selectedObj = selectable;
                    selectedObj.OnSelected();
                }
            }
            else if(selectedObj != null) {
                CancelSelect();
            }
        }
    }
    void CancelSelect() {
        selectedObj.OnDeselected();
        selectedObj = null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Vector3 min = new Vector3(xMoveRange.x, 0, zMoveRange.x);
        Vector3 max = new Vector3(xMoveRange.y, 0, zMoveRange.y);
        Gizmos.DrawLine(new Vector3(min.x, 0, min.z), new Vector3(min.x, 0, max.z));
        Gizmos.DrawLine(new Vector3(min.x, 0, max.z), new Vector3(max.x, 0, max.z));
        Gizmos.DrawLine(new Vector3(max.x, 0, max.z), new Vector3(max.x, 0, min.z));
        Gizmos.DrawLine(new Vector3(max.x, 0, min.z), new Vector3(min.x, 0, min.z));
    }
}
