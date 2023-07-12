using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;


public class FasterTask : MonoBehaviour
{
    private CancellationTokenSource _cancellationToken;

    private async void Start()
    {
        _cancellationToken = new CancellationTokenSource();
        Task<bool> result = WhatTaskFasterAsync(_cancellationToken.Token, Task1(), Task2());
        bool taskResult = await result;
        Debug.Log($"Task finished: {taskResult}");
    }

    private async Task<bool> WhatTaskFasterAsync(CancellationToken token, Task task1, Task task2)
    {
        Task completedTask = await Task.WhenAny(task1, task2);

        if (completedTask == task1)
        {
            CancelTask(task2);
            return true;
        }

        else if (completedTask == task2)
        {
            CancelTask(task1);
            return false;
        }

        else if (_cancellationToken.IsCancellationRequested)
        {
            CancelTask(task1);
            CancelTask(task2);
            return false;
        }
        throw new InvalidOperationException("Unexpected task completion.");
    }

    private void CancelTask(Task task)
    {
        if (!task.IsCompleted)
            _cancellationToken.Cancel();
    }

    private Task Task1()
    {
        return Task.Delay(2000);
    }

    private Task Task2()
    {
        return Task.Delay(1000);
    }
}

