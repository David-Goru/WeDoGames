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
    
    public void Initialize()
    {
        ui = Instantiate(loaderUIPrefab, GameObject.Find("UI").transform);

        Button[] buttons = ui.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(displayCreateUI);
        buttons[1].onClick.AddListener(displayLoadUI);
    }

    private void tryToCreateGame()
    {
        if (isNameValid) createGame();
    }

    private void checkName(string s)
    {
        isNameValid = s != "";
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

    private void displayCreateUI()
    {
        ui.SetActive(false);

        ui = Instantiate(createUIPrefab, GameObject.Find("UI").transform);
        ui.GetComponentInChildren<Button>().onClick.AddListener(tryToCreateGame);
        ui.GetComponentInChildren<InputField>().onEndEdit.AddListener(checkName);
    }

    private void displayLoadUI()
    {
        //ui.SetActive(false);

        Debug.Log("You can't load anything yet");
    }
}