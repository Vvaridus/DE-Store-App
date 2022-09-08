using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    public static AuthManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AuthManager>();
            }

            return instance;
        }
    }

    [SerializeField] private CanvasGroup loginCanvasGroup;
    [SerializeField] private CanvasGroup registerCanvasGroup;
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;

    [SerializeField] private TMP_InputField newPriceInput;
    [SerializeField] private Toggle freeDeliveryToggle;
    [SerializeField] private Toggle bogofToggle;
    [SerializeField] private Toggle threeForTwoToggle;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TextMeshProUGUI stockText;

    public List<string> productName = new List<string>();

    private bool loaded = false;

    //Firebase Vars
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference databaseReference;

    //Login Vars
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register Vars
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("UserData")]
    public TextMeshProUGUI usernameDisplay;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    public void SignOutButton()
    {
        auth.SignOut();
        usernameDisplay.text = "";
        MenuManager.MyInstance.LoginScreenOpen();
        MoveToLogin();
        ClearLoginFields();
        ClearRegisterFields();
    }

    public void SubmitUpdatedData()
    {
        string _name = dropdown.options[dropdown.value].text;
        int _price = int.Parse(newPriceInput.text);
        int _stock = int.Parse(stockText.text);
        bool _delivery = freeDeliveryToggle.isOn;
        bool _bogof = bogofToggle.isOn;
        bool _threeForTwo = threeForTwoToggle.isOn;

        Debug.Log("NAME: " + dropdown.options[dropdown.value].text);

        SaveData(_name, _price, _stock, _delivery, _bogof, _threeForTwo);
    }

    public void SaveData(string productName, int productPrice, int currentStock, bool _delivery, bool _bogof, bool _threeForTwo)
    {
        StartCoroutine(UpdateUsernameAuth(user.DisplayName));

        //wip
        //StartCoroutine(UpdateUsernameDatabase(user.DisplayName));

        StartCoroutine(UpdateProductName(productName));
        StartCoroutine(UpdateProductPrice(productPrice, productName));
        StartCoroutine(UpdateCurrentStock(currentStock, productName));
        StartCoroutine(UpdateFreeDelivery(productName, _delivery));
        StartCoroutine(UpdateBOGOF(productName, _bogof));
        StartCoroutine(UpdateThreeForTwo(productName, _threeForTwo));

        StartCoroutine(LoadProductData(dropdown.options[dropdown.value].text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            user = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logging in...";

            usernameDisplay.text = user.DisplayName;

            StartCoroutine(LoadUserData());

            yield return new WaitForSeconds(2);

            confirmLoginText.text = "";
            LoginMoveToMain();
            ClearLoginFields();
            ClearRegisterFields();
            Debug.Log(user.DisplayName);
        }
    }

    private void LoginMoveToMain()
    {
        loginCanvasGroup.alpha = 0;
        loginCanvasGroup.blocksRaycasts = false;
        loginCanvasGroup.interactable = false;
        registerCanvasGroup.alpha = 0;
        registerCanvasGroup.blocksRaycasts = false;
        registerCanvasGroup.interactable = false;
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.blocksRaycasts = true;
        mainMenuCanvasGroup.interactable = true;        
    }

    public void MoveToLogin()
    {
        loginCanvasGroup.alpha = 1;
        loginCanvasGroup.blocksRaycasts = true;
        loginCanvasGroup.interactable = true;
        //registerCanvasGroup.alpha = 1;
        //registerCanvasGroup.blocksRaycasts = true;
        //registerCanvasGroup.interactable = true;
        mainMenuCanvasGroup.alpha = 0;
        mainMenuCanvasGroup.blocksRaycasts = false;
        mainMenuCanvasGroup.interactable = false;
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningRegisterText.text = "Password does not match";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                user = RegisterTask.Result;

                if (user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };
                    var ProfileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseException = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                        warningRegisterText.text = "Username Set Failed";
                    }
                    else
                    {
                        warningRegisterText.text = "Registered";
                        ClearLoginFields();
                        ClearRegisterFields();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        UserProfile profile = new UserProfile { DisplayName = _username };

        var ProfileTask = user.UpdateUserProfileAsync(profile);

        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //username is updated
        }
    }

    //private IEnumerator UpdateUsernameDatabase(string _username)
    //{
    //    var DBTask = databaseReference.Child("users").Child(user.UserId).Child("username").SetValueAsync(_username);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //database username updated
    //    }
    //}

    private IEnumerator UpdateProductName(string _productName)
    {
        //Debug.Log(user.DisplayName + " : " + _productName + " : " + databaseReference);
        var DBTask = databaseReference.Child("productName").Child(_productName).Child("name").SetValueAsync(_productName);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //product name updated
        }
    }

    private IEnumerator UpdateProductPrice(int _productPrice, string _name)
    {
        var DBTask = databaseReference.Child("productName").Child(_name).Child("price").SetValueAsync(_productPrice);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //price updated
        }
    }

    private IEnumerator UpdateCurrentStock(int _currentStock, string _name)
    {
        var DBTask = databaseReference.Child("productName").Child(_name).Child("stock").SetValueAsync(_currentStock);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //stock updated
        }
    }

    private IEnumerator UpdateFreeDelivery(string _productName, bool _checked)
    {        
        var DBTask = databaseReference.Child("productName").Child(_productName).Child("freeDelivery").SetValueAsync(_checked);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //product delivery updated
        }
    }

    private IEnumerator UpdateBOGOF(string _productName, bool _checked)
    {
        var DBTask = databaseReference.Child("productName").Child(_productName).Child("bogof").SetValueAsync(_checked);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //product BOGOF updated
        }
    }

    private IEnumerator UpdateThreeForTwo(string _productName, bool _checked)
    {
        var DBTask = databaseReference.Child("productName").Child(_productName).Child("threeForTwo").SetValueAsync(_checked);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //product Three For Two updated
        }
    }

    public IEnumerator LoadProductData(string _name)
    {
        var DBTask = databaseReference.Child("productName").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            int cost = int.Parse(snapshot.Child(_name).Child("price").Value.ToString());
            int stock = int.Parse(snapshot.Child(_name).Child("stock").Value.ToString());
            bool delivery = snapshot.Child(_name).Child("freeDelivery").Value.Equals(true);
            bool bogof = snapshot.Child(_name).Child("bogof").Value.Equals(true);
            bool threeForTwo = snapshot.Child(_name).Child("threeForTwo").Value.Equals(true);            

            PriceControlManager.MyInstance.ShowProductData(_name, cost, stock, delivery, bogof, threeForTwo);
        }
    }

    public IEnumerator LoadUserData()
    {
        var DBTask = databaseReference.Child("productName").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            string name = snapshot.Child("Tiles").Child("name").Value.ToString();
            string nametwo = snapshot.Child("Wood").Child("name").Value.ToString();
            string namethree = snapshot.Child("Paint").Child("name").Value.ToString();
            string namefour = snapshot.Child("Varnish").Child("name").Value.ToString();

            if (loaded != true)
            {
                productName.Add(name);
                productName.Add(nametwo);
                productName.Add(namethree);
                productName.Add(namefour);
                loaded = true;
            }            

            int productStockOne = int.Parse(snapshot.Child(name).Child("stock").Value.ToString());
            int productStockTwo = int.Parse(snapshot.Child(nametwo).Child("stock").Value.ToString());
            int productStockThree = int.Parse(snapshot.Child(namethree).Child("stock").Value.ToString());
            int productStockFour = int.Parse(snapshot.Child(namefour).Child("stock").Value.ToString());

            StockControlManager.MyInstance.LoadStock(name, nametwo, namethree, namefour);
            StockControlManager.MyInstance.LoadStockAmount(productStockOne, productStockTwo, productStockThree, productStockFour);
        }
    }

    public IEnumerator SellItem(string _name)
    {
        var DBTask = databaseReference.Child("productName").GetValueAsync();

        var DBTaskSalesData = databaseReference.Child("Sales").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            DataSnapshot salesSnapshot = DBTaskSalesData.Result;

            int productStockOne = int.Parse(snapshot.Child(_name).Child("stock").Value.ToString());

            productStockOne -= 1;

            StartCoroutine(UpdateCurrentStock(productStockOne, _name));

            yield return new WaitForSeconds(1);

            StartCoroutine(LoadUserData());

            int sale = int.Parse(salesSnapshot.Child(_name).Child("Sold").Value.ToString());

            sale += 1;

            StartCoroutine(UpdateSalesData(_name, sale));

        }
    }

    public IEnumerator UpdateSalesData(string _name, int sale)
    {
        var DBTask = databaseReference.Child("Sales").Child(_name).Child("Sold").SetValueAsync(sale);

        var DBTaskSalesData = databaseReference.Child("Sales").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot salesSnapshot = DBTaskSalesData.Result;

            int qtyOne = int.Parse(salesSnapshot.Child("Paint").Child("Sold").Value.ToString());
            int qtyTwo = int.Parse(salesSnapshot.Child("Tiles").Child("Sold").Value.ToString());
            int qtyThree = int.Parse(salesSnapshot.Child("Varnish").Child("Sold").Value.ToString());
            int qtyFour = int.Parse(salesSnapshot.Child("Wood").Child("Sold").Value.ToString());

            ReportManager.MyInstance.UpdateReportData("Paint: ", "Tiles: ", "Varnish: ", "Wood: ", qtyOne, qtyTwo, qtyThree, qtyFour);
            //Sales updated
        }
    }

    public IEnumerator UpdateCustomerData(string loyaltyCardID, string firstName, string secondName, string creditTotal, string purchasesTotal)
    {
        var DBTask = databaseReference.Child("Customers").Child("ID").Child("ID").SetValueAsync(loyaltyCardID);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {   
            var DBfistNameTask = databaseReference.Child("Customers").Child("ID").Child(loyaltyCardID).Child("FirstName").SetValueAsync(firstName);

            yield return new WaitUntil(predicate: () => DBfistNameTask.IsCompleted);

            if (DBfistNameTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBfistNameTask.Exception}");
            }
            else
            {
                var DBsecondNameTask = databaseReference.Child("Customers").Child("ID").Child(loyaltyCardID).Child("SecondName").SetValueAsync(secondName);

                yield return new WaitUntil(predicate: () => DBsecondNameTask.IsCompleted);

                if (DBsecondNameTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBsecondNameTask.Exception}");
                }
                else
                {
                    var DBceditTotalTask = databaseReference.Child("Customers").Child("ID").Child(loyaltyCardID).Child("Credit").SetValueAsync(creditTotal);

                    yield return new WaitUntil(predicate: () => DBceditTotalTask.IsCompleted);

                    if (DBceditTotalTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBceditTotalTask.Exception}");
                    }
                    else
                    {
                        var DBpurchaseTask = databaseReference.Child("Customers").Child("ID").Child(loyaltyCardID).Child("Purchases").SetValueAsync(purchasesTotal);

                        yield return new WaitUntil(predicate: () => DBpurchaseTask.IsCompleted);

                        if (DBpurchaseTask.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task with {DBpurchaseTask.Exception}");
                        }
                        else
                        {
                            //Customer Updated
                        }
                    }
                }
            }
            
        }
    }
}
