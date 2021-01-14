using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tabela3 : MonoBehaviour
{
    #region classes
    [System.Serializable]
    //(hovered item, hovered column)
    public class TableEvent : UnityEvent<int, int>
    {
    }
    [System.Serializable]
    public class Coluna
    {
        public string Title;
        public Tipo Tipo;
        public float Width;
    }

    public enum Tipo
    {
        String,
        Data,
        Float
    }




    public struct TableItem
    {
        public string Value { get; set; }
        public Tipo Type;
        public TableItem(string v)
        {
            Value = v;
            Type = Tipo.String;
        }
        public TableItem(Data d)
        {
            Value = d.ToString();
            Type = Tipo.Data;
        }
        public TableItem(float v)
        {
            Value = v.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", ",");
            Type = Tipo.Float;
        }
        public void ModifyValue(string a)
        {
            Value = a;
        }
    }

    public class Campo
    {
        public Text Text;
        public Image Image;
        public Campo(Text t, Image i)
        {
            Text = t;
            Image = i;
        }
    }

    #endregion

    public Scrollbar bar;
    public RectTransform content;

    //prefabs
    public GameObject colunaPrefab;

    [HideInInspector]
    public float[] Largura;

    public float titleHeight = 20f;
    public float rowHeight = 15f;
    public int Margin = 2;

    public int numeroDeRows = 1;
    public int firstVisibleIndex = 0;

    public Coluna[] Colunas;

    public List<List<TableItem>> tableContent;

    public List<List<Campo>> Campos;

    public TableEvent RightClickOptions;

    public InputField FindMenu;
    public RectTransform Right_ClickMenu;

    public int HoveredItem, HoveredColumn;

    public int highlightedItem = -1;


    private void Update()
    {
        if (Mouse.IsHovering(this.gameObject))
        {
            //desce um bloco por vez no scroll do mouse
            if (numeroDeRows > 0)
            {
                bar.value = Mathf.Clamp(bar.value - Input.mouseScrollDelta.y / (2 * numeroDeRows),-1,1);
            }
        }
        if (Input.GetMouseButtonUp(1) && Mouse.IsHovering(content.gameObject))
        {
            //right-click menu
            Vector2 mousePos = MouseNormalizedPosition(content);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.root as RectTransform, Input.mousePosition, transform.root.GetComponent<Canvas>().worldCamera, out Vector2 pos);
            Right_ClickMenu.position = transform.root.transform.TransformPoint(pos) ;

            Right_ClickMenu.gameObject.SetActive(true);

            Vector2Int mouseInfo = GetClickIndex(mousePos);
            HoveredItem = firstVisibleIndex + mouseInfo.y;
            HoveredColumn = mouseInfo.x;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(HideRightClick());
        }
        else if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.F))
        {
            // CTRL + F
            Localizar(0, 0);
            StartCoroutine(HideRightClick());
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            StartCoroutine(HideRightClick());
            HideSearch();
        }
    }

    public void ApplyEfect(int i)
    {
        for (int k = 0; k < RightClickOptions.GetPersistentEventCount(); k++)
        {
            if (k != i)
                RightClickOptions.SetPersistentListenerState(k, UnityEventCallState.Off);
            else
                RightClickOptions.SetPersistentListenerState(k, UnityEventCallState.RuntimeOnly);
        }
        RightClickOptions.Invoke(HoveredColumn, HoveredItem);
    }

    IEnumerator HideRightClick()
    {
        yield return 0;
        Right_ClickMenu.gameObject.SetActive(false);
    }

    IEnumerator SelectSearch()
    {
        while(EventSystem.current.currentSelectedGameObject != FindMenu.gameObject)
        {
            FindMenu.Select();
            yield return 0;
        }
    }

    public void OnScroll(float v)
    {
        if(v < 0)
        {
            v = 0;
            bar.value = 0;
        }
        int totalParaSeConsiderar = Mathf.Max(tableContent.Count - numeroDeRows, 0);
        PopulateFrom( Mathf.RoundToInt(v * totalParaSeConsiderar));
    }

    public void PopulateFrom(int index)
    {
        if (tableContent.Count - index < numeroDeRows)
        {
            index = tableContent.Count - numeroDeRows;
        }
        if (index < 0)
        {
            index = 0;
        }

        firstVisibleIndex = index;


        for (int i = 0; i < numeroDeRows; i++)
        {
            for (int j = 0; j < Colunas.Length; j++)
            {
                if (tableContent.Count > i + index && tableContent[i].Count > j)
                    Campos[i][j].Text.text = tableContent[i + index][j].Value;
                else
                    Campos[i][j].Text.text = "";

                if (i + index == highlightedItem)
                    Campos[i][j].Image.color = Color.yellow;
                else
                    Campos[i][j].Image.color = Color.white;
            }
        }

    }

    public void GoTo(int index)
    {
        bar.value = ((float)(index)) / (float)(tableContent.Count - 1);
    }

    public void Desenhar(List<List<TableItem>> newContent)
    {
        numeroDeRows = content.GetChild(0).childCount-1;

        Campos = FetchCampos();
        Largura = GetNormalizedWidth();
        tableContent = newContent;
        PopulateFrom(0);

        if (tableContent.Count > 0)
            bar.size = Mathf.Max(20f / tableContent.Count, 0.01f);
        else
            bar.size = 1;

        //RightClickSetup();

    }
    
    private Vector2 MouseNormalizedPosition(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, transform.root.GetComponent<Canvas>().worldCamera, out Vector2 pos);
        return new Vector2((pos.x+rect.rect.width/2)/rect.rect.width,1 -(pos.y + rect.rect.height/2)/rect.rect.height);
    }

    private Vector2Int GetClickIndex(Vector2 normalMouse)
    {
        float y = Mathf.Clamp((normalMouse.y * (content.rect.height) - titleHeight) / (rowHeight + Margin),0,numeroDeRows-1);
        //x
        float xPos = 0;
        for (int i = 0; i < Colunas.Length; i++)
        {
            xPos += Largura[i];
            if (normalMouse.x < xPos)
            {
                return new Vector2Int(i,(int)y);
            }
        }
        return new Vector2Int(Colunas.Length-1,(int)y);
    }
    private List<List<Campo>> FetchCampos()
    {
        List<List<Campo>> output = new List<List<Campo>>();
        for (int i = 0; i < numeroDeRows; i++)
        {
            List<Campo> row = new List<Campo>();
            for (int j = 0; j < Colunas.Length; j++)
            {
                row.Add(new Campo(content.GetChild(j).GetChild(i + 1).GetComponentInChildren<Text>(), content.GetChild(j).GetChild(i + 1).GetComponent<Image>() ));
            }
            output.Add(row);
        }
        return output;
    }

    

    public void HighLight(int index)
    {
        highlightedItem = index;
        PopulateFrom(firstVisibleIndex);
    }

    private int orderMethod = 0;
    private bool inverseOrdering = false;
    public void SortBy(int f)
    {
        if (f == orderMethod)
            inverseOrdering = !inverseOrdering;
        else
            inverseOrdering = false;

        orderMethod = f;

        if (inverseOrdering)
        {
            if (Colunas[orderMethod].Tipo == Tipo.String)
                tableContent.Sort((b, a) => a[orderMethod].Value.CompareTo(b[orderMethod].Value));
            else if (Colunas[orderMethod].Tipo == Tipo.Float)
                tableContent.Sort((a, b) => Ultilities.ParseFloat(b[orderMethod].Value).CompareTo(Ultilities.ParseFloat(a[orderMethod].Value)));
            else if (Colunas[orderMethod].Tipo == Tipo.Data)
                tableContent.Sort((a, b) => new Data(b[orderMethod].Value).CompareTo(new Data(a[orderMethod].Value)));
        }
        else
        {

            if (Colunas[orderMethod].Tipo == Tipo.String)
                tableContent.Sort((a, b) => a[orderMethod].Value.CompareTo(b[orderMethod].Value));
            else if (Colunas[orderMethod].Tipo == Tipo.Float)
                tableContent.Sort((b, a) => Ultilities.ParseFloat(b[orderMethod].Value).CompareTo(Ultilities.ParseFloat(a[orderMethod].Value)));
            else if (Colunas[orderMethod].Tipo == Tipo.Data)
                tableContent.Sort((b, a) => new Data(b[orderMethod].Value).CompareTo(new Data(a[orderMethod].Value)));
        }
        PopulateFrom(firstVisibleIndex);
    }


    #region FindMenu
    private int currentSearchIndex;
    private List<int> SearchResults;
    private Text indicadorNumeroDeResultados;
    public void OnSearchEnter(string search)
    {
        currentSearchIndex = 0;
        SearchResults = new List<int>();

        if (!indicadorNumeroDeResultados)
            indicadorNumeroDeResultados = FindMenu.transform.parent.GetChild(4).GetComponent<Text>();


        if (search != "")
        {
            int prioridade = HoveredColumn;
            for (int i = 0; i < tableContent.Count; i++)
            {
                if (tableContent[i][prioridade].Value.ToLower().Contains(search))
                {
                    SearchResults.Add(i);
                }
            }

            for (int k = 0; k < Colunas.Length; k++)
            {
                if (k != prioridade)
                {
                    for (int i = 0; i < tableContent.Count; i++)
                    {
                        if (tableContent[i][k].Value.ToLower().Contains(search))
                        {
                            SearchResults.Add(i);
                        }
                    }
                }
            }
        }


        if (SearchResults.Count > 0)
        {
            indicadorNumeroDeResultados.text = "(" + (currentSearchIndex + 1) + "/" + (SearchResults.Count > 999 ? "999+" : SearchResults.Count.ToString()) + ")";
            GoTo(SearchResults[currentSearchIndex]);
            HighLight(SearchResults[currentSearchIndex]);
        }
        else
        {
            indicadorNumeroDeResultados.text = "";
            HighLight(-1);
        }

    }
    public void HideSearch()
    {
        FindMenu.transform.parent.gameObject.SetActive(false);
        HighLight(-1);
    }
    public void UpSearch()
    {
        if (SearchResults.Count > 0)
        {
            currentSearchIndex--;
            if (currentSearchIndex < 0)
                currentSearchIndex = SearchResults.Count - 1;
            GoTo(SearchResults[currentSearchIndex]);
            HighLight(SearchResults[currentSearchIndex]);
            indicadorNumeroDeResultados.text = "(" + (currentSearchIndex + 1) + "/" + (SearchResults.Count > 999 ? "999+" : SearchResults.Count.ToString()) + ")";

        }
    }
    public void DownSearch()
    {
        if (SearchResults.Count > 0)
        {
            currentSearchIndex++;
            if (currentSearchIndex > SearchResults.Count - 1)
                currentSearchIndex = 0;
            GoTo(SearchResults[currentSearchIndex]);
            HighLight(SearchResults[currentSearchIndex]);
            indicadorNumeroDeResultados.text = "(" + (currentSearchIndex + 1) + "/" + (SearchResults.Count > 999 ? "999+" : SearchResults.Count.ToString()) + ")";

        }
    }
    #endregion


    #region Built-in right-click options
    public void Localizar(int a, int b)
    {
        FindMenu.transform.parent.gameObject.SetActive(true);
        StartCoroutine(SelectSearch());
    }
    public void Copiar(int a, int b)
    {
        TextEditor te = new TextEditor();
        if (tableContent.Count > a)
            te.text = tableContent[b][a].Value;
        else
            te.text = "";

        te.SelectAll();
        te.Copy();
    }
    #endregion


    #region Editor

    private void RightClickSetup()
    {
        //Arruma o quadro de right click

        int numOfOptions = RightClickOptions.GetPersistentEventCount();


        if (numOfOptions > 0)
        {
            GameObject originalButtom = Right_ClickMenu.GetChild(0).gameObject;
            for (int l = 0; l < numOfOptions; l++)
            {
                GameObject effectButtom = (l == 0 ? originalButtom : Instantiate(originalButtom, Right_ClickMenu.transform));
                effectButtom.GetComponentInChildren<Text>().text = RightClickOptions.GetPersistentMethodName(l);
                effectButtom.name = l.ToString();

            }
            Right_ClickMenu.sizeDelta = new Vector2(Right_ClickMenu.sizeDelta.x, Right_ClickMenu.GetComponent<GridLayoutGroup>().cellSize.y * numOfOptions);
            Right_ClickMenu.SetAsLastSibling();
        }
        else
        {
            Right_ClickMenu.sizeDelta = Vector2.zero;
        }
        Right_ClickMenu.gameObject.SetActive(false);
    }
    public void ConstruirTabela()
    {
        Limpar();
        Largura = GetNormalizedWidth();

        //Cria uma coluna-modelo
        RectTransform colunaModelo = Instantiate(colunaPrefab, content).GetComponent<RectTransform>();
        VerticalLayoutGroup colunaModeloGrid = colunaModelo.GetComponent<VerticalLayoutGroup>();
        colunaModeloGrid.padding.left = Margin;
        colunaModeloGrid.spacing = Margin;

        RectTransform titleBase = colunaModelo.GetChild(0).GetComponent<RectTransform>();
        titleBase.sizeDelta = new Vector2(colunaModelo.rect.width, titleHeight);
        
        float alturaDisponivel = content.rect.height - titleBase.rect.height-Margin;

        numeroDeRows = (int)(alturaDisponivel / (rowHeight + Margin));

        RectTransform campoBase = colunaModelo.GetChild(1).GetComponent<RectTransform>();
        campoBase.sizeDelta = new Vector2(colunaModelo.rect.width, (alturaDisponivel/numeroDeRows)-Margin);


        for (int i = 1; i < numeroDeRows; i++)
        {
            Instantiate(campoBase, colunaModelo);
        }

        for (int i = 0; i < Colunas.Length; i++)
        {
            RectTransform coluna = i==0? colunaModelo :Instantiate(colunaModelo, content).GetComponent<RectTransform>();
            coluna.sizeDelta = new Vector2(content.rect.width * Largura[i], content.rect.height);
            coluna.GetChild(0).GetComponentInChildren<Text>().text = Colunas[i].Title;
            coluna.GetChild(0).gameObject.name = i.ToString();
        }

        RightClickSetup();
    }
    void Limpar()
    {
        //limpa
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(content.GetChild(i).gameObject);
        }
        for (int i = Right_ClickMenu.childCount - 1; i >= 1; i--)
        {
            DestroyImmediate(Right_ClickMenu.GetChild(i).gameObject);
        }

    }
    private float[] GetNormalizedWidth()
    {
        //normaliza as larguras
        float[] ret = new float[Colunas.Length];
        float somatorio = 0;
        for (int i = 0; i < Colunas.Length; i++)
        {
            if (Colunas[i].Width <= 0)
                Colunas[i].Width = 1;
            somatorio += Colunas[i].Width;
        }
        for (int i = 0; i < Colunas.Length; i++)
        {
            ret[i] = Colunas[i].Width / somatorio;
        }
        return ret;
    }

    #endregion 

}
