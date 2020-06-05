using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//改善:內容拖動，排版
public class WaveWindow : EditorWindow
{
    private WaveManager waveManager;

    List<bool> foldouts;//摺疊wave的開關
    string[] enemyNames;
    string[] spawnPointNames;

    //添加開啟視窗的選項到Tools
    [MenuItem("Tools/Wave Editor")]
    public static void ShowWindow()
    {
        WaveWindow editorWindow = (WaveWindow)EditorWindow.GetWindow(typeof(WaveWindow));
        editorWindow.Show();
    }

    //打開視窗時觸發
    private void OnEnable()
    {
        waveManager = FindObjectOfType<WaveManager>();
        foldouts = new List<bool>(new bool[waveManager.waves.Count]);

        //data to dropdownList
        enemyNames = new string[waveManager.enemiesDB.Length];
        for (int i = 0; i < enemyNames.Length; i++)
        {
            enemyNames[i] = waveManager.enemiesDB[i].name;
        }

        spawnPointNames = new string[waveManager.paths.Length];
        for (int i = 0; i < spawnPointNames.Length; i++)
        {
            spawnPointNames[i] = waveManager.paths[i].name;
        }
    }

    void OnGUI()
    {
        if (waveManager.waves.Count > 0)
        {
            EditorGUI.BeginChangeCheck();
            //show wave
            for (int i = 0; i < waveManager.waves.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "Wave " + i);
                if (GUILayout.Button("Add Wave"))
                {
                    waveManager.waves.Insert(i+1, new Wave());
                    foldouts.Insert(i+1, true);
                    break;//迴圈中，長度變短導致報錯，等待下次刷新自動解決
                }
                if (GUILayout.Button("Delete Wave"))
                {
                    waveManager.waves.RemoveAt(i);
                    foldouts.RemoveAt(i);
                    break;//迴圈中，長度變短導致報錯，等待下次刷新自動解決
                }
                EditorGUILayout.EndHorizontal();

                if (foldouts[i])
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                    //show wave detail
                    for (int j = 0; j < waveManager.waves[i].spawnOrders.Count; j++)
                    {
                        DrawOrderLayout(i, j);
                    }

                    if (GUILayout.Button("Add Order"))
                    {
                        //空值會導致GUI報錯
                        SpawnOrder newOrder = new SpawnOrder(waveManager.enemiesDB[0], 1, waveManager.paths[0]);
                        waveManager.waves[i].spawnOrders.Add(newOrder);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();//波次的間隔
            }
            EditorGUI.EndChangeCheck();
        }
        else
        {
            if (GUILayout.Button("New Empty Wave"))
            {
                waveManager.waves.Add(new Wave());
                foldouts.Add(false);
            }
        }
    }

    private void DrawOrderLayout(int waveIndex, int orderIndex)
    {
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        SpawnOrder spawnOrder = waveManager.waves[waveIndex].spawnOrders[orderIndex];

        spawnOrder.startTime = EditorGUILayout.FloatField("Start Time", spawnOrder.startTime);
        spawnOrder.spawnInterval = EditorGUILayout.FloatField("Spawn Interval", spawnOrder.spawnInterval);
        EditorGUILayout.Space();//波次的間隔

        //enemy field
        int enemyIndex = GetIndex<Soldier>(spawnOrder.enemy, waveManager.enemiesDB);
        enemyIndex = EditorGUILayout.Popup(enemyIndex, enemyNames);
        spawnOrder.enemy = waveManager.enemiesDB[enemyIndex];
        //amount field
        spawnOrder.amount = EditorGUILayout.IntField("Amount", spawnOrder.amount);
        EditorGUILayout.Space();//波次的間隔

        //spawnPoint field
        int spawnPointIndex = GetIndex<PathIndicator>(spawnOrder.path, waveManager.paths);
        spawnPointIndex = EditorGUILayout.Popup(spawnPointIndex, spawnPointNames);
        spawnOrder.path = waveManager.paths[spawnPointIndex];

        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("has change");
            waveManager.waves[waveIndex].spawnOrders[orderIndex] = spawnOrder;
            //Undo.RecordObject(target, "Edit Wave");
        }

        if (GUILayout.Button("Delete"))
        {
            //Debug.Log("Delete " + waveManager.waves[i].Enemy[j]);
            waveManager.waves[waveIndex].spawnOrders.RemoveAt(orderIndex);
            //break;
        }
        EditorGUILayout.EndVertical();
    }

    private int GetIndex<T>(T target, T[] collections) where T : Object
    {
        int index = -1;
        for (int i = 0; i < collections.Length; i++)
        {
            if (collections[i] == target)
            {
                index = i;
                return index;
            }
        }
        Debug.LogWarning("Not Found Index in " + collections);
        return index;
    }

}