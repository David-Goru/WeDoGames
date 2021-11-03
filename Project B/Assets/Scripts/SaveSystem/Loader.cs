using UnityEngine.UI;
using UnityEngine;

public class Loader : MonoBehaviour, IGameInitializer
{
    [SerializeField] private GameObject loaderUIPrefab;
    [SerializeField] private GameObject createUIPrefab;
    //[SerializeField] private GameObject loadUIPrefab;

    private string playerName;
    private GameObject ui;

    private bool isNameValid;

    private void tryToCreateGame()
    {
        if (isNameValid) createGame();
    }

    private void checkName(string s)
    {
        if (s != "") isNameValid = true;
        else isNameValid = false;
    }

    private void createGame()
    {
        playerName = ui.GetComponentInChildren<InputField>().text;

        ui.SetActive(false);

        GameObject game = transform.parent.gameObject;
        ILoadable[] gameComponents = game.GetComponentsInChildren<ILoadable>();
        foreach(ILoadable component in gameComponents)
        {
            component.Create();
        }
    }

    public void Initialize()
    {
        ui = Instantiate(loaderUIPrefab, GameObject.Find("UI").transform);

        //I need to improve this
        Button[] buttons = ui.GetComponentsInChildren<Button>();
        bool first = true;

        foreach(Button button in buttons)
        {
            if (first)
            {
                button.onClick.AddListener(() => displayCreateUI());
                first = false;
            }
            else button.onClick.AddListener(() => displayLoadUI());
        }
    }

    private void displayCreateUI()
    {
        ui.SetActive(false);

        ui = Instantiate(createUIPrefab, GameObject.Find("UI").transform);
        ui.GetComponentInChildren<Button>().onClick.AddListener(() => tryToCreateGame());
        ui.GetComponentInChildren<InputField>().onEndEdit.AddListener((string s) => checkName(s));
    }

    private void displayLoadUI()
    {
        //ui.SetActive(false);

        Debug.Log("You can't load anything yet");
    }
}