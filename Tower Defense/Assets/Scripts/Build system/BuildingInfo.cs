using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingInfo", order = 0)]
public class BuildingInfo : ScriptableObject
{
    [HideInInspector] public List<Stat> Stats;
    [SerializeField] protected Pool buildingPool = null;
    [SerializeField] protected Pool buildingBlueprintPool = null;

    public Pool GetBuildingPool() { return buildingPool; }
    public Pool GetBuildingBlueprintPool() { return buildingBlueprintPool; }
    //public float GetStat(string statName) { return Stats[statName].StatValue; }
}

[CustomEditor(typeof(BuildingInfo))]
public class BuildingInfoEditor : Editor
{
    BuildingInfo buildingInfo;
    List<int> statsIndices;
    List<string> statChoices;
    List<string> temp = new List<string>() { "Sergio", "Cristian", "Arturo", "David" };
    List<string> statValues;

    public void OnEnable()
    {
        buildingInfo = (TurretInfo)target;
        if (buildingInfo.Stats == null)
        {
            buildingInfo.Stats = new List<Stat>();
        }

        statChoices = new List<string>(temp);
        statsIndices = new List<int>();
        statValues = new List<string>();
        foreach (var stat in buildingInfo.Stats.ToArray())
        {
            if (temp.Exists(x => x == stat.StatName))
            {
                statChoices.Remove(stat.StatName);
                statsIndices.Add(temp.FindIndex(x => x == stat.StatName));
                statValues.Add(stat.StatValue.ToString());
            }
            else
            {
                buildingInfo.Stats.Remove(stat);
            }
        }
    }

    public void OnDisable()
    {
        SaveStats();
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        if (GUILayout.Button("Add stat")) AddStat();

        for (int i = 0; i < buildingInfo.Stats.Count; i++)
        {
            if (statsIndices[i] == -1) statsIndices[i] = 0;

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            string oldStat = buildingInfo.Stats[i].StatName;
            statChoices.Add(oldStat);
            statsIndices[i] = EditorGUILayout.Popup(statChoices.IndexOf(buildingInfo.Stats[i].StatName), statChoices.ToArray(), GUILayout.Width(150), GUILayout.Height(30));
            buildingInfo.Stats[i].StatName = statChoices[statsIndices[i]];

            if (oldStat != buildingInfo.Stats[i].StatName) statChoices.Remove(buildingInfo.Stats[i].StatName);
            else statChoices.Remove(oldStat);

            statValues[i] = GUILayout.TextField(statValues[i], GUILayout.Width(100), GUILayout.Height(20));

            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) RemoveStat(i);
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save stats")) SaveStats();

        // Save the changes back to the object
        EditorUtility.SetDirty(target);
        //serializedObject.ApplyModifiedProperties();
    }

    public void AddStat()
    {
        if (statChoices.Count == 0) return;

        buildingInfo.Stats.Add(new Stat(statChoices[0], 0));
        statChoices.RemoveAt(0);
        statsIndices.Add(0);
        statValues.Add("");
    }

    public void RemoveStat(int i)
    {
        statChoices.Add(buildingInfo.Stats[i].StatName);
        buildingInfo.Stats.RemoveAt(i);
        statsIndices.RemoveAt(i);
        statValues.RemoveAt(i);
    }

    public void SaveStats()
    {
        for (int i = 0; i < statValues.Count; i++)
        {
            if (!float.TryParse(statValues[i], out buildingInfo.Stats[i].StatValue)) buildingInfo.Stats[i].StatValue = 0;
            statValues[i] = buildingInfo.Stats[i].StatValue.ToString();
        }
    }
}