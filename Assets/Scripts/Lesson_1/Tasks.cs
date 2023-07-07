using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class Tasks : MonoBehaviour
{
    private CancellationTokenSource _tokenSource;
    private int _currentFrame = 0;
    private int _frameToWait = 60;

    private void Start()
    {
        _tokenSource = new CancellationTokenSource();
        TaskAsync1(_tokenSource.Token);
        TaskAsync2(_tokenSource.Token);
    }

    private async Task TaskAsync1(CancellationToken token)
    {
        await Task.Delay(TimeSpan.FromSeconds(1), token);
        Debug.Log("Task 1 has completed");
    }

    private async Task TaskAsync2(CancellationToken token)
    {
        while(_currentFrame < _frameToWait)
        {
            if(token.IsCancellationRequested)
            {
                Debug.Log("Task 2 canceled");
                return;
            }
            _currentFrame++;
            await Task.Yield();
        }
        Debug.Log("Task 2 has completed");
    }
}

