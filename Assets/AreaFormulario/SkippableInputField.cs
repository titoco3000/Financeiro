using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SkipToNextField))]
public class SkippableInputField : InputField
{
    public UnityEvent OnSelectEvent;
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnSelectEvent.Invoke();

    }


    public override void OnUpdateSelected(BaseEventData eventData)
    {
        base.OnUpdateSelected(eventData);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable nextObj = GetComponent<SkipToNextField>().NextField;
            if (nextObj)
                nextObj.Select();
        }
    }

}
