using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerManager : MonoBehaviour
{  
    [SerializeField] private TMP_InputField customerFirstName, customerSecondName, customerLoyaltyCardID, customerCreditBalance, customerPurchaseTotal;


    public void UpdateCustomerData()
    {
        StartCoroutine(AuthManager.MyInstance.UpdateCustomerData(
            customerLoyaltyCardID.text, 
            customerFirstName.text, 
            customerSecondName.text, 
            customerCreditBalance.text, 
            customerPurchaseTotal.text));
    }
}

