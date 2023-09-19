using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

[BurstCompile]
public struct DeformMeshJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> OriginalVertices;
    [ReadOnly] public float3 RipplePosition;
    [ReadOnly] public float DeltaTime;

    public NativeArray<float3> NewVertices;
    public void Execute(int index)
    {
        float3 originalVertex = OriginalVertices[index];
        float distance = math.distance(originalVertex, RipplePosition);
        float rippleAmount = math.sin(distance - DeltaTime);
        float3 offset = math.normalize(originalVertex - RipplePosition) * rippleAmount;
        float3 newPos = originalVertex + offset;

        NewVertices[index] = newPos;
    }
}
