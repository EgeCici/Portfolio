using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public TaskUIController taskUIPrefab;
    public Transform taskUIHolder;

    public Task[] tasks;

    private Dictionary<int, TaskUIController> taskUIDict;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].Index = i;
        }

        taskUIDict = new Dictionary<int, TaskUIController>();
    }


    public void TaskCalled(int index)
    {
        Debug.Log("Task started");
        //Ui olusturucuaz

        var taskUI = Instantiate(taskUIPrefab.gameObject, taskUIHolder).GetComponent<TaskUIController>();

        taskUIDict.Add(index, taskUI);

        UpdateTaskUI(index);
    }
    public void TaskCompleted(int index)
    {
        if (taskUIDict.ContainsKey(index))
        {
            var taskUI = taskUIDict[index];
            taskUIDict.Remove(index);

            //TODO: VFX ve SFX
            PlayerController.Instance.PlayTaskCompletedSound();
            PlayerController.Instance.CollectedMoney += tasks[index].moneyRewardAmount;

            taskUI.CompletedEffect();
            StartCoroutine(DestoryTaskUI(taskUI.gameObject));

            if (index == 0)
            {
                tasks[1].StartTask();
                tasks[2].StartTask();
            }
        }


        //ui'i silicez
        //para odulu
    }
    public void UpdateTaskUI(int index)
    {
        if (taskUIDict.ContainsKey(index))
        {
            taskUIDict[index].titleText.text = tasks[index].title;
            taskUIDict[index].descriptionText.text = tasks[index].description;
            taskUIDict[index].moneyRewardText.text = "Reward:" + tasks[index].moneyRewardAmount.ToString()+ "$";
            taskUIDict[index].countText.text = $"{tasks[index].currentCount} / {tasks[index].targetCount}";
            taskUIDict[index].SetTaskType(tasks[index].taskType);
        }
    }

    public void TaskProgress(int index)
    {
        if(taskUIDict.ContainsKey(index))
        {
            tasks[index].currentCount++;
            UpdateTaskUI(index);
            if (tasks[index].CheckIfCompleted())
                tasks[index].TaskCompleted();

        }
    }

    private IEnumerator DestoryTaskUI(GameObject go)
    {
        yield return new  WaitForSeconds(1);
        Destroy(go);
    }
}
