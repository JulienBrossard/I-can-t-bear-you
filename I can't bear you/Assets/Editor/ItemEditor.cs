using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item), true)]
public class ItemEditor : Editor
{
    private Item item;
    private bool chargedBuffer;

    private void OnEnable()
    {
        item = (Item)target;
        chargedBuffer = item.charged;
    }

    public override void OnInspectorGUI()
    {
        item = (Item)target;
        base.OnInspectorGUI();
        if (chargedBuffer != item.charged)
        {
            chargedBuffer = item.charged;
            if(chargedBuffer) CreateZone();
            else RemoveZone();
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
        item.zone.SetActive(false);
        return;
    }
    void RemoveZone()
    {
        Debug.Log(0);
        for (int i = 0; i < item.gameObject.transform.childCount; i++)
        {
            if (item.gameObject.transform.GetChild(i).GetComponent<ElectricityZone>() != default)
            {
                DestroyImmediate(item.gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}