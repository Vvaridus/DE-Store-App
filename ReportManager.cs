using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportManager : MonoBehaviour
{
    private static ReportManager instance;

    public static ReportManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ReportManager>();
            }

            return instance;
        }
    }

    [SerializeField] private TMP_Text titleOne, titleTwo, titleThree, titleFour;
    [SerializeField] private TMP_Text qtyOne, qtyTwo, qtyThree, qtyFour;

    public void UpdateReportData(string nameOne, string nameTwo, string nameThree, string nameFour, int one, int two, int three, int four)
    {
        titleOne.text = nameOne;        
        titleTwo.text = nameTwo;        
        titleThree.text = nameThree;        
        titleFour.text = nameFour;
        
        qtyOne.text = one.ToString();
        qtyTwo.text = two.ToString();
        qtyThree.text = three.ToString();
        qtyFour.text = four.ToString();
    }
}
