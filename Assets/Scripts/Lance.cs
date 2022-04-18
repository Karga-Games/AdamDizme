using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lance : MonoBehaviour
{
    public int AdditionValue;
    public float MultiplyFactor;

    public int LanceHeightForMultiply;

    public TextMeshPro lanceText;

    public Material goodMaterial;
    public Material badMaterial;

    public MeshRenderer planeRenderer;


    public List<ColumnHeader> columnsToAdd;

    public int triggeringCount;
    public bool triggered = false;

    private void OnValidate()
    {
        if (isActiveAndEnabled)
        {
            SetupText();
        }
    }

    public void SetupText()
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

        if (MultiplyFactor > 0)
        {
            if (MultiplyFactor > 1)
            {
                prefix = "x";
                planeRenderer.material = goodMaterial;
            }
            else
            {
                prefix = "x";
                planeRenderer.material = badMaterial;
            }
            lanceText.text = prefix + MultiplyFactor.ToString();
        }


    }

    private void Start()
    {
        columnsToAdd = new List<ColumnHeader>();
        triggered = false;

        SetupText();
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


            foreach(ColumnHeader column in columnsToAdd)
            {
                columnsToAdd[index].crowd.MultiplyColumn(column.columnIndex, MultiplyFactor, LanceHeightForMultiply);
            }


        }
    }




    public void ColumnEntered(ColumnHeader column)
    {
        triggered = true;
        triggeringCount++;
        if (Mathf.Abs(AdditionValue) > 0 || Mathf.Abs(MultiplyFactor) > 0)
        {
            columnsToAdd.Add(column);
        }
    }
    public void ColumnExited(ColumnHeader column)
    {
        triggeringCount--;
    }

    public void StickmanEntered(Stickman stickman)
    {
        stickman.Glow();
    }

    public void OnTriggerEnter(Collider other)
    {
        ColumnHeader column = other.GetComponent<ColumnHeader>();
        if(column != null)
        {
            ColumnEntered(column);

        }

        Stickman stickman = other.GetComponent<Stickman>();
        if(stickman != null && (AdditionValue > 0 || MultiplyFactor > 1))
        {
            StickmanEntered(stickman);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        ColumnHeader column = other.GetComponent<ColumnHeader>();
        if (column != null)
        {
            ColumnExited(column);
            /*
            if (Mathf.Abs(AdditionValue) > 0 || Mathf.Abs(MultiplyFactor) > 0)
            {
                //columnsToAdd.Add(column);
            }
            */
            

        }

    }



}
