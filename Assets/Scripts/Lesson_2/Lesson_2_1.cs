using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Lesson_2_1 : MonoBehaviour
{
    private NativeArray<int> _narArray;
    private ZeroOutJob _zeroOutJob;
    private JobHandle _jobHandle;

    void Start()
    {
        _narArray = new NativeArray<int>(10, Allocator.TempJob);
        for (int i = 0; i < _narArray.Length; i++)
        {
            _narArray[i] = Random.Range(0, 20);
        }

        _zeroOutJob = new ZeroOutJob();
        _zeroOutJob.data = _narArray;

        _jobHandle = _zeroOutJob.Schedule();
        _jobHandle.Complete();

        for(int i=0; i<_narArray.Length; i++)
        {
            Debug.Log($"Data[{i}] = {_narArray[i]}");
        }
        _narArray.Dispose();
    }

    public struct ZeroOutJob : IJob
    {
        public NativeArray<int> data;
        public void Execute()
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 10)
                    data[i] = 0;
            }
        }
    }
}
