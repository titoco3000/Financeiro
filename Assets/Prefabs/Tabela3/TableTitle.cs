using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TableTitle :Button
{
    protected override void Start()
    {
        if (!int.TryParse(gameObject.name, out int r))
        {
            gameObject.name = "0";
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        transform.parent.parent.parent.GetComponent<Tabela3>().SortBy(int.Parse(gameObject.name));
    }
}
