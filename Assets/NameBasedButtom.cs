using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class NameBasedButtom : MonoBehaviour
{
    [System.Serializable]
    public class MyEvent : UnityEvent<int>
    {
    }

    public MyEvent OnClickInt;

    private void Start()
    {
        if (GetComponent<Button>())
        {

        }
        else
        {
            gameObject.AddComponent<Button>().onClick.AddListener(new UnityAction(OnClick));
        }
        if(!int.TryParse(gameObject.name, out int r))
        {
            gameObject.name = "0";
        }
    }
    public void OnClick()
    {
        OnClickInt.Invoke(int.Parse(gameObject.name));
    }
}
