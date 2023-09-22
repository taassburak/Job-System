using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Burst;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private int _maxZombieCount;
    [SerializeField] private ZombieBehaviour _zombie;

    private CancellationTokenSource _cancellationTokenSource;
    private int _zombieCount;

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartToCreateZombie();
    }

    private async void StartToCreateZombie()
    {
        while (_zombieCount <= _maxZombieCount)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            await CreateZombie();
        }
    }

    [BurstCompile]
    private async Task CreateZombie()
    {
        Debug.Log("Zombie created");
        var zombie = Instantiate(_zombie.gameObject, new Vector3(Random.Range(50, 100), 0, Random.Range(50, 100)), Quaternion.identity);
        zombie.GetComponent<ZombieBehaviour>().Initialize();
        _zombieCount++;
        await Task.Delay(500);
        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }

}
