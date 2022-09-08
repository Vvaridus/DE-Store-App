using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddProductScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField name;
    [SerializeField] private TMP_InputField price;
    [SerializeField] private TMP_InputField stock;
    [SerializeField] private TMP_InputField newPrice;
    [SerializeField] private Toggle freeDelivery;
    [SerializeField] private Toggle bogof;
    [SerializeField] private Toggle threeForTwo;


    public void SubmitProduct()
    {
        int priceOne = int.Parse(price.text);
        int stockOne = int.Parse(stock.text);
        AuthManager.MyInstance.SaveData(name.text, priceOne, stockOne, freeDelivery.isOn, bogof.isOn, threeForTwo.isOn);
    }
}
