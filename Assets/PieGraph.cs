using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Desenha um pie graph
     */
public class PieGraph : MonoBehaviour
{
    public Sprite Circle;
    public Font graphFont;

    private float diametro;

    public GameObject NewGameObj;

    public string EmptyMessage = "0%";



    public void Draw(float[] values)
    {
        string[] names = new string[values.Length];
        Color[] cores = new Color[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            names[i] = "";
            cores[i] = RandomColor();
        }
        Draw(values,names,cores);
    }
    public void Draw(float[] values, string[] names)
    {
        Color[] cores = new Color[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            cores[i] = RandomColor();
        }
        Draw(values, names, cores);
    }
    public void Draw(float[] values, Color[] cores)
    {
        string[] names = new string[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            names[i] = "";
        }
        Draw(values, names, cores);
    }
    public void Draw(float[] values, string[] names, Color[] cores)
    {

        diametro = this.GetComponent<RectTransform>().sizeDelta[0];

        //limpa 
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float total = 0;
        foreach (var item in values)
        {
            total += item;
        }

        if(total <= 0)
        {
            Text txt = Instantiate(NewGameObj, this.transform).AddComponent<Text>();
            txt.color = Color.black;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.font = graphFont;
            txt.text = EmptyMessage;
            return;
        }

        float rotation = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if(values[i] != 0)
            {
                Image piece = Instantiate(NewGameObj,this.transform).AddComponent<Image>();

                //corta a fatia
                piece.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(diametro, diametro);
                piece.sprite = Circle;
                piece.type = Image.Type.Filled;
                piece.fillClockwise = false;
                piece.fillOrigin = 2;
                piece.fillAmount = values[i]/total;
                piece.rectTransform.rotation = Quaternion.Euler(0, 0, rotation);
            
                //adiciona o objeto do texto
                Text textObj = Instantiate(NewGameObj, this.transform).AddComponent<Text>();
                textObj.transform.SetParent(transform);
                textObj.font = graphFont;
                textObj.alignment = TextAnchor.MiddleCenter;

                //posiciona o texto
                float midRotation = -rotation - ((values[i] / total) * 180);
                textObj.transform.localPosition = PosNoCirculo(midRotation, diametro/4);

                //dá o valor ao texto
                if (name.Length > i)
                    textObj.text = names[i];
            
                //acrescenta a rotação
                rotation += ((values[i] / total) * 360);
           
                //Cor
                if (cores.Length > i)
                    piece.color = cores[i];
                else
                    piece.color = RandomColor();
            }
        }
    }


    public Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private Vector2 PosNoCirculo(float angulo, float raio)
    {
        return new Vector2(
                Mathf.Sin(angulo* Mathf.PI / 180) * raio,
                Mathf.Cos( angulo* Mathf.PI / 180) * raio
                );
    }

}
