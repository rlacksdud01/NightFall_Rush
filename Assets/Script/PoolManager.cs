using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject Get(int index, GameObject gridObj = null, Transform player = null)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        Reposition reposition = select.GetComponent<Reposition>();
        if (reposition != null)
        {
            if (gridObj != null) reposition.grid = gridObj;
            if (player != null) reposition.player = player;
        }

        return select;
    }

    public void Clear(int index)
    {
        foreach (GameObject item in pools[index])
            item.SetActive(false);
    }

    public void ClearAll()
    {
        for (int index = 0; index < pools.Length; index++)
            foreach (GameObject item in pools[index]) item.SetActive(false);
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false); 
        obj.transform.SetParent(transform); 

        for (int index = 0; index < prefabs.Length; index++)
        {
            if (prefabs[index].name == obj.name.Replace("(Clone)", "").Trim())
            {
                pools[index].Add(obj);
                break;
            }
        }
    }
}
