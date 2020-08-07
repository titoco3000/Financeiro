using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tabela : MonoBehaviour
{
    public enum TipoDeConteudo
    {
        Float,
        String,
        Data,
        Dinheiro,
        Button
    }

    public class Conteudo
    {
        public string Value;
        public TipoDeConteudo Tipo;
        public Conteudo(float f, TipoDeConteudo t)
        {
            Value = f.ToString();
            Tipo = t;
        }
        public Conteudo(string s)
        {
            Value = s;
            Tipo = TipoDeConteudo.String;
        }
        public Conteudo(Data d)
        {
            Value = d.ToString();
            Tipo = TipoDeConteudo.Data;
        }
        public Conteudo()
        {
            Tipo = TipoDeConteudo.Button;
        }
    }

    public class Coluna
    {
        public string Title;
        public TipoDeConteudo tipo;
        public float largura;
        public Coluna(string a,TipoDeConteudo b,float c)
        {
            Title = a;
            tipo = b;
            largura = c;
        }
    }

    public List<Coluna> colunas;

    //referencias internas
    public RectTransform TitlesBar;
    public RectTransform Row;
    public RectTransform content;

    //prefabs editor
    public GameObject titlePrefab;
    public GameObject fieldPrefab;
    public GameObject buttomPrefab;


    public bool ExpandToFill = true;
    public bool LimparTodaVez;
    public bool EditButtomAllowed;

    public int ExtraContentSize = 0;

    private GameObject listItemPrefab;

    public List<List<Conteudo>> tableContent;
    private List<Vector3> localFieldPositions;

    public int orderMethod = 0;

    private int borderWidth = 2;

    private void Start()
    {
        content.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, borderWidth);
    }

    public void Preparar(List<Coluna> c)
    {
        colunas = c;
        
        //adapta as larguras para ocupar toda a tabela
        if (ExpandToFill)
        {
            float somatorio = 0;
            foreach (Coluna item in colunas)
            {
                somatorio += item.largura;
            }
            float prop = TitlesBar.sizeDelta.x / somatorio;
            for (int i = 0; i < colunas.Count; i++)
            {
                colunas[i].largura *= prop;
                colunas[i].largura -= borderWidth;

            }
        }
        Preparar();
    }
    public void Preparar()
    {
        //limpa os títulos anteriores
        foreach (Transform child in TitlesBar)
        {
            Destroy(child.gameObject);
        }
        localFieldPositions = new List<Vector3>();

        //limpa o prefab da linha
        foreach (Transform  child in Row)
        {
            Destroy(child.gameObject);
        }

            
            //desenha cada um dos titulos
            float distanciaAtual = -TitlesBar.sizeDelta.x / 2;       
            for (int i = 0; i < colunas.Count; i++)
            {
                GameObject titleObj = Instantiate(titlePrefab, TitlesBar);
                titleObj.name = i.ToString();
                distanciaAtual += colunas[i].largura / 2 + borderWidth;
                RectTransform rt = titleObj.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(colunas[i].largura, TitlesBar.sizeDelta.y);
                titleObj.transform.localPosition = new Vector2(distanciaAtual , 0);
                titleObj.GetComponentInChildren<Text>().text = colunas[i].Title;
            
                //prefab da linha
                RectTransform field = Instantiate((colunas[i].tipo == TipoDeConteudo.Button? buttomPrefab : fieldPrefab), Row).GetComponent<RectTransform>();
            
                field.sizeDelta = rt.sizeDelta;
                field.transform.localPosition = titleObj.transform.localPosition;
                localFieldPositions.Add(field.transform.localPosition);
            
            
                distanciaAtual += colunas[i].largura / 2;

            }
        
        Row.gameObject.SetActive(false);

    }

    public void Desenhar(List<List<Conteudo>> conteudos)
    {
        tableContent = conteudos;
        Desenhar();
    }
    public void Desenhar()
    {
        //limpa conteudos anteriores
        for (int i = content.childCount-1; i > 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        //organiza a tabela
        SortContent();

        float contentHeight = 0;
        int k = 0;
        foreach (List<Conteudo> valorDaLinha in tableContent)
        {
            Transform linha = Instantiate(Row, content).transform;
            for (int i = 0; i < linha.childCount; i++)
            {
                linha.GetChild(i).transform.localPosition = localFieldPositions[i];
                Text field = linha.GetChild(i).GetComponentInChildren<Text>();
                field.text = valorDaLinha[i].Value;
                
                if(valorDaLinha[i].Tipo == TipoDeConteudo.Dinheiro)
                {
                    //adiciona um filtro
                    field.text = FormatarParaReal(field.text);
                }
                else if (valorDaLinha[i].Tipo == TipoDeConteudo.Button)
                {
                    linha.GetChild(i).name = k.ToString();
                    linha.GetChild(i).GetComponent<TableActionButton>().Tabela = this;

                }
            }
            linha.gameObject.SetActive(true);
            contentHeight += 30;
            k++;
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight + ExtraContentSize); 
    }


    public void ChangeSortingMethod(int i)
    {
        //muda o método
        if (orderMethod == i)
            return;
        orderMethod = i;

        //redesenha
        Desenhar();
        
    }

    public void SortContent()
    {
        if (colunas[orderMethod].tipo == TipoDeConteudo.String)
            tableContent.Sort((a, b) => a[orderMethod].Value.CompareTo(b[orderMethod].Value));
        else if (colunas[orderMethod].tipo == TipoDeConteudo.Float)
            tableContent.Sort((a, b) => float.Parse(b[orderMethod].Value).CompareTo(float.Parse(a[orderMethod].Value)));
        else if (colunas[orderMethod].tipo == TipoDeConteudo.Data)
            tableContent.Sort((a, b) =>  new Data(b[orderMethod].Value).CompareTo(new Data(a[orderMethod].Value)));

    }

    public virtual void ButtomMethod(int i)
    {
        Debug.Log("Row " + i + " selecionado");
    } 

    private string FormatarParaReal(string s)
    {
        float f = float.Parse(s);
        string r = f.ToString("C");
        r = r.Substring(1, r.Length - 1);
        r = r.Replace(",", ".");

        int i = r.LastIndexOf(".");
        r = r.Substring(0, i) +","+ r.Substring(i+1, r.Length - i-1);

        return r;
    }
}
