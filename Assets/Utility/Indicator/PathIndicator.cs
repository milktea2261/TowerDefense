using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class PathIndicator : MonoBehaviour
{
    private LineRenderer pathRenderer;
    private NavMeshAgent agent;

    public Vector3[] points = new Vector3[0];

    private void Awake() {
        pathRenderer = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        SetPathByNavMeshAgent(transform.position, LevelManager.Instance.player.castle.transform.position);
    }

    /// <summary>
    /// 利用AI尋路功能獲得路徑
    /// </summary>
    /// <param name="start">起點</param>
    /// <param name="end">終點</param>
    public void SetPathByNavMeshAgent(Vector3 start, Vector3 end) {
        transform.position = start;
        agent.enabled = true;
        NavMeshPath navMeshPath = new NavMeshPath();
        if(agent.CalculatePath(end, navMeshPath)) {
            SetPath(navMeshPath.corners);
        }
        else {
            Debug.LogError("Find path Failed");
        }
        agent.enabled = false;
    }

    /// <summary>
    /// 設定座標組給渲染器
    /// </summary>
    /// <param name="points"></param>
    public void SetPath(Vector3[] points) {
        this.points = points;

        pathRenderer.positionCount = points.Length;
        pathRenderer.SetPositions(points);
    }
}
