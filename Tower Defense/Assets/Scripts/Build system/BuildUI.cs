using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    public GameObject ObjectUIPrefab;
    public Transform BuildingsListUI;
    public MasterInfo MasterInfo;
    public GameObject Master;

    Transform objectUI;

    void Start()
    {
        string buildingName;
        for (int i = 0; i < MasterInfo.GetBuildingsSet().Length; i++)
        {
            objectUI = Instantiate<GameObject>(ObjectUIPrefab, BuildingsListUI.position, BuildingsListUI.rotation).transform;

            buildingName = MasterInfo.GetBuildingsSet()[i];
            objectUI.Find("Name").GetComponent<Text>().text = buildingName;
            objectUI.GetComponent<Button>().onClick.AddListener(() => Master.GetComponent<BuildObject>().StartBuilding(buildingName));
            objectUI.SetParent(BuildingsListUI);
        }
    }
}