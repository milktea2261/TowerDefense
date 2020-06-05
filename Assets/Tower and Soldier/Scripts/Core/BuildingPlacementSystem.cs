using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 基本:
/// 預覽物沒有金額限制，建造時會告知金額是否不足
/// 物體的放置與拆除，物體不能重疊，物體貼在地上不會飛到其他的物體上方
/// 進階:
/// 物體建造時間結束後才能使用功能，建造期間可被破壞、拆除
/// 物體強化(加裝其他東西EX:建塔加裝弩箭)、物體升級(修改物體EX:箭塔升級法師塔)
/// 
/// 針對射界狹小的塔
/// 1.確認建造前，可以旋轉建築，達到自己想要的布置角度
/// 2.讓塔有緩慢旋轉的功能，可重新設定攻擊區域
/// 3.重新布置功能，將物體放到想要的角度OR新的地方(EX:大型吊機)
/// 
/// </summary>
public class BuildingPlacementSystem : MonoBehaviour
{
    public LayerMask groundMask;//地面
    public LayerMask obstacleMask;//障礙物(建築、樹木、士兵等)

    public Building[] buildings;

    private Building buildingInHand = null;//拿在手中的物體
    private int previewIndex = -1;

    private void Update()
    {
        if (buildingInHand != null)
        {
            //玩家視角投射到場景上的射線
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100, Color.blue);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, 100, groundMask))
            {
                //將預覽物放在地面上
                buildingInHand.transform.position = hitInfo.transform.position + Vector3.up * 1f;

                ////由上而下的射線
                //Ray topDownRay = new Ray(hitInfo.point + Vector3.up * 50, Vector3.down * 100);
                //Debug.DrawRay(topDownRay.origin, topDownRay.direction * 100, Color.red);
                //if(Physics.Raycast(topDownRay, out RaycastHit hitInfo2, 100, groundMask)) {
                    ////將預覽物吸附在可建造區的位置
                    //buildingInHand.transform.position = hitInfo2.point;
                //}
            }

            //you can rotate object here
        }
    }

    #region Check Can build
    private float thickness = 0.01f;//避免大小太剛好造成不期望的碰撞
    public float allowSlopeAngle = 5;//可容忍的斜坡角度

    /// <summary>
    /// 檢測是否在可建造區域
    /// </summary>
    /// <returns></returns>
    private bool IsBuildableArea() {
        Ray ray = new Ray(buildingInHand.transform.position + Vector3.up * 50, Vector3.down * 100);
        if(Physics.Raycast(ray, out RaycastHit hit, 100, groundMask)) {

            if(hit.collider.tag != "Buildable") return false;

            //斜坡判定
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            bool isSlope = slopeAngle > allowSlopeAngle;
            return !isSlope;


            //現在的圖層是否包含在遮罩中
            //bool isIncludeLayer = buildableMask == (buildableMask | (1 << hit.collider.gameObject.layer));
            //return !isSlope && isIncludeLayer;
        }
        return false;
    }

    /// <summary>
    /// 是否和周遭的物體重疊
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>
    /// <returns>true:重疊</returns>
    private bool OverlapBuildings(Vector3 pos, Quaternion quaternion) {
        Vector3 size = Vector3.one * (1 - thickness);
        Collider[] colliders = Physics.OverlapBox(pos, size * 0.5f, quaternion, obstacleMask);
        return colliders.Length > 0;
    }

    #endregion


    //生成預覽物
    public void CreatePreview(int index)
    {
        if (buildingInHand != null)
        {
            Destroy(buildingInHand.gameObject);
        }
        buildingInHand = Instantiate(buildings[index]);
        buildingInHand.enabled = false;
        buildingInHand.level = 0;
        if(buildingInHand is Tower) {
            Tower tower = (Tower)buildingInHand;
            tower.rangeIndicator.gameObject.SetActive(true);
        }
        previewIndex = index;

        buildingInHand.SetFaction(LevelManager.Instance.player.isDarkFaction);
        buildingInHand.GetComponent<Collider>().enabled = false;

        Cursor.visible = false;
    }

    /// <summary>
    /// 確認在預覽物的位置建造
    /// </summary>
    public void ConfirmPreview()
    {
        if (previewIndex == -1) return;

        //判斷是否與其他物體(buildings layer)重疊
        if (IsBuildableArea())
        {
            if(!OverlapBuildings(buildingInHand.transform.position, buildingInHand.transform.rotation)) {

                if(LevelManager.Instance.player.money < buildings[previewIndex].cost) {
                    Debug.Log("Not Enough Moeny");
                    return;
                }
                //扣錢
                LevelManager.Instance.player.money -= buildings[previewIndex].cost;
                BuildBuilding(buildingInHand);

                if(Input.GetKey(KeyCode.LeftShift)) {
                    //連續建造
                    //nothing
                }
                else {
                    //單一建造
                    buildingInHand = null;
                    Cursor.visible = true;//顯示鼠標
                }

            }
            else {
                Debug.Log("該區域已有東西");
            }
        }
        else
        {
            Debug.Log("不可建造的區域");
        }
    }
    public void CancelPreview() 
    {
        Debug.Log("Cancel Building.");
        if (buildingInHand != null)
        {
            Destroy(buildingInHand.gameObject);
        }
        buildingInHand = null;

        Cursor.visible = true;//顯示鼠標
    }

    public void BuildBuilding(Building newBuilding)
    {
        //Debug.Log("Build " + newBuilding);
        newBuilding.enabled = true;
        buildingInHand.GetComponent<Collider>().enabled = true;
        newBuilding.OnDead += DestroyBuilding;
        newBuilding.OnDestroy += DestroyBuilding;
        newBuilding.Init();
    }
    public void DestroyBuilding(Unit building)
    {
        //Debug.Log("destroy " + building);
        building.OnDestroy -= DestroyBuilding;
        building.OnDeselected();
        if(building as ISelectable == LevelManager.Instance.player.selectedObj) {
            LevelManager.Instance.player.selectedObj = null;
        }

        Destroy(building.gameObject);
    }


    public bool HasPreviewObject => buildingInHand != null;
    private Vector3 SnapPoint(Vector3 pos) {
        Vector3 newPoint;

        newPoint.x = Mathf.Round(pos.x);
        newPoint.y = pos.y;
        newPoint.z = Mathf.Round(pos.z);
        return newPoint;
    }
}