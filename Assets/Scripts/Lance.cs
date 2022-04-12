using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lance : MonoBehaviour
{
    public int AdditionValue;
    public float MultiplyFactor;

    public TextMeshPro lanceText;

    public Material goodMaterial;
    public Material badMaterial;

    public MeshRenderer planeRenderer;


    public List<ColumnHeader> columnsToAdd;

    int triggeringCount;
    bool triggered = false;

    private void OnValidate()
    {
        if (isActiveAndEnabled)
        {
            string prefix = "";

            if (AdditionValue > 0)
            {
                prefix = "+";
                planeRenderer.material = goodMaterial;
            }
            else
            {
                planeRenderer.material = badMaterial;
            }

            lanceText.text = prefix + AdditionValue.ToString();

        }
    }

    private void Start()
    {
        columnsToAdd = new List<ColumnHeader>();
        triggered = false;
    }

    private void Update()
    {
        if(triggered && triggeringCount == 0)
        {

            triggered = false;
            int added = 0;

            int index = 0;
            while(added < Mathf.Abs(AdditionValue))
            {
                int count = 1;
                if (AdditionValue < 0)
                {
                    count = -1;
                }

                columnsToAdd[index].crowd.AddToColumn(columnsToAdd[index].columnIndex, count);

                added++;

                index++;

                if(index > columnsToAdd.Count - 1)
                {
                    index = 0;
                }
            }

            /*
            foreach (ColumnHeader column in columnsToAdd)
            {
                if(added < Mathf.Abs(AdditionValue))
                {
                    int count = 1;
                    if (AdditionValue < 0)
                    {
                        count = -1;
                    }

                    column.crowd.AddToColumn(column.columnIndex,count);

                    added++;
                }
                else
                {
                    return;
                }
            }
            */

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        ColumnHeader column = other.GetComponent<ColumnHeader>();
        if(column != null)
        {
            triggered = true;
            triggeringCount++;
            if (Mathf.Abs(AdditionValue) > 0)
            {
                //column.crowd.AddToColumn(column.columnIndex, AdditionValue);
                columnsToAdd.Add(column);
            }

            /*
            if (MultiplyFactor > 0)
            {
                column.crowd.MultiplyColumn(column.columnIndex, MultiplyFactor);
            }
            */

        }
    }

    public void OnTriggerExit(Collider other)
    {
        ColumnHeader column = other.GetComponent<ColumnHeader>();
        if (column != null)
        {
            triggeringCount--;
            if (Mathf.Abs(AdditionValue) > 0)
            {
                //column.crowd.AddToColumn(column.columnIndex, AdditionValue);
                columnsToAdd.Add(column);
            }

            /*
            if (MultiplyFactor > 0)
            {
                column.crowd.MultiplyColumn(column.columnIndex, MultiplyFactor);
            }
            */

        }

    }
}
