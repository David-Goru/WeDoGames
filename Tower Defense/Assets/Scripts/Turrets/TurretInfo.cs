using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "ScriptableObjects/TurretInfo", order = 0)]
public class TurretInfo : BuildingInfo
{
    [HideInInspector] public List<Stat> Stats;
    [SerializeField] float attackRate = 0;
    [SerializeField] float attackDamage = 0;
    [SerializeField] float attackRange = 0;

    public float GetAttackRate() { return attackRate; }
    public float GetAttackDamage() { return attackDamage; }
    public float GetAttackRange() { return attackRange; }

}

[CustomEditor(typeof(TurretInfo))]
public class TurretInfoEditor : Editor
{
    TurretInfo turretInfo;
    List<int> statsIndices;
    List<string> statChoices;
    List<string> temp = new List<string>() { "hola buenas tardes", "adios, que tenga un buen dia"};

    public void OnEnable()
    {
        turretInfo = (TurretInfo)target;
        if(turretInfo.Stats == null)
        {
            turretInfo.Stats = new List<Stat>();
        }
        statChoices = new List<string>(temp);
        statsIndices = new List<int>();
        foreach (var stat in turretInfo.Stats)
        {
            statChoices.Remove(stat.StatName);
            statsIndices.Add(temp.FindIndex(x => x == stat.StatName));
        }
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();
        if(GUILayout.Button("Add stat"))
        {
            AddStat();
        }
        
        for (int i = 0; i < turretInfo.Stats.Count; i++)
        {
            if (statsIndices[i] == -1)
                statsIndices[i] = 0;
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            string oldStat = turretInfo.Stats[i].StatName;
            statChoices.Add(oldStat);
            statsIndices[i] = EditorGUILayout.Popup(statsIndices[i], statChoices.ToArray(), GUILayout.Width(150), GUILayout.Height(30));
            turretInfo.Stats[i].StatName = temp[statsIndices[i]];
            if(oldStat != turretInfo.Stats[i].StatName)
            {
                statChoices.Remove(turretInfo.Stats[i].StatName);
            }
            else
            {
                statChoices.Remove(oldStat);
            }
            turretInfo.Stats[i].StatValue = float.Parse(GUILayout.TextField(turretInfo.Stats[i].StatValue.ToString(), GUILayout.Width(100), GUILayout.Height(20)));
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
            {
                RemoveStat(i);
            }
            GUILayout.EndHorizontal();
        }
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
        //serializedObject.ApplyModifiedProperties();
    }

    public void AddStat()
    {
        if (statChoices.Count == 0)
            return;
        turretInfo.Stats.Add(new Stat(statChoices[0], 0));
        statChoices.RemoveAt(0);
        statsIndices.Add(0);
    }
    public void RemoveStat(int i)
    {
        statChoices.Add(turretInfo.Stats[i].StatName);
        turretInfo.Stats.RemoveAt(i);
        statsIndices.RemoveAt(i);
    }
}
