using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobType
{
    NoJob,
    ParallelJob,
    DefaultJob
}
public class JobManager : MonoBehaviour
{
    public JobType JobType => _jobType;
    [SerializeField] private JobType _jobType;
}
