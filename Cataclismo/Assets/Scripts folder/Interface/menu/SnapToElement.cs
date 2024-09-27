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
    public float smoothTime = 0.3f;     // Время для плавного перехода
    public float fadeDuration = 0.5f;   // Время исчезновения панели
    public float swipeThreshold = 0.5f; // Процент, который нужно пролистать для смены страницы (0.5 = 50%)

    private bool isSnapping = false;
    private Vector2 targetNormalizedPosition;
    private Vector2 velocity = Vector2.zero;  // Переменная для скорости сглаживания
    private GameObject[] overlays;  // Массив объектов панелей наложения
    private int previousIndex = -1;  // Переменная для хранения предыдущего выбранного индекса
    private Vector2 startDragPosition;

    private const string PageIndexKey = "SavedPageIndex";  // Ключ для сохранения индекса страницы

    void Start()
    {
        // Привязываем событие изменения позиции ScrollRect
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        // Массив для хранения созданных панелей (null, если панель удалена)
        overlays = new GameObject[content.childCount];

        // Загружаем сохранённый индекс страницы
        int savedIndex = PlayerPrefs.GetInt(PageIndexKey, 0);  // По умолчанию 0 (первая страница)
        scrollRect.normalizedPosition = new Vector2(0, (float)savedIndex / (content.childCount - 1));  // Устанавливаем сохранённую позицию

        // Затеняем панели для всех элементов, кроме активного
        SetOverlayVisibility(savedIndex, true);  // true - пропускаем затухание для активной страницы

        // Устанавливаем плавное приведение к сохранённой позиции после старта
        previousIndex = savedIndex;  // Устанавливаем сохранённый элемент как активный
        StartCoroutine(SnapToClosestElement(false));  // false - пропуск проверки перелистывания
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapping)
        {
            StopAllCoroutines(); // Остановим любое текущее snapping
            isSnapping = false;
        }
        startDragPosition = scrollRect.normalizedPosition;  // Запоминаем начальную позицию при начале свайпа
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isSnapping)
        {
            StartCoroutine(SnapToClosestElement(true));  // true - проверка перелистывания после драга
        }
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        // Обновляем текущую позицию scrollRect
        targetNormalizedPosition = normalizedPosition;
    }

    private IEnumerator SnapToClosestElement(bool checkSwipe)
    {
        isSnapping = true;

        // Определяем количество элементов и их позиции
        int totalElements = content.childCount;
        float currentPosY = scrollRect.normalizedPosition.y;
        float dragDifference = currentPosY - startDragPosition.y;

        // Определяем, достаточно ли было свайпа для перелистывания
        int closestIndex = previousIndex;
        if (checkSwipe && Mathf.Abs(dragDifference) >= swipeThreshold / (totalElements - 1))
        {
            if (dragDifference > 0 && previousIndex < totalElements - 1)
            {
                closestIndex = previousIndex + 1;  // Перелистываем на следующую страницу
            }
            else if (dragDifference < 0 && previousIndex > 0)
            {
                closestIndex = previousIndex - 1;  // Перелистываем на предыдущую страницу
            }
        }
        else
        {
            // Найти ближайшую страницу при обычной прокрутке
            closestIndex = Mathf.RoundToInt(currentPosY * (totalElements - 1));
        }

        // Рассчитываем нормализованную позицию ScrollRect для привязки к ближайшему элементу
        Vector2 targetPosition = new Vector2(targetNormalizedPosition.x, (float)closestIndex / (totalElements - 1));

        // Плавный переход с использованием Lerp
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

            // Сохраняем новый индекс страницы в PlayerPrefs
            PlayerPrefs.SetInt(PageIndexKey, closestIndex);
            PlayerPrefs.Save();
        }
    }

    // Метод для управления панелями наложения
    private void SetOverlayVisibility(int activeIndex, bool isFirstStart = false)
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            if (i == activeIndex)
            {
                if (!isFirstStart)  // Если это не первый запуск, убираем панель для активной страницы
                {
                    if (overlays[i] == null)
                    {
                        overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                    }
                    StartCoroutine(FadeOutOverlay(overlays[i])); // Для текущего элемента панель исчезает
                }
            }
            else
            {
                if (overlays[i] == null)
                {
                    overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                }
                StartCoroutine(FadeInOverlay(overlays[i]));  // Для остальных панелей задаем затемнение
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
