using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 最小範圍2*2可形成導航網圖，當失去可行走路徑時進行破壞
//Q:遇到障礙物的處理


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class SoldierAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody controller;

    public Transform target;
    private bool isStop = false;
    public float moveSpeed = 1;
    private float pointDst = 0.5f;//允許跳至下一個點的距離

    [Header("Debug")]
    [SerializeField,ReadOnly] private string debugStatus = string.Empty;

    [SerializeField]private Vector3[] points = new Vector3[0];//整個路徑
    [SerializeField]private int pathIndex = 0;//目前目標點的索引值
    public float dstToNextPoint { get { return Vector3.Distance(transform.position, points[pathIndex]); } }
    public float dstToEndPoint { get {
            float dst = Vector3.Distance(transform.position, points[pathIndex]);
            for(int i = pathIndex; i < points.Length - 1; i++) {
                dst += Vector3.Distance(points[i], points[i + 1]);
            }
            return dst;
        } 
    }

    void Awake()
    {
        controller = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    public void SetPathANDTarget(Vector3[] points, Transform target) 
    {
        pathIndex = 0;
        this.points = points;
        this.target = target;

        if(points.Length == 0) {
            ResetPath();
        }
        agent.speed = moveSpeed;
    }
    public void SetMoveSpeed(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    private void FixedUpdate() {
        if(isStop) { return; }

        if(target == null) {
            Debug.LogWarning("No Target Point");
            debugStatus = "Error No Target";
            return;
        }

        //路徑為空，或是路徑的終點不是目標點
        if(points == null || Vector3.Distance(points[points.Length - 1], target.position) > 0.1f) {
            ResetPath();
            return;
        }

        if(pathIndex >= points.Length) {
            debugStatus = "Reach Target";
            return;
        }

        Vector3 direction = points[pathIndex] - transform.position;
        direction = new Vector3(direction.x, 0, direction.z).normalized;

        UpdatePosition(direction);
        UpdateRotation(direction);
    }

    public void Stop() {
        debugStatus = "Stop";
        isStop = true;
    }
    public void Continue() {
        isStop = false;
    }

    private void UpdatePosition(Vector3 direction) 
    {
        debugStatus = "Moving";

        
        controller.MovePosition(controller.position + direction * moveSpeed * Time.fixedDeltaTime);
        agent.nextPosition = transform.position;//更新代理的位置
        if(Vector3.Distance(transform.position, points[pathIndex]) < pointDst)
        {
            pathIndex++;
        }
    }
    private void UpdateRotation(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    [ContextMenu("Reset Path")]
    private void ResetPath() 
    {
        Debug.Log("Reset Path");
        debugStatus = "ResetPath";
        if(agent.SetDestination(target.position)) {
            points = agent.path.corners;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //描繪整個路徑
        if (points.Length > 0)
        {
            Gizmos.color = new Color(.4f, .4f, 1);
            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawSphere(points[i], 0.1f);
            }

            //至下一個點的路徑
            if (pathIndex < points.Length)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, points[pathIndex]);
            }
        }
    }

}
