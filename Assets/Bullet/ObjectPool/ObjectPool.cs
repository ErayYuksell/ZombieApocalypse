using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> queue = new Queue<GameObject>();
    [SerializeField] GameObject bulletObject;
    [SerializeField] int poolValue = 15;
    private void Start()
    {
        CreateGameObject();
    }
    public void CreateGameObject()
    {
        for (int i = 0; i < poolValue; i++)
        {
            GameObject obj = Instantiate(bulletObject, gameObject.transform);
            queue.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    public GameObject GetPoolObjects()
    {
        foreach (GameObject obj in queue)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                queue.Enqueue(obj);
                return obj;
            }
        }
        GameObject objt = Instantiate(bulletObject);
        queue.Enqueue(objt);
        objt.SetActive(false);
        return objt;
    }
}
