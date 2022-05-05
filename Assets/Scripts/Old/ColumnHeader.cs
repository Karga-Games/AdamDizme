using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KargaGames.Drawing;
public class ColumnHeader : MonoBehaviour
{
    public CrowdController crowd;
    public int columnIndex;
    public KargaGames.Drawing.Line line;

    private void OnDestroy()
    {
        if(line != null)
        {
            line.autoDraw = false;
            line.transform.SetParent(null);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Lance lance = collision.gameObject.GetComponent<Lance>();
        if (lance != null)
        {
            //lance.ColumnEntered(this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Lance lance = collision.gameObject.GetComponent<Lance>();
        if (lance != null)
        {
            //lance.ColumnExited(this);
        }
    }
}
