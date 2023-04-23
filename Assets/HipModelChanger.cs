using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipModelChanger : MonoBehaviour
{
    public List<GameObject> hipModels;

    private void Awake()
    {
        GetAllHipModels();
    }
    private void GetAllHipModels()
    {
        int childrenGameObjects = transform.childCount;

        for (int i = 0; i < childrenGameObjects; i++)
        {
            hipModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllHipModels()
    {
        foreach (GameObject helmetModel in hipModels)
        {
            helmetModel.SetActive(false);
        }
    }

    public void EquipHipModelByName(string helmetName)
    {
        for (int i = 0; i < hipModels.Count; i++)
        {
            if (hipModels[i].name == helmetName)
            {
                hipModels[i].SetActive(true);
            }
        }
    }
}
