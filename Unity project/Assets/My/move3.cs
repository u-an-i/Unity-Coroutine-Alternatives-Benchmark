using UnityEngine;
using MEC;
using System.Collections.Generic;

public class move3 : move
{
    public move3()
    {
        base.id = "MEC Coroutine";
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
