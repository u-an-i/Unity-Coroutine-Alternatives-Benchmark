using UnityEngine;
using u_i_1;
using System.Collections.Generic;

public class move4 : move
{
    public move4()
    {
        base.id = "u&i .NET LinkedList<T> backed";
    }

    public override void Move()
    {
        coroutine = Timing.RunCoroutine(update());
    }

    public override void Stop()
    {
        Timing.KillCoroutines(coroutine);
    }

    private CoroutineHandle coroutine;

    // Update is called once per frame
    IEnumerator<float> update()
    {
        do
        {
            yield return 0F;
            transform.rotation *= Quaternion.Euler(0F, base.speed * Time.deltaTime, 0F);
        }
        while (true);
    }
}
