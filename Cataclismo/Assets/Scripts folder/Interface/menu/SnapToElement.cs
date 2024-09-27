using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapToElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;       // ��� ScrollRect
    public RectTransform content;       // ��������� � ����������
    public GameObject overlayPrefab;    // ������ ������ ���������
    public float snapSpeed = 5f;        // �������� ��������
    public float smoothTime = 0.3f;     // ����� ��� �������� ��������
    public float fadeDuration = 0.5f;   // ����� ������������ ������
    public float swipeThreshold = 0.5f; // �������, ������� ����� ���������� ��� ����� �������� (0.5 = 50%)

    private bool isSnapping = false;
    private Vector2 targetNormalizedPosition;
    private Vector2 velocity = Vector2.zero;  // ���������� ��� �������� �����������
    private GameObject[] overlays;  // ������ �������� ������� ���������
    private int previousIndex = -1;  // ���������� ��� �������� ����������� ���������� �������
    private Vector2 startDragPosition;

    private const string PageIndexKey = "SavedPageIndex";  // ���� ��� ���������� ������� ��������

    void Start()
    {
        // ����������� ������� ��������� ������� ScrollRect
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        // ������ ��� �������� ��������� ������� (null, ���� ������ �������)
        overlays = new GameObject[content.childCount];

        // ��������� ���������� ������ ��������
        int savedIndex = PlayerPrefs.GetInt(PageIndexKey, 0);  // �� ��������� 0 (������ ��������)
        scrollRect.normalizedPosition = new Vector2(0, (float)savedIndex / (content.childCount - 1));  // ������������� ���������� �������

        // �������� ������ ��� ���� ���������, ����� ���������
        SetOverlayVisibility(savedIndex, true);  // true - ���������� ��������� ��� �������� ��������

        // ������������� ������� ���������� � ���������� ������� ����� ������
        previousIndex = savedIndex;  // ������������� ���������� ������� ��� ��������
        StartCoroutine(SnapToClosestElement(false));  // false - ������� �������� ��������������
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapping)
        {
            StopAllCoroutines(); // ��������� ����� ������� snapping
            isSnapping = false;
        }
        startDragPosition = scrollRect.normalizedPosition;  // ���������� ��������� ������� ��� ������ ������
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isSnapping)
        {
            StartCoroutine(SnapToClosestElement(true));  // true - �������� �������������� ����� �����
        }
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        // ��������� ������� ������� scrollRect
        targetNormalizedPosition = normalizedPosition;
    }

    private IEnumerator SnapToClosestElement(bool checkSwipe)
    {
        isSnapping = true;

        // ���������� ���������� ��������� � �� �������
        int totalElements = content.childCount;
        float currentPosY = scrollRect.normalizedPosition.y;
        float dragDifference = currentPosY - startDragPosition.y;

        // ����������, ���������� �� ���� ������ ��� ��������������
        int closestIndex = previousIndex;
        if (checkSwipe && Mathf.Abs(dragDifference) >= swipeThreshold / (totalElements - 1))
        {
            if (dragDifference > 0 && previousIndex < totalElements - 1)
            {
                closestIndex = previousIndex + 1;  // ������������� �� ��������� ��������
            }
            else if (dragDifference < 0 && previousIndex > 0)
            {
                closestIndex = previousIndex - 1;  // ������������� �� ���������� ��������
            }
        }
        else
        {
            // ����� ��������� �������� ��� ������� ���������
            closestIndex = Mathf.RoundToInt(currentPosY * (totalElements - 1));
        }

        // ������������ ��������������� ������� ScrollRect ��� �������� � ���������� ��������
        Vector2 targetPosition = new Vector2(targetNormalizedPosition.x, (float)closestIndex / (totalElements - 1));

        // ������� ������� � �������������� Lerp
        while (Vector2.Distance(scrollRect.normalizedPosition, targetPosition) > 0.001f)
        {
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, targetPosition, snapSpeed * Time.deltaTime);
            yield return null;
        }

        // ������������� ������ �������, ����� �������� ���������
        scrollRect.normalizedPosition = targetPosition;
        isSnapping = false;

        // ������ ���� ������� ������������� ���������, ��������� ������ ���������
        if (closestIndex != previousIndex)
        {
            SetOverlayVisibility(closestIndex);
            previousIndex = closestIndex;  // ��������� ������� ������ ��� ����������

            // ��������� ����� ������ �������� � PlayerPrefs
            PlayerPrefs.SetInt(PageIndexKey, closestIndex);
            PlayerPrefs.Save();
        }
    }

    // ����� ��� ���������� �������� ���������
    private void SetOverlayVisibility(int activeIndex, bool isFirstStart = false)
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            if (i == activeIndex)
            {
                if (!isFirstStart)  // ���� ��� �� ������ ������, ������� ������ ��� �������� ��������
                {
                    if (overlays[i] == null)
                    {
                        overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                    }
                    StartCoroutine(FadeOutOverlay(overlays[i])); // ��� �������� �������� ������ ��������
                }
            }
            else
            {
                if (overlays[i] == null)
                {
                    overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                }
                StartCoroutine(FadeInOverlay(overlays[i]));  // ��� ��������� ������� ������ ����������
            }
        }
    }

    // ������� ������������ ������
    private IEnumerator FadeOutOverlay(GameObject overlay)
    {
        float elapsedTime = 0f;
        Image overlayImage = overlay.GetComponent<Image>();
        Color color = overlayImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);  // ������������ �� 1 �� 0
            overlayImage.color = color;
            yield return null;
        }

        color.a = 0f;
        overlayImage.color = color;  // ��������� ������������ �� �����

        // ������� ������ ����� ���������� ��������
        Destroy(overlay);
    }

    // ������� ��������� ������
    private IEnumerator FadeInOverlay(GameObject overlay)
    {
        float elapsedTime = 0f;
        Image overlayImage = overlay.GetComponent<Image>();
        Color color = overlayImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);  // ������������ �� 0 �� 1
            overlayImage.color = color;
            yield return null;
        }

        color.a = 1f;
        overlayImage.color = color;  // ��������� ������������ �� �����
    }
}
