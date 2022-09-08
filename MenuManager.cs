using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup priceControlCanvasGroup;
    [SerializeField] private CanvasGroup loginCanvasGroupUI;
    [SerializeField] private CanvasGroup registerCanvasGroupUI;
    [SerializeField] private CanvasGroup addProductCanvasGroup;
    [SerializeField] private CanvasGroup stockControlCanvasGroup;
    [SerializeField] private CanvasGroup saleScreenCavasGroup;
    [SerializeField] private CanvasGroup popUpScreenCanvasGroup;
    [SerializeField] private CanvasGroup reportScreenCanvasGroup;
    [SerializeField] private CanvasGroup financeScreenCanvasGroup;
    [SerializeField] private CanvasGroup customerLoyaltyCardCanvasGroup;

    

    private static MenuManager instance;

    public static MenuManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
            }

            return instance;
        }
    }

    public void OpenPriceControlScreen()
    {
        priceControlCanvasGroup.alpha = 1;
        priceControlCanvasGroup.blocksRaycasts = true;
        priceControlCanvasGroup.interactable = true;

    }

    public void ClosePriceControlScreen()
    {
        priceControlCanvasGroup.alpha = 0;
        priceControlCanvasGroup.blocksRaycasts = false;
        priceControlCanvasGroup.interactable = false;
    }

    public void OpenMainMenu()
    {
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.blocksRaycasts = true;
        mainMenuCanvasGroup.interactable = true;
    }

    public void CloseMainMenu()
    {
        mainMenuCanvasGroup.alpha = 0;
        mainMenuCanvasGroup.blocksRaycasts = false;
        mainMenuCanvasGroup.interactable = false;
    }

    public void LoginScreenOpen()
    {
        loginCanvasGroupUI.alpha = 1;
        loginCanvasGroupUI.blocksRaycasts = true;
        loginCanvasGroupUI.interactable = true;
    }

    public void LoginScreenClose()
    {
        loginCanvasGroupUI.alpha = 0;
        loginCanvasGroupUI.blocksRaycasts = false;
        loginCanvasGroupUI.interactable = false;
    }

    public void RegisterScreenOpen()
    {
        registerCanvasGroupUI.alpha = 1;
        registerCanvasGroupUI.blocksRaycasts = true;
        registerCanvasGroupUI.interactable = true;
    }

    public void RegisterScreenClose()
    {
        registerCanvasGroupUI.alpha = 0;
        registerCanvasGroupUI.blocksRaycasts = false;
        registerCanvasGroupUI.interactable = false;
    }

    public void StockControlScreenOpen()
    {
        stockControlCanvasGroup.alpha = 1;
        stockControlCanvasGroup.blocksRaycasts = true;
        stockControlCanvasGroup.interactable = true;
    }

    public void StockControlScreenClosed()
    {
        stockControlCanvasGroup.alpha = 0;
        stockControlCanvasGroup.blocksRaycasts = false;
        stockControlCanvasGroup.interactable = false;
    }

    public void SalesScreenOpen()
    {
        saleScreenCavasGroup.alpha = 1;
        saleScreenCavasGroup.blocksRaycasts = true;
        saleScreenCavasGroup.interactable = true;
    }

    public void SalesScreenClosed()
    {
        saleScreenCavasGroup.alpha = 0;
        saleScreenCavasGroup.blocksRaycasts = false;
        saleScreenCavasGroup.interactable = false;
    }

    public void PopUpScreenOpen()
    {
        popUpScreenCanvasGroup.alpha = 1;
        popUpScreenCanvasGroup.blocksRaycasts = true;
        popUpScreenCanvasGroup.interactable = true;
    }

    public void PopUpScreenClose()
    {
        popUpScreenCanvasGroup.alpha = 0;
        popUpScreenCanvasGroup.blocksRaycasts = false;
        popUpScreenCanvasGroup.interactable = false;
    }

    public void ReportScreenOpen()
    {
        reportScreenCanvasGroup.alpha = 1;
        reportScreenCanvasGroup.blocksRaycasts = true;
        reportScreenCanvasGroup.interactable = true;
    }

    public void ReportScreenClosed()
    {
        reportScreenCanvasGroup.alpha = 0;
        reportScreenCanvasGroup.blocksRaycasts = false;
        reportScreenCanvasGroup.interactable = false;
    }

    public void FinanceReportScreenOpen()
    {
        financeScreenCanvasGroup.alpha = 1;
        financeScreenCanvasGroup.blocksRaycasts = true;
        financeScreenCanvasGroup.interactable = true;
    }

    public void FinanceReportScreenClosed()
    {
        financeScreenCanvasGroup.alpha = 0;
        financeScreenCanvasGroup.blocksRaycasts = false;
        financeScreenCanvasGroup.interactable = false;
    }

    public void customerScreenOpen()
    {
        customerLoyaltyCardCanvasGroup.alpha = 1;
        customerLoyaltyCardCanvasGroup.blocksRaycasts = true;
        customerLoyaltyCardCanvasGroup.interactable = true;
    }

    public void customerScreenClosed()
    {
        customerLoyaltyCardCanvasGroup.alpha = 0;
        customerLoyaltyCardCanvasGroup.blocksRaycasts = false;
        customerLoyaltyCardCanvasGroup.interactable = false;
    }

    public void OpenAddProductScreen()
    {
        addProductCanvasGroup.alpha = 1;
        addProductCanvasGroup.blocksRaycasts = true;
        addProductCanvasGroup.interactable = true;
    }
}
