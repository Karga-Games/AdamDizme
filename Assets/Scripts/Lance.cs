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

    public List<Ball> InteractedBalls;

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
        SetupText();
        InteractedBalls = new List<Ball>();
    }

    private void Update()
    {

    }

    public Dictionary<CrowdColumn,int> CalculateInteractions()
    {

        Dictionary<CrowdColumn, int> interactions = new Dictionary<CrowdColumn, int>();

        foreach (Ball ball in InteractedBalls)
        {
            if (ball.column != null)
            {

                if (!interactions.ContainsKey(ball.column))
                {
                    interactions.Add(ball.column, 0);
                }

                interactions[ball.column]++;

            }
        }

        return interactions;

    }

    public void AddBalls(BallCrowd crowd)
    {
        Dictionary<CrowdColumn, int> interactions = CalculateInteractions();

        List<CrowdColumn> columnList = new List<CrowdColumn>();

        foreach (KeyValuePair<CrowdColumn, int> interaction in interactions)
        {
            columnList.Add(interaction.Key);
        }

        if (AdditionValue > 0)
        {
            crowd.AddBallsToColumn(columnList, AdditionValue);
        }
        else
        {
            crowd.KillBallsFromColumn(columnList, AdditionValue);
        }

    }

    public void MultiplyBalls(BallCrowd crowd)
    {
        Dictionary<CrowdColumn, int> interactions = CalculateInteractions();

        foreach (KeyValuePair<CrowdColumn, int> interaction in interactions)
        {
            if(MultiplyFactor > 1)
            {
                crowd.AddBallsToColumn(interaction.Key, (int)(interaction.Value * MultiplyFactor),true);
            }
            else
            {
                crowd.KillBallsFromColumn(interaction.Key, (int)(interaction.Value * MultiplyFactor),true);
            }
        }

    }

    public void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.layer)
        {
            case 6:
                //Ball Collider Entered

                Ball ball = other.gameObject.GetComponent<Ball>();

                if (ball != null)
                {
                    InteractedBalls.Add(ball);
                }

                break;
        }
    }


    public void OnCollisionExit(Collision other)
    {

        switch (other.gameObject.layer)
        {
            case 7:
                //Player Collider Exit
                Player player = other.gameObject.GetComponent<Player>();

                if(player != null)
                {
                    if (MultiplyFactor > 0)
                    {
                        MultiplyBalls(player.crowd);
                    }
                    else
                    {
                        AddBalls(player.crowd);
                    }
                }

                break;
        }
        
    }

}


