using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TableContentBlock : MonoBehaviour
{
    private Text text;
    private RectTransform self;

    protected void Start()
    {
        self = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
    }
    public float currentPoint = 0;
    public bool cycling = false;
    public float cycleSpeed = 10f;
    private float mouseWaitTime = 1f;
    public float round = 0f;

    private void Update()
    {
        float textWidth = LayoutUtility.GetPreferredWidth(text.rectTransform) - self.sizeDelta.x;
        if (cycling && textWidth + self.sizeDelta.x > self.sizeDelta.x)
        {
             currentPoint = Mathf.MoveTowards(currentPoint, textWidth + self.sizeDelta.x, Time.deltaTime * cycleSpeed);
            if (currentPoint == textWidth + self.sizeDelta.x)
                currentPoint = - self.sizeDelta.x;
        }
        else
        {
            currentPoint = 0;
        }
        text.rectTransform.sizeDelta = new Vector2( textWidth, text.rectTransform.sizeDelta.y / 2);
        text.transform.localPosition = new Vector3(textWidth / 2 - currentPoint, 0, 0);
    }

    public void OnMouseEnter()
    {
        StartCoroutine(CheckMouse());
    }
    public void OnMouseExit()
    {
        StopAllCoroutines();
        currentPoint = 0;
        cycling = false;
    }

    IEnumerator CheckMouse()
    {
        yield return new WaitForSeconds(mouseWaitTime);
        cycling = true;
    }

}
