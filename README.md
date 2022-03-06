# Unity-Coroutine-Alternatives-Benchmark
benchmark of ways to "animate" in Unity

You will need https://assetstore.unity.com/packages/tools/animation/more-effective-coroutines-free-54975

benchmarks baseline, Update, Coroutine, above MEC Coroutine and 3 own implementations 2 of which use .NET Collections and 1 of which directly


**Results**:

![coroutine benchmark results](https://user-images.githubusercontent.com/84718885/156923180-f3fac92f-5b82-41b6-bbab-03e3353b009d.png)


**winner**:

my own implementation using my own data structure


**takeaway**:

Unity's Update is already very effective and moving 10,000 individual Updates to 1 single Update lets improve performance only by a rather very small amount.


**note**:

this project is made with Unity 2022.1 and uses its Standard built-in rendering pipeline.
