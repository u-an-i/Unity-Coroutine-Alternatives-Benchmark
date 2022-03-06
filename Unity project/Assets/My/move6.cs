using UnityEngine;
using u_i_3;
using System.Collections.Generic;

public class move6 : move
{
    public move6()
    {
        base.id = "u&i .NET List<T> with custom removal storage backed";
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
