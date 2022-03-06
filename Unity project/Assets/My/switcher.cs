using UnityEngine;

public class switcher : MonoBehaviour
{
    private move[] type = new move[Settings.Values.numTests];

    private int currentType = -1;

    void Awake()
    {
        enabled = false;
        type[0] = GetComponent<move1>();            // Note: this order needs to be take into account for Settings.Transitions
        type[1] = GetComponent<move2>();
        type[2] = GetComponent<move4>();
        type[3] = GetComponent<move5>();
        type[4] = GetComponent<move6>();
        type[5] = GetComponent<move3>();            // at end because it auto-creates a MonoBehaviour I did not want to manually destroy
    }

    public void switchToNextMethod()
    {
        ++currentType;
    }

    public void stopCurrentMethod()
    {
        type[currentType].Stop();
    }

    public void moveCurrentMethod()
    {
        type[currentType].Move();
    }

    public move getMover(int i)
    {
        return type[i];
    }
}
