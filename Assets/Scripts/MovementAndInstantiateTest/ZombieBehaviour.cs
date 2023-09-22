using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{

    private NativeArray<Vector3> _positionResult;
    private bool _isInitialized;
    private JobHandle _jobHandle;
    public void Initialize()
    {
        _positionResult = new NativeArray<Vector3>(1, Allocator.Persistent);
        _positionResult[0] = transform.position;

        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized)
        {

            ZombieMoveJob zombieMoveJob = new ZombieMoveJob()
            {
                TargetDirection = Vector3.zero,
                DeltaTime = Time.deltaTime,
                Position = transform.position,
                Speed = 5,
                PositionResult = _positionResult,

            };

            Debug.Log(zombieMoveJob.Position);
            Debug.Log(zombieMoveJob.TargetDirection * zombieMoveJob.DeltaTime * zombieMoveJob.Speed);
            _jobHandle = zombieMoveJob.Schedule();

            _jobHandle.Complete();
        }

    }

    private void LateUpdate()
    {
        if (_isInitialized)
        {
            _jobHandle.Complete();
            transform.position = _positionResult[0];
        }
    }

    private void OnDestroy()
    {
        _positionResult.Dispose();
    }
}
