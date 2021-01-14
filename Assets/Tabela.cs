using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tabela : MonoBehaviour
{
    [System.Serializable]
    //(hovered item, hovered column)
    public class TableEvent : UnityEvent<int,int>
    {
    }
    public class Column
    {
        public string Title;
        public float Width;
        public ColumnTypes Type;

        public Column(string title, float width, ColumnTypes tipo)
        {
            Title = title;
            Width = width;
            Type = tipo;
        }
    }

    public struct TableItem
    {
        public string Value;
        public ColumnTypes Type;
        public TableItem(string v)
        {
            Value = v;
            Type = ColumnTypes.String;
        }
        public TableItem(Data d)
        {
            Value = d.ToString();
            Type = ColumnTypes.Data;
        }
        public TableItem(float v)
        {
            Value = v.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", ",");
            Type = ColumnTypes.Float;
        }
    }
    public enum ColumnTypes
    {
        String,
        Float,
        Data,
    }

    private List<Column> colunas;
    public List<List<TableItem>> tableContent;

    public float RowHeight = 15f;
    public int Margin = 2;
    public float sensitivity;

    public Scrollbar scrollBar;
    public Scrollbar visibleScrollBar;

    public RectTransform Header;
    private RectTransform Rect;
    private Canvas Canvas;

    public RectTransform ScrollView;
    public GridLayoutGroup grid;
    public RectTransform RightClickMenu;

    public Font Font;

    public bool preparado;

    private int realRows;
    private int totalBlocks;

    public int firstVisibleIndex = 0;
    public List<Text[]> TableFields;

    private List<Image> RowBackground;

    public TableEvent RightClickOptions;

    public int HoveredItemIndex, HoveredColumn;


    //Default functions
    public void Localizar(int a,int b)
    {
        FindMenu.transform.parent.gameObject.SetActive(true);
        FindMenu.Select();
    }
    public void Copiar(int a, int b)
    {
        TextEditor te = new TextEditor();
        if (tableContent.Count > a)
            te.text = tableContent[b][a].Value ;
        else
            te.text = "";

        te.SelectAll();
        te.Copy();
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
        RightClickOptions.Invoke(HoveredColumn,HoveredItemIndex);
    }
    IEnumerator HideRightClick()
    {
        yield return new WaitForSeconds(0.1f);
        RightClickMenu.gameObject.SetActive(false);
    }

    public void Preparar(List<Column> novasColunas)
    {
        colunas = novasColunas;

        tableContent = new List<List<TableItem>>();

        SearchResults = new List<int>();

        Rect = GetComponent<RectTransform>();
        Canvas = transform.root.GetComponent<Canvas>();

        //normalizar as dimensoes
        float total = 0;
        for (int i = 0; i < colunas.Count; i++)
        {
            total += colunas[i].Width;
        }
        float prop = 1 / total;
        for (int i = 0; i < colunas.Count; i++)
        {
            colunas[i].Width*= prop;
        }


        //cria um row-modelo
        GameObject RowPrefab = new GameObject("row", typeof(Image));
        RowPrefab.transform.SetParent( grid.transform);
        RowPrefab.transform.localScale = Vector3.one;

        //Titulo-modelo
        GameObject titlePrefab = Header.GetChild(0).gameObject;

        Color barraColor = gameObject.GetComponent<Image>().color;

        float totalMarginsWidth = (colunas.Count - 1) * Margin;

        //arruma o row-modelo e arruma o header
        float xPos = -ScrollView.sizeDelta.x/2 + Margin;
        for (int i = 0; i < colunas.Count; i++)
        {
            xPos += colunas[i].Width * ScrollView.sizeDelta.x/2;

            RectTransform txtObj = new GameObject("text", typeof(Text)).GetComponent<RectTransform>();
            txtObj.SetParent(RowPrefab.transform);
            
            

            RectTransform Title = Instantiate(titlePrefab, Header).GetComponent<RectTransform>();
            Title.name = i.ToString();

            
            txtObj.sizeDelta = new Vector2(colunas[i].Width * (ScrollView.sizeDelta.x -totalMarginsWidth ), RowHeight);
            Title.sizeDelta = new Vector2(colunas[i].Width * (ScrollView.sizeDelta.x - totalMarginsWidth), Header.sizeDelta.y);


            txtObj.localPosition = new Vector3(xPos, 0);
            Title.localPosition = new Vector3(xPos-Margin, 0);


            Text txt = txtObj.GetComponent<Text>();
            txt.font = Font;
            txt.color = Color.black;
            txt.alignment = TextAnchor.MiddleLeft;
            txt.resizeTextForBestFit = true;
            txt.resizeTextMinSize = (int)(RowHeight / 2);

            Title.GetComponentInChildren<Text>().text = colunas[i].Title;
            
            xPos += colunas[i].Width * ScrollView.sizeDelta.x/2;

            if (i < colunas.Count - 1)
            {
                RectTransform barra = new GameObject("Barra", typeof(RectTransform)).GetComponent<RectTransform>();
                barra.SetParent(Header);
                barra.gameObject.AddComponent<Image>().color = barraColor;
                barra.sizeDelta = new Vector2(Margin, Rect.sizeDelta.y);
                barra.localPosition = new Vector3(xPos-(Margin/2), 0);

                barra.position = new Vector3(barra.position.x, Rect.position.y, 0);

            }


        }

        //deleta o titulo-modelo
        Destroy(titlePrefab);


        grid.cellSize = new Vector2( Rect.sizeDelta.x ,RowHeight);
        grid.spacing = new Vector2(Margin, Margin);

        realRows  = (int)(ScrollView.sizeDelta.y / (RowHeight+Margin)) +1;
        ScrollView.sizeDelta = new Vector2(ScrollView.sizeDelta.x, (realRows-1) * (RowHeight + Margin));


        //cria os rows vazios
        RowBackground = new List<Image>();
        TableFields = new List<Text[]>();
        for (int i = 0; i < realRows; i++)
        {
            GameObject newRow = Instantiate(RowPrefab, grid.transform);
            newRow.SetActive(true);


            Text[] childstext = newRow.GetComponentsInChildren<Text>();
            TableFields.Add(childstext);
            RowBackground.Add(newRow.GetComponent<Image>());

        }

        //deleta o row-modelo
        Destroy(RowPrefab);


        RectTransform content = grid.GetComponent<RectTransform>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, realRows * (Margin + RowHeight));

        RightClickSetup();

        FindMenu.transform.parent.gameObject.SetActive(false);


        preparado = true;
    }

    public void Desenhar(List<List<TableItem>> novoConteudo )
    {
        tableContent = novoConteudo;

        

        SortBy(0);
    }
    public void UpdateTableVisual()
    {
        totalBlocks = (tableContent.Count >= realRows ? tableContent.Count - realRows + 1 : 1);

        visibleScrollBar.size = Mathf.Max(0.1f, 1f / totalBlocks);
        GoTo(firstVisibleIndex);
        OnScroll();
    }

    private void Update()
    {
        if (Mouse.IsHovering(gameObject) && totalBlocks > 1)
        {
            //desce um bloco por vez no scroll do mouse
            visibleScrollBar.value = Mathf.Clamp01(visibleScrollBar.value - Input.mouseScrollDelta.y * sensitivity/totalBlocks);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(HideRightClick());
        }
        else if (Input.GetMouseButtonUp(1) && Mouse.IsHovering(ScrollView.gameObject))
        {
            //ativa o menu de right-click
            RightClickMenu.gameObject.SetActive(true);

            //pos do mouse
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ScrollView as RectTransform, Input.mousePosition, Canvas.worldCamera, out pos);

            RightClickMenu.position = ScrollView.TransformPoint(pos) + new Vector3(RightClickMenu.sizeDelta.x / 2, RightClickMenu.sizeDelta.y / 2);

            Vector2Int clickInfo = GetHoveredInfo(pos);

            HoveredColumn = clickInfo.x;
            HoveredItemIndex = clickInfo.y;
        }
        else if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.F))
        {
            // CTRL + F
            Localizar(0, 0);
            StartCoroutine(HideRightClick());
        }
        else if (Input.GetKey(KeyCode.Escape)){
            StartCoroutine(HideRightClick());
            HideSearch();
        }
    }

    private Vector2Int GetHoveredInfo(Vector2 mouseCoordinates)
    {
        Vector2Int retorno = Vector2Int.zero;

        retorno.y = firstVisibleIndex + Mathf.RoundToInt(1-scrollBar.value) + (int)((-(mouseCoordinates.y - (ScrollView.sizeDelta.y / 2))) / ScrollView.sizeDelta.y * (realRows - 1));

        float mouseX = (mouseCoordinates.x + (ScrollView.sizeDelta.x / 2)) / ScrollView.sizeDelta.x;

        float somatorio = 0;
        for (int i = 0; i < colunas.Count; i++)
        {
            somatorio += colunas[i].Width;
            if (mouseX < somatorio)
            {
                retorno.x = i;
                return retorno;
            }
        }

        return retorno;

    }

    public void PopulateTableFrom(int start)
    {
        for (int i = 0; i < TableFields.Count ; i++)
        {
            for (int j = 0; j < colunas.Count; j++)
            {

                if(tableContent.Count > i + start)
                {
                    TableFields[i][j].text = tableContent[start + i][j].Value;
                }
                else
                {
                    TableFields[i][j].text = "";
                }
            }
        }

    }
    public void OnScroll()
    {
        if (preparado)
        {
            firstVisibleIndex = (int)(visibleScrollBar.value * totalBlocks);

            float visiblePart = (visibleScrollBar.value * totalBlocks) - firstVisibleIndex;

            scrollBar.value = 1-visiblePart;

            PopulateTableFrom(firstVisibleIndex);
            RemoveHighlight();
        }
    }

    int orderMethod = 0;
    bool inverseOrdering = false;
    public void SortBy(int f)
    {
        RemoveHighlight();

        if(f == orderMethod)
            inverseOrdering = !inverseOrdering;
        else
            inverseOrdering = false;

        orderMethod = f;

        if (inverseOrdering)
        {
            if (colunas[orderMethod].Type == ColumnTypes.String)
                tableContent.Sort((b, a) => a[orderMethod].Value.CompareTo(b[orderMethod].Value));
            else if (colunas[orderMethod].Type == ColumnTypes.Float)
                tableContent.Sort((a, b) => float.Parse(b[orderMethod].Value).CompareTo(float.Parse(a[orderMethod].Value, System.Globalization.CultureInfo.InvariantCulture)));
            else if (colunas[orderMethod].Type == ColumnTypes.Data)
                tableContent.Sort((a, b) => new Data(b[orderMethod].Value).CompareTo(new Data(a[orderMethod].Value)));
        }
        else
        {

            if (colunas[orderMethod].Type == ColumnTypes.String)
                tableContent.Sort((a, b) => a[orderMethod].Value.CompareTo(b[orderMethod].Value));
            else if (colunas[orderMethod].Type == ColumnTypes.Float)
                tableContent.Sort((b, a) => float.Parse(b[orderMethod].Value).CompareTo(float.Parse(a[orderMethod].Value, System.Globalization.CultureInfo.InvariantCulture)));
            else if (colunas[orderMethod].Type == ColumnTypes.Data)
                tableContent.Sort((b, a) => new Data(b[orderMethod].Value).CompareTo(new Data(a[orderMethod].Value)));
        }
        PopulateTableFrom(firstVisibleIndex);
        
    }

    private void RightClickSetup()
    {
        //Arruma o quadro de right click
        int numOfOptions = RightClickOptions.GetPersistentEventCount();
        if (numOfOptions > 0)
        {
            GameObject originalButtom = RightClickMenu.GetChild(0).gameObject;
            for (int l = 0; l < numOfOptions; l++)
            {
                GameObject effectButtom = (l == 0 ? originalButtom : Instantiate(originalButtom, RightClickMenu.transform));
                effectButtom.GetComponentInChildren<Text>().text = RightClickOptions.GetPersistentMethodName(l);
                effectButtom.name = l.ToString();

            }
            RightClickMenu.sizeDelta = new Vector2(RightClickMenu.sizeDelta.x, RowHeight * numOfOptions);
            RightClickMenu.SetAsLastSibling();
        }
        RightClickMenu.gameObject.SetActive(false);
    }

    bool theresABlockHighlighted = false;

    public void Highlight(int index)
    {
        RemoveHighlight();

        GoTo(index);

        int targettedBlock = index - firstVisibleIndex;
        
        RowBackground[targettedBlock].color = Color.yellow;

        theresABlockHighlighted = true;
    }

    public void RemoveHighlight()
    {
        if (theresABlockHighlighted)
        {
            foreach (var item in RowBackground)
            {
                item.color = Color.white;
            }
        }
    }

    public void GoTo(int index)
    {
        float valor = ((float)(index) / tableContent.Count);
        
        valor += (1f/ tableContent.Count)*valor*(realRows);

        valor = Mathf.Clamp01(valor);

        visibleScrollBar.value = valor;
        scrollBar.value = 1;
        OnScroll();
    }



    //findMenu
    public InputField FindMenu;
    private int currentSearchIndex;
    private List<int> SearchResults;
    private Text indicadorNumeroDeResultados;
    public void OnSearchEnter(string search)
    {
        currentSearchIndex = 0;
        SearchResults = new List<int>();
        int prioridade = HoveredColumn;
        for (int i = 0; i < tableContent.Count; i++)
        {
            if (tableContent[i][prioridade].Value.ToLower().Contains(search))
            {
                SearchResults.Add(i);
            }
        }

        for (int k = 0; k < colunas.Count; k++)
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

        if (!indicadorNumeroDeResultados)
            indicadorNumeroDeResultados = FindMenu.transform.parent.GetChild(4).GetComponent<Text>();

        indicadorNumeroDeResultados.text = "(" + (SearchResults.Count>999?"999+":SearchResults.Count.ToString()) + " resultados)";


        if (SearchResults.Count > 0)
        {
            Highlight(SearchResults[0]);
        }
        else
            RemoveHighlight();
    }
    public void HideSearch()
    {
        FindMenu.transform.parent.gameObject.SetActive(false);
    }
    public void UpSearch()
    {
        if (SearchResults.Count > 0)
        {
            currentSearchIndex--;
            if (currentSearchIndex < 0)
                currentSearchIndex = SearchResults.Count - 1;
            Highlight(SearchResults[currentSearchIndex]);
        }
    }
    public void DownSearch()
    {
        if (SearchResults.Count > 0)
        {
            currentSearchIndex++;
            if (currentSearchIndex > SearchResults.Count - 1)
                currentSearchIndex = 0;
            Highlight(SearchResults[currentSearchIndex]);
        }
    }

}
