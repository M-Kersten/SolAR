using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialManager : MonoBehaviour
{
    /// <summary>
    /// A list holding all tutorialpages in chronological order
    /// </summary>
    [Tooltip("A list holding all tutorialpages in order")]
    [SerializeField]
    private List<UITutorialStep> tutorialPages;
    /// <summary>
    /// A list holding all tutorialDots in chronological order (same order as tutorialpages)
    /// </summary>
    [Tooltip("A list holding all tutorialDots in order (same order as tutorialpages)")]
    [SerializeField]
    private List<Image> tutorialDots;
    [SerializeField]
    private float fadeDuration;
    /// <summary>
    /// index of the currently active tutorial page
    /// </summary>
    private int currentTutorialPage;

    private void OnEnable()
    {
        tutorialPages.ForEach(page => page.gameObject.SetActive(false));
        LoadTutorialPage(0, true);
    }

    /// <summary>
    /// Cycles 1 forward through tutorialpages
    /// </summary>
    public void NextTutorialPage()
    {
        if (currentTutorialPage + 1 >= tutorialPages.Count)
            LoadTutorialPage(0, false);
        else
            LoadTutorialPage(currentTutorialPage + 1, true);
    }
    /// <summary>
    /// Cycles 1 back through tutorialpages
    /// </summary>
    public void PreviousTutorialPage()
    {
        if (currentTutorialPage > 0)
            LoadTutorialPage(currentTutorialPage - 1, false);
        else
            IntroMenuManager.Instance.ViewMenu();
    }

    /// <summary>
    /// Load a tutorialpage given the index
    /// </summary>
    private void LoadTutorialPage(int pageIndex, bool forward)
    {
        if (!tutorialPages[currentTutorialPage].gameObject.activeInHierarchy)
        {
            currentTutorialPage = pageIndex;            
            tutorialDots.ForEach(dot => dot.color = Color.gray);

            tutorialPages[pageIndex].EaseIn(fadeDuration, forward);
            tutorialDots[pageIndex].color = Color.white;
        }
        else
        {
            tutorialPages[currentTutorialPage].EaseOut(fadeDuration, () =>
            {
                currentTutorialPage = pageIndex;

                tutorialDots.ForEach(dot => dot.color = Color.gray);

                tutorialPages[pageIndex].EaseIn(fadeDuration, forward);
                tutorialDots[pageIndex].color = Color.white;
            }
            );
        }
    }
}
