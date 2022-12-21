using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NpcCreator : EditorWindow
{
#if UNITY_EDITOR

    private GameObject model;
    private Npc npc;
    private EntityData entityData;
    private NpcData npcData;
    StatusEffectsData statusEffectsData;
    PanicData panicData;
    private GameObject canvas;
    private string name = "Npc name";
    RuntimeAnimatorController animatorController;
    
    [MenuItem("Tools/Npc Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(NpcCreator));
    }
    
    void OnGUI()
    {
        GUILayout.Label("Create Npc",EditorStyles.boldLabel);
        name = GUILayout.TextField(name, 25);
        model = EditorGUILayout.ObjectField("Npc model", model ,typeof(GameObject),true) as GameObject;
        //npc = EditorGUILayout.ObjectField("Npc script", model ,typeof(Npc),true) as Npc;
        entityData = EditorGUILayout.ObjectField("Entity Data", entityData ,typeof(EntityData),true) as EntityData;
        npcData = EditorGUILayout.ObjectField("Npc Data", npcData ,typeof(NpcData),true) as NpcData;
        statusEffectsData = EditorGUILayout.ObjectField("Status Effects Data", statusEffectsData ,typeof(StatusEffectsData),true) as StatusEffectsData;
        panicData = EditorGUILayout.ObjectField("Panic Data", panicData ,typeof(PanicData),true) as PanicData;
        canvas = EditorGUILayout.ObjectField("Canvas", canvas ,typeof(GameObject),true) as GameObject;
        animatorController = EditorGUILayout.ObjectField("Animator Controller", animatorController ,typeof(RuntimeAnimatorController),true) as RuntimeAnimatorController;
        if (GUILayout.Button("Create"))
        {
            CreateNpc();
        }
    }

    void CreateNpc()
    {
        GameObject npc = Instantiate(model, Vector3.zero, Quaternion.identity);
        npc.tag = "Npc";
        npc.layer = 6;
        npc.AddComponent<Npc>();
        npc.gameObject.name = name;
        npc.GetComponent<Npc>().entityData = entityData;
        npc.GetComponent<Npc>().npcData = npcData;
        npc.GetComponent<StatusEffects>().statusEffectsData = statusEffectsData;
        npc.GetComponent<Panic>().panicData = panicData;
        GameObject canvasObject = Instantiate(canvas, Vector3.zero, Quaternion.identity, npc.transform);
        npc.GetComponent<NpcUI>().canvas = canvasObject;
        npc.GetComponent<NpcUI>().thirstImage = FindChildObjectWithName(canvasObject, "Water");
        npc.GetComponent<NpcUI>().hungerImage =  FindChildObjectWithName(canvasObject, "Food");
        npc.GetComponent<NpcUI>().bladderImage =  FindChildObjectWithName(canvasObject, "Toilet");
        npc.GetComponent<NpcUI>().suspiciousImage =  FindChildObjectWithName(canvasObject, "Suspicious").GetComponent<Image>();
        npc.GetComponent<NpcUI>().suspiciousImage.transform.parent.GetComponent<PanicImage>().npc = npc;
        npc.GetComponent<NpcUI>().panicImage =  FindChildObjectWithName(canvasObject, "Panic").GetComponent<Image>();
        npc.GetComponent<NpcUI>().panicImage.transform.parent.GetComponent<PanicImage>().npc = npc;
        npc.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        npc.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.97f, 0);
        npc.GetComponent<CapsuleCollider>().radius = 0.5f;
        npc.GetComponent<CapsuleCollider>().height = 2;
        npc.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        npc.GetComponent<NavMeshAgent>().stoppingDistance = 0.25f;
        npc.GetComponent<AwarenessNpc>().viewRadius = 6;
        npc.GetComponent<AwarenessNpc>().viewAngle = 120;
        npc.GetComponent<AwarenessNpc>().targetMask = LayerMask.GetMask("Player") + LayerMask.GetMask("Npc");
        npc.GetComponent<AwarenessNpc>().obstacleMask = LayerMask.GetMask("Default") + LayerMask.GetMask("TransparentFX") +
                                                        LayerMask.GetMask("Water") + LayerMask.GetMask("UI");
        npc.GetComponent<AwarenessNpc>().maxTargets = 5;
        PrefabUtility.SaveAsPrefabAsset(npc, "Assets/Prefabs/" + name + ".prefab");
        DestroyImmediate(npc);
    }

    GameObject FindChildObjectWithName(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
            else
            {
                GameObject result = FindChildObjectWithName(child.gameObject, name);
                if(result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

#endif
}
