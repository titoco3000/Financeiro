using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/*
 gerencia o autocomplete de um field
 Deriva-se deste script
 */
public class AutocompleteField : MonoBehaviour
{
    //referencias internas
    public SkippableInputField field;
    public RectTransform matchesList;
    public GridLayoutGroup content;
    public RectTransform rectTransform;

    //referencias externas
    public GameObject listItemPrefab;

    //configurações
    public bool valorConhecidoObrigatorio = false;
    public bool exibirSugestoes = true;
    public bool sugerirEmTodaOcasiao = false;
    public bool fullSugestionList = false;
    public int maxSugestoesExibidas = 10;
    public int listItemHeight = 21;


    public List<string> ValoresASeremProcurados;
    
    //isso é uma lista de indexes
    public List<int> sugestoesAtuais;

    public UnityEvent OnEndEditEvent;

    private ScrollRect scroll;



    public void Awake()
    {
        if(ValoresASeremProcurados == null)
            ValoresASeremProcurados = new List<string>();

        scroll = matchesList.GetComponent<ScrollRect>();

        content.cellSize = new Vector2(rectTransform.rect.width, listItemHeight);

        //prepara o onSelect
        field.OnSelectEvent.AddListener(new UnityEngine.Events.UnityAction(OnSelect));

        StartCoroutine(SecondFrame());
    }

    IEnumerator SecondFrame()
    {
        yield return 0;
        field.DeactivateInputField();
        matchesList.gameObject.SetActive(false);
    }


    public virtual void OnEdit()
    {
        ExibirSugestoes();
        
    }

    public void ExibirSugestoes()
    {
        string textoFiltrado = field.text;
        textoFiltrado = textoFiltrado.Replace(";", "");
        textoFiltrado = textoFiltrado.Replace(",", "");


        //Verifica se deve procurar sugestões
        if (textoFiltrado != "" || sugerirEmTodaOcasiao)
        {
            //procura sugestões
            sugestoesAtuais = NovasSugestoes(textoFiltrado);
        }
        else
            sugestoesAtuais = new List<int>();

        //exibe as sugestões
        if (sugestoesAtuais.Count > 0)
        {
            //limpa a lista
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }



            //volta para o começo da lista
            scroll.verticalNormalizedPosition = 1;


            int sugestoes = Mathf.Clamp(sugestoesAtuais.Count, 0, maxSugestoesExibidas);


            if(exibirSugestoes){matchesList.gameObject.SetActive(true);}

            float height = content.cellSize.y * sugestoes;

            matchesList.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            matchesList.localPosition = new Vector2(0, -(height + rectTransform.rect.height) / 2);

            scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, content.cellSize.y * sugestoesAtuais.Count);

            for (int i = 0; i < sugestoesAtuais.Count; i++)
            {
                //adiciona o item
                GameObject item = Instantiate(listItemPrefab, content.transform);
                //configura o item
                item.GetComponentInChildren<Text>().text = ValoresASeremProcurados[sugestoesAtuais[i]];
            }
        }
        else
        {
            matchesList.gameObject.SetActive(false);
        }

        field.text = textoFiltrado;
    }

    public void OnEndEdit()
    {
        //Verifica se o mouse está sobre a lista de sugestões
        if (!Mouse.IsHovering(matchesList.gameObject))
        {
            matchesList.gameObject.SetActive(false);
            //Verifica se precisa completar
            if (valorConhecidoObrigatorio && sugestoesAtuais.Count>0 && !ValoresASeremProcurados.Contains(field.text))
            {
                Autocompletar(ValoresASeremProcurados[sugestoesAtuais[0]]);
                return;
            }
            else
            {
                //Aqui é onde REALMENTE termina

                OnEndEditEvent.Invoke();

            }
        }
    }

    public virtual void OnSelect()
    {
        if (sugerirEmTodaOcasiao)
        {
            ExibirSugestoes();
        }
    }

    List<int> NovasSugestoes(string textoBase)
    {
        textoBase = textoBase.ToLower();
        List<int> retorno = new List<int>();
        if (textoBase == "")
            return retorno;
        for (int i = 0; i < ValoresASeremProcurados.Count; i++)
        {
            if (ValoresASeremProcurados[i].ToLower().Contains(textoBase))
            {
                retorno.Add(i);
            }

        }


        if (fullSugestionList)
        {
            for (int i = 0; i < ValoresASeremProcurados.Count; i++)
            {
                if (!retorno.Contains(i))
                {
                    retorno.Add(i);
                }
            }
        }

        return retorno;
    }

    //ativado por items da lista de sugestões

    public virtual void Autocompletar(string valor)
    {
        field.text = valor;
        matchesList.gameObject.SetActive(false);
        OnEndEdit();
        //OnEndEditEvent.Invoke();
    }

}
