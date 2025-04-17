using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;   

public class MachineMan : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField inputField;
    public Button submitButton;
    public ScrollRect storyScrollView;
    public RectTransform contentPanel;

    [Header("Story Settings")]
    public float scrollSpeed = 0.1f;
    public Sprite[] storyImages;
    private string secretCode = "HAPPYBIRTHDAY";
    
    private bool isScrolling = false;
    private float targetScrollPosition = 0f;
    private List<Image> storyImageComponents = new List<Image>();

    private void Start()
    {
        submitButton.onClick.AddListener(CheckInput);
        SetupStoryImages();
        // Hide scroll view initially
        storyScrollView.gameObject.SetActive(false);
    }

    private void SetupStoryImages()
    {
        float currentY = 0f;
        float spacing = 50f; // Space between images

        foreach (Sprite sprite in storyImages)
        {
            GameObject imageObj = new GameObject("StoryImage");
            imageObj.transform.SetParent(contentPanel, false);
            
            Image imageComponent = imageObj.AddComponent<Image>();
            imageComponent.sprite = sprite;
            imageComponent.preserveAspect = true;
            imageComponent.rectTransform.sizeDelta = new Vector2(400f, 400f);
            
            // Set size and position of image container
            RectTransform rect = imageObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(800f, 600f);
            rect.anchoredPosition = new Vector2(0f, -currentY);
            
            currentY += rect.sizeDelta.y + spacing;
            storyImageComponents.Add(imageComponent);
        }

        // Add extra padding at the bottom to ensure last image is fully visible
        float totalHeight = (storyImageComponents.Count * 600f) + ((storyImageComponents.Count - 1) * spacing) + 600f;
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, totalHeight);
        
        // Initially set scroll view to top
        storyScrollView.verticalNormalizedPosition = 1f;
    }

    private void CheckInput()
    {
        string inputText = inputField.text;
        
        if (inputText.Equals(secretCode, System.StringComparison.OrdinalIgnoreCase))
        {
            StartStorySequence();
        }
    }

    private void StartStorySequence()
    {
        // Show scroll view when secret code is correct
        storyScrollView.gameObject.SetActive(true);
        isScrolling = true;
        targetScrollPosition = 0f;
    }

    private void Update()
    {
        if (isScrolling)
        {
            float currentPos = storyScrollView.verticalNormalizedPosition;
            float newPosition = Mathf.MoveTowards(currentPos, targetScrollPosition, scrollSpeed * Time.deltaTime);
            
            storyScrollView.verticalNormalizedPosition = newPosition;

            if (Mathf.Approximately(newPosition, targetScrollPosition))
            {
                isScrolling = false;
            }
        }
    }
}
