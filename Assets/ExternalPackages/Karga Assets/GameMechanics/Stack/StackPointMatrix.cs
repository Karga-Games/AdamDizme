using System.Linq;
using UnityEngine;
public enum MatrixSearchOrder
{
    XY,
    XZ,
    YX,
    YZ,
    ZX,
    ZY
}
public enum Axis
{
    X,
    Y,
    Z
}
public class StackPointMatrix<TStackPoint> where TStackPoint : StackPoint
{
    public TStackPoint[][][] Matrix;
    public int MaxRow;
    public int MaxColumn;
    public int MaxHeight;
    public MatrixSearchOrder SearchOrder;

    public StackPointMatrix(int maxrow = 1, int maxcolumn = 1, int maxheight = 1)
    {
        this.MaxRow = maxrow;
        this.MaxColumn = maxcolumn;
        this.MaxHeight = maxheight;
        InitializeMatrix();
    }

    public void InitializeMatrix()
    {
        Matrix = new TStackPoint[MaxRow][][];
        
        for(int i = 0; i < MaxRow; i++)
        {
            Matrix[i] = new TStackPoint[MaxColumn][];

            for(int j = 0; j < MaxColumn; j++)
            {
                Matrix[i][j] = new TStackPoint[MaxHeight];
            }
        }

    }

    public TStackPoint GetPointAt(Vector3Int Coordinate)
    {
        return this.Matrix[Coordinate.x][Coordinate.y][Coordinate.z];
    }


    public Vector3Int FirstEmptyCoordinate(MatrixSearchOrder order)
    {
        switch (order) 
        {
            case MatrixSearchOrder.XY:
                return FirstEmptyCoordinateXY();
            case MatrixSearchOrder.XZ:
                return FirstEmptyCoordinateXZ();
            case MatrixSearchOrder.YX:
                return FirstEmptyCoordinateYX();
            case MatrixSearchOrder.YZ:
                return FirstEmptyCoordinateYZ();
            case MatrixSearchOrder.ZX:
                return FirstEmptyCoordinateZX();
            case MatrixSearchOrder.ZY:
                return FirstEmptyCoordinateZY();
        }
        return Vector3Int.zero;
    }

    #region EmptyCoordinateSearch Functions
    public Vector3Int FirstEmptyCoordinateXY()
    {

        for (int i = 0; i < MaxHeight; i++)
        {
            for (int j = 0; j < MaxColumn; j++)
            {
                for (int k = 0; k < MaxRow; k++)
                {
                    if (Matrix[k][j][i] == null)
                    {

                        return new Vector3Int(k, j, i);

                    }
                }
            }
        }
        return new Vector3Int(0, 0, 0);
    }

