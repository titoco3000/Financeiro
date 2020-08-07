using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TableTitle : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        transform.GetComponentInParent<Transform>().GetComponentInParent<Tabela>().ChangeSortingMethod(int.Parse(transform.name));
    }
}
