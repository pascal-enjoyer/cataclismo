using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapToElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;       // Сам ScrollRect
    public RectTransform content;       // Контейнер с элементами
    public GameObject overlayPrefab;    // Префаб панели наложения
    public float snapSpeed = 5f;        // Скорость привязки
    public float snapThreshold = 0.2f;  // Порог для начала привязки
    public float smoothTime = 0.3f;     // Время для плавного перехода
    public float fadeDuration = 0.5f;   // Время исчезновения панели

    private bool isSnapping = false;
    private Vector2 targetNormalizedPosition;
    private Vector2 velocity = Vector2.zero;  // Переменная для скорости сглаживания
    private GameObject[] overlays;  // Массив объектов панелей наложения
    private int previousIndex = -1;  // Переменная для хранения предыдущего выбранного индекса

    void Start()
    {
        // Привязываем событие изменения позиции ScrollRect
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        // Массив для хранения созданных панелей (null, если панель удалена)
        overlays = new GameObject[content.childCount];

        // При старте нужно затенить все элементы, кроме первого
        SetOverlayVisibility(0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapping)
        {
            StopAllCoroutines(); // Остановим любое текущее snapping
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

        // Только если элемент действительно изменился, обновляем панели наложения
        if (closestIndex != previousIndex)
        {
            SetOverlayVisibility(closestIndex);
            previousIndex = closestIndex;  // Сохраняем текущий индекс как предыдущий
        }
    }

    // Метод для управления панелями наложения
    private void SetOverlayVisibility(int activeIndex)
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            if (i == activeIndex)
            {
                // Если панель была удалена, создаем ее заново
                if (overlays[i] == null)
                {
                    overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                }
                StartCoroutine(FadeOutOverlay(overlays[i])); // Для текущего элемента панель исчезает
            }
            else
            {
                if (overlays[i] != null) // Если панель существует, она будет затемнена
                {
                    StartCoroutine(FadeInOverlay(overlays[i]));
                }
                else // Если панель была удалена, создаем заново и затемняем
                {
                    overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                    StartCoroutine(FadeInOverlay(overlays[i]));
                }
            }
        }
    }

    // Плавное исчезновение панели
    private IEnumerator FadeOutOverlay(GameObject overlay)
    {
        float elapsedTime = 0f;
        Image overlayImage = overlay.GetComponent<Image>();
        Color color = overlayImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);  // Прозрачность от 1 до 0
            overlayImage.color = color;
            yield return null;
        }

        color.a = 0f;
        overlayImage.color = color;  // Обновляем прозрачность до конца

        // Удаляем панель после завершения анимации
        Destroy(overlay);
    }

    // Плавное появление панели
    private IEnumerator FadeInOverlay(GameObject overlay)
    {
        float elapsedTime = 0f;
        Image overlayImage = overlay.GetComponent<Image>();
        Color color = overlayImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);  // Прозрачность от 0 до 1
            overlayImage.color = color;
            yield return null;
        }

        color.a = 1f;
        overlayImage.color = color;  // Обновляем прозрачность до конца
    }
}
