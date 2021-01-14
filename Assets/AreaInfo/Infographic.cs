using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infographic : MonoBehaviour
{
    private Data_Info manager;
    private void OnEnable()
    {
        Refresh();
    }
    public virtual void Setup(List<Compra> compras)
    {

    }

    public void Refresh()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<Data_Info>();
        }
        Setup(manager.comprasNoPeriodo);
    }

}
