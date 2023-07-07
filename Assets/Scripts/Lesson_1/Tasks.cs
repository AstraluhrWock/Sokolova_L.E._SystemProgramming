using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class Tasks : MonoBehaviour
{
    private void Task1(CancellationToken token)
    {
        Debug.Log("Task 1 has completed its work");
    }

    private void Task2(CancellationToken token)
    {
        Debug.Log("Task 2 has completed its work");
    }
}

