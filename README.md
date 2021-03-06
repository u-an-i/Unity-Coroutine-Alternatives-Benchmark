# Unity-Coroutine-Alternatives-Benchmark
benchmark of ways to "animate" in Unity

You will need https://assetstore.unity.com/packages/tools/animation/more-effective-coroutines-free-54975

benchmarks baseline, Update, Coroutine, above MEC Coroutine and 3 own implementations 2 of which use .NET Collections and 1 of which directly


**Results**:

![coroutine benchmark results](https://user-images.githubusercontent.com/84718885/156923180-f3fac92f-5b82-41b6-bbab-03e3353b009d.png)


**winner**:

my own implementation using my own data structure

That is this code: https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/blob/main/Unity%20project/Assets/My/CoroutineReplacement2.cs  
Usage: Put it on an active GameObject in your Scene, it is the manager. Use `Timing.RunCoroutine` optionally passing an int from 0 to 4 for a bin as the second parameter. The first parameter is your code which must now return an `IEnumerator<float>`. Use `Timing.WaitForSeconds` and `Timing.KillCoroutines` respectively, passing either the `CoroutineHandle` returned from `Timing.RunCoroutine` or the bin int passed to it to the latter to cancel the running coroutine early or to cancel all running coroutines of that bin early. Do not instruct to kill an already killed coroutine. Set `CoroutineHandle.bin` of a handle referencing a killed coroutine to null. Best do so at the end of your code in a coroutine too if you stored the handle returned from `Timing.RunCoroutine`.


**takeaway**:

Unity's Update is already very effective and moving 10,000 individual Updates to 1 single Update lets improve performance only by a rather very small amount.


**note**:

this project is made with Unity 2022.1 and uses its Standard built-in rendering pipeline.
