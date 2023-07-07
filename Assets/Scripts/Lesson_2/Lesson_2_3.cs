using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

internal class Lesson_2_3 : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _countOfObject = 4;
    private Transform[] _objectsToRotate;
    private NativeArray<int> _angle;

    private TransformAccessArray _transformAccessArray;
    private RotateTransformJob _rotateTransformJob;
    private JobHandle _jobHandle;

    private void Start()
    {
        _prefab = Resources.Load<GameObject>("Cube");
        _objectsToRotate = new Transform[_countOfObject];
        _transformAccessArray = new TransformAccessArray(_countOfObject);
        _angle = new NativeArray<int>(_countOfObject, Allocator.Persistent);    

        for (int i = 0; i < _countOfObject; i++)
        {
            _objectsToRotate[i] = Instantiate(_prefab).transform;
            _transformAccessArray.Add(_objectsToRotate[i].transform);
            _angle[i] = Random.Range(0, 180);
        }   
    }

    private void Update()
    {
        _rotateTransformJob = new RotateTransformJob
        {
            angles = _angle,
            deltaTime = Time.deltaTime
        };

        _jobHandle = _rotateTransformJob.Schedule(_transformAccessArray);  
        _jobHandle.Complete();
    }

    private void OnDestroy()
    {
        _transformAccessArray.Dispose();
        _angle.Dispose();
    }
}

[BurstCompile]
public struct RotateTransformJob : IJobParallelForTransform
{
    public float deltaTime;
    public NativeArray<int> angles;

    public void Execute(int index, TransformAccess transformAccess)
    {     
        transformAccess.localRotation *= Quaternion.AngleAxis(angles[index]*deltaTime, Vector3.up);
        angles[index] = angles[index] == 180 ? 0 : angles[index] + 1;
    }
}

