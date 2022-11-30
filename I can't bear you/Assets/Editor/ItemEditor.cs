using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

[CustomEditor(typeof(Item), true)]
public class ItemEditor : Editor
{
    private Item item;
    private bool conductorBuffer;
    private float zoneSizeBuffer;
    private float fallHandleSize = 2.5f;

    private void OnEnable()
    {
        item = (Item)target;
        conductorBuffer = item.conductor;
        zoneSizeBuffer = item.zoneSize;
    }

    private void OnSceneGUI()
    {
        OnSceneDraw();
    }
    
    public void OnSceneDraw()
    {
        Handles.color = Color.red;
        foreach (FallAsset fall in item.falls)
        {
            Handles.DrawLine(item.transform.position, item.transform.position + fall.Dir * fallHandleSize);
        }
    }

    public override void OnInspectorGUI()
    {
        item = (Item)target;
        base.OnInspectorGUI();
        
        if(item.fallable && item.GetComponent<Rigidbody>() == default)
        {
            Debug.LogWarning(item.name + " is fallable but has no rigidbody, it has been added and set to kinematic.");
            item.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        }
        
        if (conductorBuffer != item.conductor)
        {
            conductorBuffer = item.conductor;
            if(conductorBuffer) CreateZone();
            else RemoveZone();
        }
        if(zoneSizeBuffer != item.zoneSize)
        {
            if(!item.conductor) return;
            zoneSizeBuffer = item.zoneSize;
            item.zone.GetComponent<ElectricityZone>().SetSize(ZoneMode.SPHERE,zoneSizeBuffer);
        }

        if(!Application.isPlaying) return;
        if (item.conductor)
        {
            if (!item.charged)
            {
                if (GUILayout.Button("Electrocute"))
                {
                    item.Electrocute();
                }
            }
            else
            {
                if (GUILayout.Button("De Electrocute"))
                {
                    item.DeElectrocute();
                }
            }
        }
    }

    void CreateZone()
    {
        for (int i = 0; i < item.gameObject.transform.childCount; i++)
        {
            if (item.gameObject.transform.GetChild(i).GetComponent<ElectricityZone>() != default)
            {
                Debug.Log("Zone already exists");
                return;
            }
        }
        item.CreateZone();
        item.zone.GetComponent<ElectricityZone>().SetSize(ZoneMode.SPHERE,zoneSizeBuffer);
        item.zone.SetActive(false);
    }
    void RemoveZone()
    {
        for (int i = 0; i < item.gameObject.transform.childCount; i++)
        {
            if (item.gameObject.transform.GetChild(i).GetComponent<ElectricityZone>() != default)
            {
                DestroyImmediate(item.gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}