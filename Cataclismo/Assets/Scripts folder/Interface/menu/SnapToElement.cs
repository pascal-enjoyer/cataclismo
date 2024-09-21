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
    public float snapThreshold = 0.2f;  // ����� ��� ������ ��������
    public float smoothTime = 0.3f;     // ����� ��� �������� ��������
    public float fadeDuration = 0.5f;   // ����� ������������ ������

    private bool isSnapping = false;
    private Vector2 targetNormalizedPosition;
    private Vector2 velocity = Vector2.zero;  // ���������� ��� �������� �����������
    private GameObject[] overlays;  // ������ �������� ������� ���������
    private int previousIndex = -1;  // ���������� ��� �������� ����������� ���������� �������

    void Start()
    {
        // ����������� ������� ��������� ������� ScrollRect
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        // ������ ��� �������� ��������� ������� (null, ���� ������ �������)
        overlays = new GameObject[content.childCount];

        // ��� ������ ����� �������� ��� ��������, ����� �������
        SetOverlayVisibility(0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapping)
        {
            StopAllCoroutines(); // ��������� ����� ������� snapping
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
        // ��������� ������� ������� scrollRect
        targetNormalizedPosition = normalizedPosition;
    }

    private IEnumerator SnapToClosestElement()
    {
        isSnapping = true;

        // ���������� ���������� ��������� � �� �������
        int totalElements = content.childCount;

        // ������������ ��������� ������� �� ������ ������� ������� ScrollRect
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < totalElements; i++)
        {
            // ��������������� ������� ������� ��������
            float elementNormalizedPosY = (float)i / (totalElements - 1);

            // ��������� ���������� ����� ������� ��������������� �������� ScrollRect � ���������
            float distance = Mathf.Abs(targetNormalizedPosition.y - elementNormalizedPosY);

            // ������� ��������� �������
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // ������������ ��������������� ������� ScrollRect ��� �������� � ���������� ��������
        Vector2 targetPosition = new Vector2(targetNormalizedPosition.x, (float)closestIndex / (totalElements - 1));

        // ������� ������� � �������������� Lerp ��� SmoothDamp
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
        }
    }

    // ����� ��� ���������� �������� ���������
    private void SetOverlayVisibility(int activeIndex)
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            if (i == activeIndex)
            {
                // ���� ������ ���� �������, ������� �� ������
                if (overlays[i] == null)
                {
                    overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                }
                StartCoroutine(FadeOutOverlay(overlays[i])); // ��� �������� �������� ������ ��������
            }
            else
            {
                if (overlays[i] != null) // ���� ������ ����������, ��� ����� ���������
                {
                    StartCoroutine(FadeInOverlay(overlays[i]));
                }
                else // ���� ������ ���� �������, ������� ������ � ���������
                {
                    overlays[i] = Instantiate(overlayPrefab, content.GetChild(i));
                    StartCoroutine(FadeInOverlay(overlays[i]));
                }
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
