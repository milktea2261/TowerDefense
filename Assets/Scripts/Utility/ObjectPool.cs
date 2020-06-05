using System.Collections.Generic;
using UnityEngine;

namespace My.Utility
{
    public class ObjectPool<T> where T : Component
    {

        public delegate void OnInit(T obj);
        public OnInit onInit;

        //使用Queue的原因，插入、刪除需時O(1)，可用替代結構Stack
        private Queue<T> availableObjects = new Queue<T>();//可使用的物件
        //private List<T> usingObjects = new List<T>();//使用中的物件

        [SerializeField]
        private T objectPrefab = null;

        #region 對物件的操作
        //獲得物件
        public virtual T GetObject()
        {
            T obj;
            if (availableObjects.Count == 0)
            {
                obj = Object.Instantiate(objectPrefab);//生成物件
            }
            else
            {
                obj = availableObjects.Dequeue();//取待機中的物件
            }

            InitObject(obj);
            return obj;
        }

        //初始化
        protected virtual void InitObject(T obj)
        {
            onInit(obj);
            obj.gameObject.SetActive(true);
        }

        //回收物件
        public virtual void RemoveObject(T obj)
        {
            availableObjects.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }

        #endregion

        #region 對物件池的操作
        //初始化物件池
        public void InitPool() { }
        #endregion
    }
}