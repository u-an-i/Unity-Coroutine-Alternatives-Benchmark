# Unity-Coroutine-Alternatives-Benchmark
*benchmark of ways to "animate" in Unity*  

Coroutines in Unity are useful when wanting to let perform "animations" parallel to the game logic of a MonoBehaviour (MonoBehaviours are put on GameObjects).  
They are characterised by a usual sudden start and a usual finite, relative short duration.  
For example when the Update loop of a MonoBehaviour performs GameObject movement and hit detection and on hit the gameplay designer likes the GameObject to perform a "shaking" movement,
this shaking movement can be implemented by a Coroutine. That Coroutine would be executed by Unity next to the Update loop and could change the rotation of the GameObject acting on top of the usual Update movement to mimic the shaking movement.  
Coroutines were (and still are according to the result of this benchmark here as of writing this) reducing performance in Unity though and alternatives are offered on Unity Asset Store and here. The performance of 1 of such offered and 3 self-developed alternatives is benchmarked here.  
Another alternative to a Coroutine is another MonoBehaviour which performs the "animation" in its Update loop and which gets enabled on demand.  
Besides framerate performance benchmarking, preparation and cleanup duration of each method is measured. For the Update case for example, the duration to enable the MonoBehaviour is measured, for a Coroutine alternative case the duration to call the start of it is measured.  
Having established the need for a Coroutine is a "sudden one-off", they and their alternatives can still be used to replace the usual game logic Update loop itself. For this reason, and because the "sudden one-off"s can actually occur multiple times in a gameplay session having many units represented by GameObjects many of which could be hit almost at once, and for the reason of determining how each method scales, this benchmark here benchmarks the application of each method on 10, 100, 1000, 10000 and 100000 GameObjects.  
  
You will need https://assetstore.unity.com/packages/tools/animation/more-effective-coroutines-free-54975, to be put inside `Unity project/Assets/Plugins/`  
  
---
    
benchmarks  
- Update
- Coroutine
- above MEC Coroutine
- 3 own implementations 2 of which use .NET Collections and 1 of those for all of its needs
  
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
  
