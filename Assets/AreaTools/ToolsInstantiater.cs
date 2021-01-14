using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsInstantiater : MonoBehaviour
{

    public Transform buttomHolder;
    public GameObject[] prefabs;

    private GameObject last;

    public void OnButtonClick(Transform origem)
    {
        //limpa os anteriores
        if(last)
            Destroy(last);
        last = Instantiate(prefabs[GetIndex(origem)], transform);
    }

    private int GetIndex(Transform t)
    {
        for (int i = 0; i < buttomHolder.childCount; i++)
        {
            if (buttomHolder.GetChild(i) == t)
                return i;
        }
        return -1;
    }
}
