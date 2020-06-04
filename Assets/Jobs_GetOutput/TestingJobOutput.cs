/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class TestingJobOutput : MonoBehaviour {

    private void Start() {
        NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
        SimpleJob simpleJob = new SimpleJob {
            a = 1,
            b = 2,
            result = result,
        };
        JobHandle jobHandle = simpleJob.Schedule();

        jobHandle.Complete();

        Debug.Log(simpleJob.result[0]);

        result.Dispose();
    }

}


public struct SimpleJob : IJob {

    public int a;
    public int b;
    public NativeArray<int> result;

    public void Execute() {
        result[0] = a + b;
    }

}