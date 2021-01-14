using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MensagemVolatil : MonoBehaviour
{
    public Text Text;
    public float fadeSpeed = 0.5f;
    public float delay = 3f;

    public Color goodColor;
    public Color badColor;


    private void Start()
    {
        Text.text = "";
    }

    public void SetMessage(string txt, bool Boa = false, bool persistant = false)
    {
        if (Boa)
            Text.color = goodColor;
        else
            Text.color = badColor;

        if (persistant)
        {
            Text.text = Text.text + "\n" + txt;
        }
        else
        {
            Text.text = txt;
        }
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        Color cor = Text.color;
        cor.a = 1;
        Text.color = cor;
        
        yield return new WaitForSeconds(delay);
        while (cor.a > 0)
        {
            cor.a -= Time.deltaTime * fadeSpeed;
            Text.color = cor;
            yield return new WaitForEndOfFrame();
        }
        cor.a = 0;
        Text.color = cor;
    }
}
