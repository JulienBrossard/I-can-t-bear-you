using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tools : MonoBehaviour
{
    #region Declarations

    public static Tools instance;
    
    public enum FIELD
    {
        SERIALIZED,
        HIDDEN
    }
    
    #endregion

    private void Awake()
    {
        instance = this;
    }

    #region Array

    public void AddVariableInArray <T>(T[] array, T variable)
    {
        array.ToList().Add(variable);
        array.ToArray();
    }
    
    public void AddVariablesInArray <T>(T[] array, T variable, int number)
    {
        for (int i = 0; i < number; i++)
        {
            array.ToList().Add(variable);
        }
        array.ToArray();
    }

    public int[] FillArray (int minValue, int maxValue, int[] array)
    {
        array = new int[maxValue+1];
        for (int i = minValue; i < maxValue+1; i++)
        {
            array[i] = i;
        }

        return array;
    }

    #region Sort

    private AudioSource maxAudio;
    private int maxIndex;
    private int i;
    private int j;
    
    public void Sort(AudioSource[] array)
    {
        maxAudio = array[0];
        maxIndex = 0;
        for (i = 0; i < array.Length-i; i++)
        {
            for (j = 0; j < array.Length-i; j++)
            {
                if (maxAudio.time<array[j].time)
                {
                    maxAudio = array[j];
                    maxIndex = j;
                }
            }
            array[maxIndex] = array[j-1];
            array[j-1] = maxAudio;
            maxIndex = 0;
            maxAudio = array[0];
        }
    }
    
    private GameObject maxGameObject;

    public void Sort(GameObject[] array)
    {
        maxGameObject = array[0];
        maxIndex = 0;
        for (i = 0; i < array.Length-i; i++)
        {
            for (j = 0; j < array.Length-i; j++)
            {
                if (maxGameObject.transform.position.z<array[j].transform.position.z)
                {
                    maxGameObject = array[j];
                    maxIndex = j;
                }
            }
            array[maxIndex] = array[j-1];
            array[j-1] = maxGameObject;
            maxIndex = 0;
            maxGameObject = array[0];
        }
    }
    
    
    /*public void Quick_Sort(AudioSource[] arr, int left, int right) 
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);

            if (pivot > 1) {
                Quick_Sort(arr, left, pivot - 1);
            }
            if (pivot + 1 < right) {
                Quick_Sort(arr, pivot + 1, right);
            }
        }
        
    }

    
    private float pivot;
    
    private AudioSource temp;

    private int Partition(AudioSource[] arr, int left, int right)
    {
        pivot = arr[left].time;
        while (true)
        {

            while (arr[left].time < pivot)
            {
                left++;
            }

            while (arr[right].time > pivot)
            {
                right--;
            }

            if (left < right)
            {
                if (arr[left] == arr[right]) return right;

                temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;


            }
            else
            {
                return right;
            }
        }
    }*/

    #endregion
    
    #endregion


    #region UI
    
    public bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        var pe = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);
        return hits.Count > 0;
    }

    public List<RaycastResult> ObjectPointerOverUI()
    {
        var pe = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);
        return hits;
    }
    
    #endregion

    #region Math

    #region Circle

    
    /// <summary>
    /// Check if a point is in a circle
    /// </summary>
    /// <param name="point"> Point </param>
    /// <param name="radius"> Radius of the circle </param>
    /// <param name="center"> Center of the circle </param>
    /// <returns></returns>
    public bool CheckPointInCircle(Vector3 point, float radius, Vector3 center)
    {
        if (Vector3.Distance(point, center) < radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    /// <summary>
    /// Calculate Random point in a circle
    /// </summary>
    /// <param name="radius"> Radius of the circle </param>
    /// <returns></returns>
    public Vector2 CalculateRandomPointInCircle(float radius)
    {
        return new Vector2(
            Random.Range(-radius,
                radius),
            Random.Range(-radius,
                radius));
    }

    #endregion


    #region Rectangle

    
    /// <summary>
    /// Calculate Random point in a rectangle
    /// </summary>
    /// <param name="length"> Length of the rectangle </param>
    /// <param name="width"> Width of the rectangle </param>
    /// <returns></returns>
    public Vector2 CalculateRandomPointInRectangle(float length, float width)
    {
        return new Vector2(
            Random.Range(-length/2,
                length/2),
            Random.Range(-width/2,
                width/2));
    }

    #endregion
    
    #endregion
}
