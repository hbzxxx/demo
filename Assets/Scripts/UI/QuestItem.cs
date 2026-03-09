using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    public Text TitleText;
    public Text DescText;
    public Text ProgressText;
    public Button AcceptButton;
    public Button CompleteButton;

    private QuestData quest;

    public void Setup(QuestData quest)
    {
        this.quest = quest;
        TitleText.text = quest.Title;
        DescText.text = quest.Description;

        if (quest.State == QuestState.Accepted)
        {
            ProgressText.text = $"{quest.CurrentCount}/{quest.TargetCount}";
            ProgressText.gameObject.SetActive(true);
            AcceptButton.gameObject.SetActive(false);
            CompleteButton.gameObject.SetActive(false);
        }
        else if (quest.State == QuestState.Completed)
        {
            ProgressText.text = "完成!";
            ProgressText.gameObject.SetActive(true);
            AcceptButton.gameObject.SetActive(false);
            CompleteButton.gameObject.SetActive(true);
        }
        else if (quest.State == QuestState.Available)
        {
            ProgressText.gameObject.SetActive(false);
            AcceptButton.gameObject.SetActive(true);
            CompleteButton.gameObject.SetActive(false);
        }
    }

    public void OnAcceptClicked()
    {
        if (quest != null)
        {
            QuestManager.Instance.AcceptQuest(quest.ID);
            UIManager.Instance.ToggleQuestPanel();
        }
    }

    public void OnCompleteClicked()
    {
        if (quest != null)
        {
            QuestManager.Instance.CompleteQuest(quest.ID);
            UIManager.Instance.ToggleQuestPanel();
        }
    }
}
