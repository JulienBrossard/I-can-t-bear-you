using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Puddle), true)]
public class PuddleEditor : Editor
{
#if UNITY_EDITOR
    private Puddle puddle;
    private bool conductorBuffer;
    private float zoneSizeBuffer;

    private void OnEnable()
    {
        puddle = (Puddle)target;
        conductorBuffer = puddle.conductor;
        zoneSizeBuffer = puddle.zoneSize;
    }

    public override void OnInspectorGUI()
    {
        puddle = (Puddle)target;
        base.OnInspectorGUI();
        if (conductorBuffer != puddle.conductor)
        {
            conductorBuffer = puddle.conductor;
            if(conductorBuffer) CreateZone();
            else RemoveZone();
        }
        if(zoneSizeBuffer != puddle.zoneSize)
        {
            if(!puddle.conductor) return;
            zoneSizeBuffer = puddle.zoneSize;
            puddle.zone.GetComponent<ElectricityZone>().SetSize(ZoneMode.PLANE, zoneSizeBuffer);
        }

        if(!Application.isPlaying) return;
        if (puddle.conductor)
        {
            if (!puddle.charged)
            {
                if (GUILayout.Button("Electrocute"))
                {
                    puddle.Electrocute();
                }
            }
            else
            {
                if (GUILayout.Button("De Electrocute"))
                {
                    puddle.DeElectrocute();
                }
            }
        }
    }

    void CreateZone()
    {
        for (int i = 0; i < puddle.gameObject.transform.childCount; i++)
        {
            if (puddle.gameObject.transform.GetChild(i).GetComponent<ElectricityZone>() != default)
            {
                Debug.Log("Zone already exists");
                return;
            }
        }
        puddle.CreateZone();
        puddle.zone.GetComponent<ElectricityZone>().SetSize(ZoneMode.PLANE,zoneSizeBuffer);
        puddle.zone.SetActive(false);
    }
    void RemoveZone()
    {
        for (int i = 0; i < puddle.gameObject.transform.childCount; i++)
        {
            if (puddle.gameObject.transform.GetChild(i).GetComponent<ElectricityZone>() != default)
            {
                DestroyImmediate(puddle.gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
#endif
}