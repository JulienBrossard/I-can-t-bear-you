using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[CustomEditor(typeof(Item), true)]
public class ItemEditor : Editor
{
    
#if UNITY_EDITOR
    private Item item;
    private bool conductorBuffer;
    private float zoneSizeBuffer;
    private float fallHandleSize = 2.5f;
    private float lastRadius;

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
            Handles.DrawLine(item.transform.position, item.transform.position + fall.Dir.normalized * fallHandleSize);
        }
    }

    public override void OnInspectorGUI()
    {
        item = (Item)target;
        ChangeRadius();
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

        if (item.GetComponent<Outline>() == default)
        {
            if (GUILayout.Button("Create Outline"))
            {
                GameObject outlineObject = Instantiate((GameObject)Resources.Load("Outline"), item.transform);
                outlineObject.GetComponent<MeshFilter>().sharedMesh = item.GetComponent<MeshFilter>().sharedMesh;
            
                Outline outline = item.AddComponent<Outline>();
                outline.outlineData = (OutlineData)Resources.Load("Outline Data");
                outline.outlineObject = outlineObject;
                
                List<Material> materials = new List<Material>();
                for (int i = 0; i < item.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
                {
                    materials.Add(outline.outlineData.outlineMaterial);
                }
                
                outlineObject.GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
            }
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

        if (item.fallable && item.falls.Count > 0)
        {
            if (GUILayout.Button("Fall"))
            {
                item.Fall(Random.insideUnitSphere);
            }
        }

        if (item.explosive)
        {
            if (GUILayout.Button("Explode"))
            {
                item.Explode();
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
    
    /// <summary>
    /// Change NavMeshObstacle Radius
    /// </summary>
    public void ChangeRadius()
    {
        if (target.GetType() == typeof(Disperse))
        {
            var disperse = (Disperse)target;
            if (disperse.awareness == null)
            {
                disperse.awareness = disperse.gameObject.GetComponent<Awareness>();
            }

            if (disperse.obstacle == null)
            {
                disperse.obstacle = disperse.gameObject.GetComponent<NavMeshObstacle>();
                disperse.obstacle.shape = NavMeshObstacleShape.Capsule;
                disperse.obstacle.carving = true;
            } 
            if (lastRadius != ((Disperse)target).awareness.viewRadius)
            {
                disperse.obstacle.radius = disperse.awareness.viewRadius;
            }
        }
    }
    
#endif
}