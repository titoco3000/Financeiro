using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Input_Area : Dropdown
{

    private Color corHotel = new Color(1, 0.84f, 0.84f);
    private Color corRestaurante = new Color(0.9f, 0.9f, 1);

    private bool[] isHotel = new bool[18];
    private Text identificationLabel;

    protected override void Start()
    {
        //limpa o dropdown
        ClearOptions();
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();

        //preenche ele
        int i = 0;
        foreach (var item in System.Enum.GetNames(typeof(AreasDoHotel)))
        {
            if (item.Substring(0, 5) == "Hotel")
            {
                isHotel[i] = true;
                optionList.Add(new Dropdown.OptionData( AddSpacesToSentence(item.Substring(6,item.Length-6))));
            }
            else
            {
                optionList.Add(new Dropdown.OptionData( AddSpacesToSentence(item.Substring(12,item.Length-12))));
            }

            i++;
        }
        AddOptions(optionList);

        identificationLabel = transform.GetChild(0).GetComponent<Text>();
        base.Start();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (isHotel[value])
        {
            identificationLabel.text = "(Hotel)";
            identificationLabel.color = Color.red;
        }
        else
        {
            identificationLabel.text = "(Restaurante)";
            identificationLabel.color = Color.blue;
        }
    }

    public void SetValue(int i)
    {
        value = i;
        UpdateLabel();
    }


    IEnumerator FilterDropdown(GameObject lista)
    {
        //espera um frame
        yield return 0;

        Transform collection = lista.transform.GetChild(0).GetChild(0);
        for (int i = 1; i < collection.childCount; i++)
        {
            if (isHotel[i-1])
            {
                collection.GetChild(i).GetComponent<Toggle>().image.color = corHotel;
            }
            else
            {
                collection.GetChild(i).GetComponent<Toggle>().image.color = corRestaurante;
            }

        }
    }

    protected override GameObject CreateDropdownList(GameObject template)
    {

        GameObject l = base.CreateDropdownList(template);
        StartCoroutine(FilterDropdown(l));
        return l;
    }

    string AddSpacesToSentence(string text)
    {
        if (text.Length < 2)
            return text;
        
        int i = 1;
        while (i<text.Length)
        {
            if (char.IsUpper(text[i]))
            {
                text = text.Insert(i, " ");
                i++;
            }
            i++;
        }
        return text;
    }
}
