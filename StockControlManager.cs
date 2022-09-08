using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StockControlManager : MonoBehaviour
{
    private static StockControlManager instance;

    public static StockControlManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StockControlManager>();
            }

            return instance;
        }
    }


    [SerializeField] private TMP_Text productOneName;
    [SerializeField] private TMP_Text productTwoName;
    [SerializeField] private TMP_Text productThreeName;
    [SerializeField] private TMP_Text productFourName;

    [SerializeField] private TMP_Text productOneStock;
    [SerializeField] private TMP_Text productTwoStock;
    [SerializeField] private TMP_Text productThreeStock;
    [SerializeField] private TMP_Text productFourStock;

    [SerializeField] private Image warningImageOne;
    [SerializeField] private Image warningImageTwo;
    [SerializeField] private Image warningImageThree;
    [SerializeField] private Image warningImageFour;

    [SerializeField] private TMP_Text lowStockText;

    private int minStock = 10;

    public void LoadStock(string _nameOne, string _nameTwo, string _nameThree, string _nameFour)
    {
        productOneName.text = _nameOne;
        productTwoName.text = _nameTwo;
        productThreeName.text = _nameThree;
        productFourName.text = _nameFour;
    }

    public void LoadStockAmount(int _stockOne, int _stockTwo, int _stockThree, int _stockFour)
    {
        productOneStock.text = _stockOne.ToString();
        productTwoStock.text = _stockTwo.ToString();
        productThreeStock.text = _stockThree.ToString();
        productFourStock.text = _stockFour.ToString();

        
        if (_stockOne < minStock)
        {
            warningImageOne.enabled = true;
            MenuManager.MyInstance.PopUpScreenOpen();
            lowStockText.text = "Warning Email Sent... \n\nLow Stock of: " + productOneName.text;
        }
        else
        {
            warningImageOne.enabled = false;
        }


        if (_stockTwo < minStock)
        {
            warningImageTwo.enabled = true;
            MenuManager.MyInstance.PopUpScreenOpen();
            lowStockText.text = "Warning Email Sent... \n\nLow Stock of: " + productTwoName.text;
        }
        else
        {
            warningImageTwo.enabled = false;
        }


        if (_stockThree < minStock)
        {
            warningImageThree.enabled = true;
            MenuManager.MyInstance.PopUpScreenOpen();
            lowStockText.text = "Warning Email Sent... \n\nLow Stock of: " + productThreeName.text;
        }
        else
        {
            warningImageThree.enabled = false;
        }

        if (_stockFour < minStock)
        {
            warningImageFour.enabled = true;
            MenuManager.MyInstance.PopUpScreenOpen();
            lowStockText.text = "Warning Email Sent... \n\nLow Stock of: " + productFourName.text;
        }
        else
        {
            warningImageFour.enabled = false;
        }
    }
}
