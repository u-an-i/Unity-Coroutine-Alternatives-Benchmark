# Unity-Coroutine-Alternatives-Benchmark
*benchmark of ways to "animate" in Unity*  
  
You will need https://assetstore.unity.com/packages/tools/animation/more-effective-coroutines-free-54975, to be put inside `Unity project/Assets/Plugins/`  
  
---
    
benchmarks  
- Update
- Coroutine
- above MEC Coroutine
- 3 own implementations 2 of which use .NET Collections and 1 of those directly
  
by moving GameObjects  
and  
benchmarks
- baseline by no movement
  
---
  
## Results
  
### Unity 2023.1.6, URP + IL2CPP
![coroutine benchmarks results update](https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/assets/84718885/bd50414c-7cba-4443-9c89-86a13337ee02)
  
Excel files containing benchmark results of older Unity versions, Mono and Standard Built-In rendering pipeline are inside this repository.  
  
### direct comparison showing the winning implementation of my own implementations only
![coroutine benchmarks results animation ways comparison 1000 GO](https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/assets/84718885/049ee421-1b98-4677-b847-decf1e0b88e8)
![coroutine benchmarks results animation ways comparison 10000 GO](https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/assets/84718885/5fb55628-6c10-4dcd-a881-4241dd246dcf)
![coroutine benchmarks results preparation time comparison](https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/assets/84718885/a3630d66-583f-4cff-b6f1-88a6db0d27f2)
![coroutine benchmarks results cleanup time comparison](https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/assets/84718885/4928fe2e-f553-42a3-b037-bd43e1e394c2)
  
  
## winner
  
my own implementation using my own data structure
  
That is this code: https://github.com/u-an-i/Unity-Coroutine-Alternatives-Benchmark/blob/main/Unity%20project/Assets/My/CoroutineReplacement2.cs  
  
Usage:  
Put it on an active GameObject in your Scene, it is the manager.  
Use `Timing.RunCoroutine` optionally passing an int from 0 to 4 for a bin as the second parameter.  
The first parameter is your code which must now return an `IEnumerator<float>`.  
Use `Timing.WaitForSeconds` and `Timing.KillCoroutines` respectively, passing either the `CoroutineHandle` returned from `Timing.RunCoroutine` or the bin int passed to it to the latter to cancel the running coroutine early or to cancel all running coroutines of that bin early.  
  
Do not instruct to kill an already killed coroutine.  
You can set `CoroutineHandle.bin` of a handle referencing a killed coroutine to null, for example at the end of your coroutined code if you stored the handle returned from `Timing.RunCoroutine` in an outer scope, and check it against null before you would instruct to kill it.    
  
  
## takeaway  
  
What (do) you think (?).  
  
