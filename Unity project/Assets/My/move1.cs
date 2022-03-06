using UnityEngine;

public class move1 : move
{
    public move1()
    {
        base.id = "Update";
    }

    public override void Move()
    {
        enabled = true;
    }

    public override void Stop()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0F, base.speed * Time.deltaTime, 0F);
    }
}
