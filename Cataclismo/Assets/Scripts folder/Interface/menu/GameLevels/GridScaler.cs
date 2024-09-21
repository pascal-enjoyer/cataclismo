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
        // Получаем текущее разрешение экрана
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Вычисляем соотношение сторон экрана
        float aspectRatio = screenWidth / screenHeight;

        // Вычисляем высоту клетки
        float cellHeight = cellWidth * (screenHeight / screenWidth);

        // Применяем новые размеры клеток (ширина остается константной)
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
