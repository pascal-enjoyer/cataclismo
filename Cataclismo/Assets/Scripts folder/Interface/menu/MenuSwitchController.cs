using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSwitchController : MonoBehaviour
{


    public RectTransform[] panels; // массив панелей
    public Button[] buttons; // массив кнопок
    public RectTransform buttonFrame;
    public float transitionDuration = 0.5f; // длительность перехода
    private int currentPanelIndex = 2;


    void Start()
    {
        // Назначаем слушатели для каждой кнопки
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SwitchPanel(index));
        }

        // Устанавливаем начальные позиции панелей
        UpdatePanelPositions();

    }

    public void SwitchPanel(int panelIndex)
    {
        if (panelIndex != currentPanelIndex)
        {
            StartCoroutine(TransitionPanels(panelIndex));
            currentPanelIndex = panelIndex;
        }

        MoveFrameToButton(buttons[panelIndex]);
    }

    private void UpdatePanelPositions()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            // Панели размещены слева или справа от экрана
            panels[i].anchoredPosition = new Vector2((i - currentPanelIndex) * Screen.width, 0);
        }
    }

    private IEnumerator TransitionPanels(int targetIndex)
    {
        float elapsedTime = 0f;
        Vector2[] startPositions = new Vector2[panels.Length];
        Vector2[] targetPositions = new Vector2[panels.Length];

        for (int i = 0; i < panels.Length; i++)
        {
            startPositions[i] = panels[i].anchoredPosition;
            targetPositions[i] = new Vector2((i - targetIndex) * Screen.width, 0);
        }

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].anchoredPosition = Vector2.Lerp(startPositions[i], targetPositions[i], t);
            }
            yield return null;
        }

        // Устанавливаем конечные позиции после завершения перехода
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].anchoredPosition = targetPositions[i];
        }
    }


    private void MoveFrameToButton(Button button)
    {
        // Перемещаем рамку к позиции кнопки
        buttonFrame.position = button.transform.position;
    }

}
