using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum StarColor
{
    None,
    Blue,
    Green,
    Purple,
    Red,
    Yellow,
}

public class Star : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] starType;
    public int RowIndex; //行号
    public int ColIndex; //列号
    public StarColor starColor;  //存储颜色
    private Image StarBG;
    private GameObject StarBackground;
    public bool isSelect;
    public bool IsSelect
    {
        set
        {
            isSelect = value;
            StarBackground.GetComponent<Star>().isSelect = isSelect;
            if (isSelect)
            {
                StarBG.color = Color.red;
            }
            else
            {
                StarBG.color = Color.white;
            }
        }
        get { return isSelect; }
    }

    public void Start()
    {
        StarBG = GetComponentInChildren<Image>();
    }

    public void UpdatePosition(int row, int col)
    {
        RowIndex = row;
        ColIndex = col;
        transform.localPosition = new Vector3(RowIndex * 60, ColIndex * 60, 0);
    }

    public void RandomCreateStarType()
    {
        if (StarBG != null)
            return;
        int starTypeIndex = Random.Range(0, starType.Length);
        StarBackground = Instantiate(starType[starTypeIndex]) as GameObject;
        StarBackground.transform.parent = this.transform;
        StarBackground.transform.localScale = Vector3.one;
        StarBackground.transform.localPosition = Vector3.zero;
        string color = StarBackground.tag;
        starColor = (StarColor)Enum.Parse(typeof(StarColor), color);
        StarBackground.GetComponent<Star>().ColIndex = ColIndex;
        StarBackground.GetComponent<Star>().RowIndex = RowIndex;
        StarBackground.GetComponent<Star>().starColor = starColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameControlller.Ins.Select(this);
    }
}
