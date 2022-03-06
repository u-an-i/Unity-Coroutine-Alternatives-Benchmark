using System.Collections;
using UnityEngine;

public class move2 : move
{
    public move2()
    {
        base.id = "Coroutine";
    }

    public override void Move()
    {
        coroutine = StartCoroutine(update());
    }

    public override void Stop()
    {
        StopCoroutine(coroutine);
    }

    private Coroutine coroutine;

    // Update is called once per frame
    IEnumerator update()
    {
        do
        {
            yield return null;
            transform.rotation *= Quaternion.Euler(0F, base.speed * Time.deltaTime, 0F);
        }
        while (true);
    }
}
