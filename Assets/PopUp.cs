using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public void Setup(string msg)
    {
        transform.parent = FindObjectOfType<Canvas>().transform;
        transform.localPosition = Vector3.zero;
        GetComponentInChildren<Text>().text = msg;
    }

    public void Aceitar()
    {
        Destroy(this.gameObject);
    }
}
