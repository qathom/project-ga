using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatisticsPanel : MonoBehaviour
{
    [SerializeField]
    private Text nameText;

    [SerializeField]
    private RectTransform talkScore;

    [SerializeField]
    private RectTransform talkScoreBackground;

    public float talkScoreRatio = 0;

    public void UpdateStatistics(string name, float score, float maxScore)
    {
        nameText.text = name;
        talkScoreRatio = score / maxScore;
    }

    private void Update()
    {
        talkScore.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            talkScoreRatio * talkScoreBackground.rect.width
        );
    }
}
