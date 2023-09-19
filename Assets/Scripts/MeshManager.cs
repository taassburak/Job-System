using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using System;
using Unity.Burst;

[RequireComponent(typeof(MeshFilter))]
public class MeshManager : MonoBehaviour
{

    [SerializeField] private Vector3 _rippleOrigin;

    private Mesh _mesh;
    private Vector3[] _verticesArray;
    private Vector3[] _newVerticesArray;

    private NativeArray<float3> _verticesNativeArray;
    private NativeArray<float3> _newVerticesNativeArray;

    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _verticesArray = _mesh.vertices;
        _newVerticesArray = new Vector3[_mesh.vertexCount];

        _verticesNativeArray = new NativeArray<float3>(_mesh.vertexCount, Allocator.Persistent);
        _newVerticesNativeArray = new NativeArray<float3>(_mesh.vertexCount, Allocator.Persistent);
        using (var dataArray = Mesh.AcquireReadOnlyMeshData(_mesh))
        {
            dataArray[0].GetVertices(_verticesNativeArray.Reinterpret<Vector3>());
        }
    }

    [BurstCompile]
    void Update()
    {
        //for (int i = 0; i < _verticesArray.Length; i++)
        //{
        //    Vector3 originalVertex = _verticesArray[i];
        //    float distance = Vector3.Distance(originalVertex, _rippleOrigin);
        //    float rippleAmount = Mathf.Sin(distance - Time.time);
        //    Vector3 offset = (originalVertex - _rippleOrigin).normalized * rippleAmount;
        //    Vector3 newPos = originalVertex + offset;

        //    _newVerticesArray[i] = newPos;
        //}

        //_mesh.SetVertices(_newVerticesArray);

        DeformMeshJob deformJob = new DeformMeshJob()
        {
            OriginalVertices = _verticesNativeArray,
            RipplePosition = _rippleOrigin,
            DeltaTime = Time.time,

            NewVertices = _newVerticesNativeArray,

        };


        JobHandle deformJobHandle = deformJob.Schedule(_verticesArray.Length, 1);
        
        Debug.Log("Schedule Completed");

        deformJobHandle.Complete();

        _mesh.SetVertices(deformJob.NewVertices);

        //_newVerticesNativeArray.Dispose();
        //_verticesNativeArray.Dispose();
    }

}
