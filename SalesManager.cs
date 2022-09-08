using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalesManager : MonoBehaviour
{
    public void SellWood()
    {
        StartCoroutine(AuthManager.MyInstance.SellItem("Wood"));        
    }

    public void SellTiles()
    {
        StartCoroutine(AuthManager.MyInstance.SellItem("Tiles"));
    }

    public void SellPaint()
    {
        StartCoroutine(AuthManager.MyInstance.SellItem("Paint"));
    }

    public void SellVarnish()
    {
        StartCoroutine(AuthManager.MyInstance.SellItem("Varnish"));
    }
}
