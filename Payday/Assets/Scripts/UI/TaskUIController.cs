using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TaskUIController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI moneyRewardText;
    public TextMeshProUGUI countText;

    [SerializeField] GameObject checkMark;

    [SerializeField] GameObject killLogo;
    [SerializeField] GameObject collectLogo;
    public void CompletedEffect()
    {
        checkMark.gameObject.SetActive(true);
    }

    public void SetTaskType(TaskType taskType)
    {
        if(taskType == TaskType.Kill)
        {
            killLogo.SetActive(true);
            collectLogo.SetActive(false);
        }
        else
        {
            killLogo.SetActive(false);
            collectLogo.SetActive(true);
        }
    }
}
