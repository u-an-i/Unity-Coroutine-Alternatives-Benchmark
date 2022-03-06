using UnityEngine;

public class move : MonoBehaviour
{
    public string getId()
    {
        return id;
    }

    public virtual void Stop()
    { }

    public virtual void Move()
    { }

    protected string id;

    protected float speed;

    void Awake()
    {
        transform.position = new Vector3(0F, -10F + Random.value * 20F, 0F);
        speed = -10F + -Random.value * 30F;
    }
}
