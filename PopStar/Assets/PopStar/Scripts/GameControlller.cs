
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Assets.PopStar.Scripts;

public class GameControlller : MonoBehaviour
{
    public static GameControlller Ins;
    public GameObject star;
    public Transform StarParent;
    public ArrayList starList;
    public ArrayList SameColorStarList;
    public ArrayList checkedStars;
    private int rowNum = 9; //行号
    private int colNum = 10; //列号
    private Star currentStar;

    public void Awake()
    {
        starList = new ArrayList();
        checkedStars = new ArrayList();
        SameColorStarList = new ArrayList();
        Ins = this;
    }

    public void Start()
    {
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList temp = new ArrayList();
            for (int columIndex = 0; columIndex < colNum; columIndex++)
            {
                GameObject go = AddStar(rowIndex, columIndex);
                temp.Add(go.GetComponent<Star>());
            }
            starList.Add(temp);
        }
    }

    public GameObject AddStar(int row, int col)
    {
        GameObject go = Instantiate(star) as GameObject;
        go.transform.parent = StarParent;
        go.transform.localScale = Vector3.one;
        go.GetComponent<Star>().UpdatePosition(row, col);
        go.GetComponent<Star>().RandomCreateStarType();
        return go;
    }

    //被点击
    public void Select(Star star)
    {
        currentStar = star;
        SameColorStarList.Add(star);
        CheckAllSide(currentStar);
        if (star.IsSelect)
        {
            if (SameColorStarList.Count > 1)
            {
                for (int i = 0; i < rowNum; i++)
                {
                    for (int j = 0; j < colNum; j++)
                    {
                        Star s = ((ArrayList)starList[i])[j] as Star;
                        if (SameColorStarList.Contains(s))
                        {
                            ((ArrayList)starList[i])[j] = null;
                            Destroy(s.gameObject);
                        }
                    }
                }
                PositionVerticalChange();
                Star temp = null;
                for (int i = 0; i < rowNum; i++)
                {
                    temp = ((ArrayList)starList[i])[0] as Star;
                    if (temp == null)
                    {
                        PositionHorizontalChange();
                        break;
                    }
                }
            }
        }
        else
        {
            if (SameColorStarList.Count > 1)
            {
                for (int i = 0; i < rowNum; i++)
                {
                    for (int j = 0; j < colNum; j++)
                    {
                        Star s = ((ArrayList)starList[i])[j] as Star;
                        if (s != null)
                        {
                            s.IsSelect = false;
                            if (SameColorStarList.Contains(s))
                            {
                                s.IsSelect = true;
                            }
                        }
                    }
                }
            }
        }
        SameColorStarList.Clear();
        checkedStars.Clear();
    }

    //检测上方
    public void CheckUp(Star star)
    {
        int column = star.ColIndex;
        int row = star.RowIndex;
        if (column > 8)
        {
            return;
        }
        Star starUpSide = (Star)((ArrayList)starList[row])[column + 1];
        if (starUpSide == null || starUpSide.starColor != star.starColor)
        {
            return;
        }
        SameColorStarList.Add(starUpSide);
        CheckAllSide(starUpSide);
    }

    //检测下方
    public void CheckDown(Star star)
    {
        int column = star.ColIndex;
        int row = star.RowIndex;
        if (column < 1)
        {
            return;
        }
        Star starDownSide = ((ArrayList)starList[row])[column - 1] as Star;
        if (starDownSide == null || starDownSide.starColor != star.starColor)
        {
            return;
        }
        SameColorStarList.Add(starDownSide);
        CheckAllSide(starDownSide);
    }

    //检测右方
    public void CheckRight(Star star)
    {
        int column = star.ColIndex;
        int row = star.RowIndex;
        if (row > 7)
        {
            return;
        }
        Star starRightSide = ((ArrayList)starList[row + 1])[column] as Star;
        if (starRightSide == null || starRightSide.starColor != star.starColor)
        {
            return;
        }
        SameColorStarList.Add(starRightSide);
        CheckAllSide(starRightSide);
    }

    //检测左方
    public void CheckLeft(Star star)
    {
        int column = star.ColIndex;
        int row = star.RowIndex;
        if (row < 1)
        {
            return;
        }
        Star starLeftSide = ((ArrayList)starList[row - 1])[column] as Star;
        if (starLeftSide == null || starLeftSide.starColor != star.starColor)
        {
            return;
        }
        SameColorStarList.Add(starLeftSide);
        CheckAllSide(starLeftSide);
    }

    //递归，不停检测四面
    public void CheckAllSide(Star star)
    {
        if (IsChecked(star))
        {
            return;
        }
        checkedStars.Add(star);
        CheckUp(star);
        CheckDown(star);
        CheckLeft(star);
        CheckRight(star);
    }

    //是否已经检测
    public bool IsChecked(Star star)
    {
        return checkedStars.Contains(star);
    }

    //改变竖直位置
    public void PositionVerticalChange()
    {
        ArrayList row = new ArrayList();
        for (int i = 0; i < 9; i++)
        {
            ArrayList col = new ArrayList();
            for (int j = 0; j < 10; j++)
            {
                col.Add(null);
            }
            row.Add(col);
        }

        for (int i = 0; i < rowNum; i++)
        {
            int index = 0;
            for (int j = 0; j < colNum; j++)
            {
                Star s = (((ArrayList)starList[i])[j]) as Star;
                if (s != null)
                {
                    ((ArrayList)row[i])[index] = s;
                    index++;
                }
            }
        }
        starList = row;
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                Star s = ((ArrayList)starList[i])[j] as Star;
                if (s != null)
                {
                    s.UpdatePosition(i, j);
                }
            }
        }
    }

    public void PositionHorizontalChange()
    {
        ArrayList row = new ArrayList();
        for (int i = 0; i < 9; i++)
        {
            ArrayList col = new ArrayList();
            for (int j = 0; j < 10; j++)
            {
                col.Add(null);
            }
            row.Add(col);
        }

        for (int i = 0; i < colNum; i++)
        {
            int index = 0;
            for (int j = 0; j < rowNum; j++)
            {
                Star s = (((ArrayList)starList[j])[i]) as Star;
                if (s != null)
                {
                    ((ArrayList)row[index])[i] = s;
                    index++;
                }
            }
        }
        starList = row;
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                Star s = ((ArrayList)starList[i])[j] as Star;
                if (s != null)
                {
                    s.UpdatePosition(i, j);
                }
            }
        }
    }
}
