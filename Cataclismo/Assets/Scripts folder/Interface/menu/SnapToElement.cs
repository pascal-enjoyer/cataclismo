using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapToElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;       // Сам ScrollRect
    public RectTransform content;       // Контейнер с элементами
    public float snapSpeed = 5f;        // Скорость привязки
    public float snapThreshold = 0.2f;  // Порог для начала привязки
    public float smoothTime = 0.3f;     // Время для плавного перехода

    private bool isSnapping = false;
    private Vector2 targetNormalizedPosition;
    private Vector2 velocity = Vector2.zero;  // Переменная для скорости сглаживания

    void Start()
    {
        // Привязываем событие изменения позиции ScrollRect
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapping)
        {
            StopAllCoroutines(); // Остановим любое текущее с snapping
            isSnapping = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isSnapping)
        {
            StartCoroutine(SnapToClosestElement());
        }
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        // Обновляем текущую позицию scrollRect
        targetNormalizedPosition = normalizedPosition;
    }

    private IEnumerator SnapToClosestElement()
    {
        isSnapping = true;

        // Определяем количество элементов и их позиции
        int totalElements = content.childCount;

        // Рассчитываем ближайший элемент на основе текущей позиции ScrollRect
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < totalElements; i++)
        {
            // Нормализованная позиция каждого элемента
            float elementNormalizedPosY = (float)i / (totalElements - 1);

            // Вычисляем расстояние между текущей нормализованной позицией ScrollRect и элементом
            float distance = Mathf.Abs(targetNormalizedPosition.y - elementNormalizedPosY);

            // Находим ближайший элемент
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // Рассчитываем нормализованную позицию ScrollRect для привязки к ближайшему элементу
        Vector2 targetPosition = new Vector2(targetNormalizedPosition.x, (float)closestIndex / (totalElements - 1));

        // Плавный переход с использованием Lerp или SmoothDamp
        while (Vector2.Distance(scrollRect.normalizedPosition, targetPosition) > 0.001f)
        {
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, targetPosition, snapSpeed * Time.deltaTime);
            yield return null;
        }

        // Устанавливаем точную позицию, когда привязка завершена
        scrollRect.normalizedPosition = targetPosition;
        isSnapping = false;
    }
}