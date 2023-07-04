using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLegModelChanger : MonoBehaviour
{
    public List<GameObject> LegModels;

    private void Awake()
    {
        GetAllLegModels();
    }
    private void GetAllLegModels()
    {
        int childrenGameObjects = transform.childCount;

        for (int i = 0; i < childrenGameObjects; i++)
        {
            LegModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllLegModels()
    {
        foreach (GameObject helmetModel in LegModels)
        {
            helmetModel.SetActive(false);
        }
    }

    public void EquipLegModelByName(string helmetName)
    {
        for (int i = 0; i < LegModels.Count; i++)
        {
            if (LegModels[i].name == helmetName)
            {
                LegModels[i].SetActive(true);
            }
        }
    }
}
