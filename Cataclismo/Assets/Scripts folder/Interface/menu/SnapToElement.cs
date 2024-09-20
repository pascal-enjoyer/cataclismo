using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapToElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;       // ��� ScrollRect
    public RectTransform content;       // ��������� � ����������
    public float snapSpeed = 5f;        // �������� ��������
    public float snapThreshold = 0.2f;  // ����� ��� ������ ��������
    public float smoothTime = 0.3f;     // ����� ��� �������� ��������

    private bool isSnapping = false;
    private Vector2 targetNormalizedPosition;
    private Vector2 velocity = Vector2.zero;  // ���������� ��� �������� �����������

    void Start()
    {
        // ����������� ������� ��������� ������� ScrollRect
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapping)
        {
            StopAllCoroutines(); // ��������� ����� ������� � snapping
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
    }
}