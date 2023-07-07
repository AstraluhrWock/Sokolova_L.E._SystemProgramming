using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

internal class Lesson_2_2 : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _countElements = 2;

    private TransformAccessArray _transformAccessArray;
    private NativeArray<Vector3> _positionsArray;
    private NativeArray<Vector3> _velocitiesArray;
    private NativeArray<Vector3> _finalPositionsArray;

    private CalculatePositionsJob _calculatePositionsJob;
    private JobHandle _jobHandle;

    private void Start()
    {
        _transformAccessArray = new TransformAccessArray(_countElements);
        _positionsArray = new NativeArray<Vector3>(_countElements, Allocator.TempJob);
        _velocitiesArray = new NativeArray<Vector3>(_countElements, Allocator.TempJob);
        _finalPositionsArray = new NativeArray<Vector3>(_countElements, Allocator.TempJob);
        //_prefab = Resources.Load<GameObject>("Cube");

        for (int i = 0; i < _countElements; i++)
        {
            GameObject gameObject = Instantiate(_prefab, new Vector3(i * 2, 0f, 0f), Quaternion.identity);
            _transformAccessArray.Add(gameObject.transform);
            _positionsArray[i] = gameObject.transform.position;
            Debug.Log($"Position[{i}] = {_positionsArray[i]}");      
            _velocitiesArray[i] = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Debug.Log($"Velocity[{i}] = {_velocitiesArray[i]}");  
        }

        _calculatePositionsJob = new CalculatePositionsJob();
        _calculatePositionsJob.Positions = _positionsArray;
        _calculatePositionsJob.Velocities = _velocitiesArray;
        _calculatePositionsJob.FinalPositions = _finalPositionsArray;

        _jobHandle = _calculatePositionsJob.Schedule(_transformAccessArray);
        _jobHandle.Complete();

        for (int i = 0; i < _countElements; i++)
        {
            Debug.Log($"FinalPosition[{i}] = {_finalPositionsArray[i]}");
        }

        _transformAccessArray.Dispose();
        _positionsArray.Dispose();
        _velocitiesArray.Dispose();
        _finalPositionsArray.Dispose();
    }



    public struct CalculatePositionsJob : IJobParallelForTransform
    { 
        [Unity.Collections.ReadOnly]
        public NativeArray<Vector3> Positions;

        [Unity.Collections.ReadOnly]
        public NativeArray<Vector3> Velocities;

        public NativeArray<Vector3> FinalPositions;

        public void Execute(int index, TransformAccess transform)
        {
            FinalPositions[index] = Positions[index] + Velocities[index];
            transform.position = FinalPositions[index];
        }
    }
}

