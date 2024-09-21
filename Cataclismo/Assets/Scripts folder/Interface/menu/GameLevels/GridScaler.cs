using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridScaler : MonoBehaviour
{
    public SafeArea SafeArea;
    public GridLayoutGroup gridLayoutGroup;
    public int columns;

    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        AdjustGrid();
    }

    void AdjustGrid()
    {
         float cellWidth = 1080f;
        // �������� ������� ���������� ������
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ��������� ����������� ������ ������
        float aspectRatio = screenWidth / screenHeight;

        // ��������� ������ ������
        float cellHeight = cellWidth * (screenHeight / screenWidth);

        // ��������� ����� ������� ������ (������ �������� �����������)
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
