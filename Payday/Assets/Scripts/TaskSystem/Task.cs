using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[CreateAssetMenu(fileName = "Task", menuName = "Task")]

public class Task : ScriptableObject
{
    public int Index { get; set; }

    public int currentCount { get; set; }
    public int targetCount; //kac tane oldurmemiz gerektigi
    public int moneyRewardAmount;

    public string title;
    public string description;

    public TaskType taskType;

    public virtual void StartTask()
    {
        currentCount = 0;
        if(TaskManager.Instance != null)
        {
            TaskManager.Instance.TaskCalled(Index);
        }
    }

    public virtual void TaskCompleted()
    {
        Debug.Log("1");
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.TaskCompleted(Index);
        }

    }


    public bool CheckIfCompleted()
    {
        return currentCount >= targetCount;
    }

}

public enum TaskType
{
    Kill,
    Collect
}

