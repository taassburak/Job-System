using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct ZombieMoveJob : IJob
{
    public Vector3 TargetDirection;
    public float Speed;
    public float DeltaTime;
    public Vector3 Position;

    public NativeArray<Vector3> PositionResult;


    [BurstCompile]
    public void Execute()
    {
        Vector3 positionChange = (TargetDirection - PositionResult[0]).normalized * Speed * DeltaTime;
        Position += positionChange;

        PositionResult[0] = Position;
    }
}
