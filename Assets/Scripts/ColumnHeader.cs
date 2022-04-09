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
        line.autoDraw = false;
        line.transform.SetParent(null);
    }
}
