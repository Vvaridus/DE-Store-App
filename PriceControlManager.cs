using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PriceControlManager : MonoBehaviour
{
    private static PriceControlManager instance;

    public static PriceControlManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PriceControlManager>();
            }

            return instance;
        }
    }

    private List<string> list = new List<string>();

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_Text text;    
    [SerializeField] private TMP_Text prodName;
    [SerializeField] private TMP_Text prodCost;
    [SerializeField] private TMP_Text prodStock;
    [SerializeField] private Toggle freeDelivery;
    [SerializeField] private Toggle bogof;
    [SerializeField] private Toggle threeForTwo;


    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
    }

    public void DropdownValueChanged(TMP_Dropdown change)
    {
        //changes
        StartCoroutine(AuthManager.MyInstance.LoadProductData(dropdown.options[dropdown.value].text));
    }

    public void ShowProductData(string _name, int _cost, int _stock, bool _freeDelivery, bool _bogof, bool _threeForTwo)
    {
        prodName.text = _name;
        prodCost.text = _cost.ToString();
        prodStock.text = _stock.ToString();
        freeDelivery.isOn = _freeDelivery;
        bogof.isOn = _bogof;
        threeForTwo.isOn = _threeForTwo;
    }

    public void PopulateDropdownMenu()
    {
        dropdown.options.Clear();
        list = AuthManager.MyInstance.productName;
        foreach (string item in list)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }
    }
}
