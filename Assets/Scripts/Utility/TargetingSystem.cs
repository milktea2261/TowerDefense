using UnityEngine;

/// <summary>
/// 索敵系統，找到最優先攻擊的目標
/// </summary>
public static class TargetingSystem
{
    public enum TargetMode { Closet, Strongest, Weakest, Random } //First, Priority

    public static Unit GetTarget(Unit[] enemies, Vector3 myPosition, TargetMode targetMode)
    {
        if (enemies.Length > 0)
        {
            switch (targetMode)
            {
                //case TargetMode.First:
                    //return GetFirstTarget(enemies);
                case TargetMode.Strongest:
                    return GetStrongestTarget(enemies);
                case TargetMode.Weakest:
                    return GetWeakestTarget(enemies);
                case TargetMode.Closet:
                    return GetClosetTarget(enemies, myPosition);
                case TargetMode.Random:
                    return GetRandomTarget(enemies);
            }
        }
        return null;
    }
    
    //找離自己最近的
    private static Unit GetClosetTarget(Unit[] damagables, Vector3 myPosition) {
        Unit target = damagables[0];
        float minDistance = Vector3.Distance(myPosition, target.transform.position);
        for(int i = 1; i < damagables.Length; i++) {
            float distance = Vector3.Distance(myPosition, damagables[i].transform.position);
            if(minDistance > distance) {
                target = damagables[i];
                minDistance = distance;
            }
        }
        return target;
    }
    //找血量最多的
    private static Unit GetStrongestTarget(Unit[] damagables)
    {
        Unit target = damagables[0];
        for (int i = 1; i < damagables.Length; i++)
        {
            if (target.Health < damagables[i].Health)
            {
                target = damagables[i];
            }
        }
        return target;
    }
    //找血量最少的
    private static Unit GetWeakestTarget(Unit[] damagables)
    {
        Unit target = damagables[0];
        for (int i = 1; i < damagables.Length; i++)
        {
            if (target.Health > damagables[i].Health)
            {
                target = damagables[i];
            }
        }
        return target;
    }
    //隨機選擇
    private static Unit GetRandomTarget(Unit[] damagables)
    {
        int randomNum = Random.Range(0, damagables.Length);
        return damagables[randomNum];
    }

    /*
    private static Soldier_Pawn GetFirstTarget(Soldier_Pawn[] damagables) {
        Soldier_Pawn target = damagables[0];
        for(int i = 1; i < damagables.Length; i++) {
            if(target.DstToTarget > damagables[i].DstToTarget) {
                target = damagables[i];
            }
        }
        return target;
    }
    */


    //依據權重(Weights、priority)選擇對象
    //private static Unit GetPriorityTarget() {  }

}