    public Vector3Int FirstEmptyCoordinateYX()
    {

        for (int i = 0; i < MaxHeight; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                for (int k = 0; k < MaxColumn; k++)
                {
                    if (Matrix[j][k][i] == null)
                    {

                        return new Vector3Int(j, k, i);

                    }
                }
            }
        }
        return new Vector3Int(0, 0, 0);
    }
   public Vector3Int FirstEmptyCoordinateXZ()
   {

       for (int i = 0; i < MaxColumn; i++)
       {
           for (int j = 0; j < MaxHeight; j++)
           {
               for (int k = 0; k < MaxRow; k++)
               {
                   if (Matrix[k][i][j] == null)
                   {

                       return new Vector3Int(k, i, j);

                   }
               }
           }
       }
       return new Vector3Int(0, 0, 0);
   }
    public Vector3Int FirstEmptyCoordinateZX()
    {

        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                for (int k = 0; k < MaxHeight; k++)
                {
                    if (Matrix[j][i][k] == null)
                    {

                        return new Vector3Int(j, i, k);

                    }
                }
            }
        }
        return new Vector3Int(0, 0, 0);
    }

    public Vector3Int FirstEmptyCoordinateYZ()
    {

        for (int i = 0; i < MaxRow; i++)
        {
            for (int j = 0; j < MaxHeight; j++)
            {
                for (int k = 0; k < MaxColumn; k++)
                {
                    if (Matrix[i][k][j] == null)
                    {

                        return new Vector3Int(i, k, j);

                    }
                }
            }
        }
        return new Vector3Int(0, 0, 0);
    }
    public Vector3Int FirstEmptyCoordinateZY()
    {
        
        for (int i = 0; i < MaxRow; i++)
        {
            for (int j = 0; j < MaxColumn; j++)
            {
                for (int k = 0; k < MaxHeight; k++)
                {
                    if(Matrix[i][j][k] == null)
                    {
                        
                        return new Vector3Int(i,j,k);
                        
                    }
                }
            }
        }
        return new Vector3Int(0,0,0);
    }
    #endregion

    public int PointCount()
    {
        int count = 0;
        for(int i = 0; i < MaxRow; i++)
        {
            for(int j = 0; j < MaxColumn; j++)
            {
                for(int k = 0; k < MaxHeight; k++)
                {
                    if(Matrix[i][j][k] != null)
                    {
                       count++;
                    }
                }
            }
        }
        return count;
    }

    public void ReArrange(Axis[] axises)
    {
        foreach(Axis axis in axises)
        {
            switch (axis)
            {
                case Axis.X:

                    ArrangeRows(Matrix);

                    break;
                case Axis.Y:

                    for (int i = 0; i < MaxRow; i++)
                    {
                        ArrangeColumns(Matrix[i]);
                    }

                    break;
                case Axis.Z:

                    for (int i = 0; i < MaxRow; i++)
                    {
                        for (int j = 0; j < MaxColumn; j++)
                        {
                            ArrangePoints(Matrix[i][j]);
                        }

                    }

                    break;
            }
        }

        RefreshCoordinates();
    }

    public void RefreshCoordinates()
    {
        for (int i = 0; i < MaxRow; i++)
        {
            for (int j = 0; j < MaxColumn; j++)
            {
                for (int k = 0; k < MaxHeight; k++)
                {
                    if(Matrix[i][j][k] != null)
                    {
                        Matrix[i][j][k].RefreshCoordinate(new Vector3Int(i, j, k));
                        //Matrix[i][j][k].Coordinate = new Vector3Int(i, j, k);
                    }
                    
                }
            }
        }
    }

    public static bool isRowEmpty(TStackPoint[][] row)
    {
        foreach(TStackPoint[] column in row)
        {
            if (!isColumnEmpty(column))
            {
                return false;
            }
        }
        return true;
    }

    public static bool isColumnEmpty(TStackPoint[] column)
    {
        foreach(TStackPoint point in column)
        {
            if (!isPointEmpty(point))
            {
                return false;
            }
        }
        return true;
    }

    public static bool isPointEmpty(TStackPoint point)
    {
        if(point != null)
        {
            return false;
        }
        return true;
    }

    public TStackPoint Add(TStackPoint Point)
    {
        Vector3Int emptyCoord = FirstEmptyCoordinate(SearchOrder);
        Matrix[emptyCoord.x][emptyCoord.y][emptyCoord.z] = Point;
        Point.Coordinate = emptyCoord;
        return Matrix[emptyCoord.x][emptyCoord.y][emptyCoord.z];
    }

    public TStackPoint AddTo(Vector3Int Coordinate, TStackPoint Point)
    {
        Matrix[Coordinate.x][Coordinate.y][Coordinate.z] = Point;
        Point.Coordinate = Coordinate;
        return Matrix[Coordinate.x][Coordinate.y][Coordinate.z];
    }

    static void ArrangePoints(TStackPoint[] arr)
    {
        int ileft = 0;
        int iright = 1;

        while (iright < arr.Length)
        {
            TStackPoint left = arr[ileft];
            TStackPoint right = arr[iright];

            if (left == null && right != null)
            {
                PointSwap(arr, ileft++, iright++);
            }
            else if (left == null && right == null)
            {
                iright++;
            }
            else if (left != null && right != null)
            {
                ileft += 2;
                iright += 2;
            }
            else if (left != null && right == null)
            {
                ileft++;
                iright++;
            }
        }
    }

    static void ArrangeColumns(TStackPoint[][] arr)
    {
        int ileft = 0;
        int iright = 1;

        while (iright < arr.Length)
        {
            TStackPoint[] left = arr[ileft];
            TStackPoint[] right = arr[iright];
            
            if (isColumnEmpty(left) && !isColumnEmpty(right))
            {
                ColumnSwap(arr, ileft++, iright++);
            }
            else if (isColumnEmpty(left) && isColumnEmpty(right))
            {
                iright++;
            }
            else if (!isColumnEmpty(left) && !isColumnEmpty(right))
            {
                ileft += 2;
                iright += 2;
            }
            else if (!isColumnEmpty(left) && isColumnEmpty(right))
            {
                ileft++;
                iright++;
            }
        }
    }

    static void ArrangeRows(TStackPoint[][][] arr)
    {
        int ileft = 0;
        int iright = 1;

        while (iright < arr.Length)
        {
            TStackPoint[][] left = arr[ileft];
            TStackPoint[][] right = arr[iright];

            if (isRowEmpty(left) && !isRowEmpty(right))
            {
                RowSwap(arr, ileft++, iright++);
            }
            else if (isRowEmpty(left) && isRowEmpty(right))
            {
                iright++;
            }
            else if (!isRowEmpty(left) && !isRowEmpty(right))
            {
                ileft += 2;
                iright += 2;
            }
            else if (!isRowEmpty(left) && isRowEmpty(right))
            {
                ileft++;
                iright++;
            }
        }
    }

    static void PointSwap(TStackPoint[] arr, int left, int right)
    {
        TStackPoint temp = arr[left];
        arr[left] = arr[right];
        arr[right] = temp;
    }

    static void ColumnSwap(TStackPoint[][] arr, int left, int right)
    {
        TStackPoint[] temp = arr[left];
        arr[left] = arr[right];
        arr[right] = temp;
    }

    static void RowSwap(TStackPoint[][][] arr, int left, int right)
    {
        TStackPoint[][] temp = arr[left];
        arr[left] = arr[right];
        arr[right] = temp;
    }

    public static void Multiply(ref StackPointMatrix<TStackPoint> old, Vector3Int factor)
    {

        StackPointMatrix<TStackPoint> newMatrix = new StackPointMatrix<TStackPoint>(old.MaxRow * factor.x, old.MaxColumn * factor.y, old.MaxHeight * factor.z);

        for (int i = 0; i < old.MaxRow; i++)
        {
            for (int j = 0; j < old.MaxColumn; j++)
            {
                for (int k = 0; k < old.MaxHeight; k++)
                {
                    newMatrix.Matrix[i][j][k] = old.Matrix[i][j][k];
                }
            }
        }

        old = newMatrix;

    }

   
}
