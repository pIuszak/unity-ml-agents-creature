using UnityEngine;

public class Egg : MonoBehaviour
{
    [HideInInspector]
    public CreatureArea area;
    public string purpleGoalTag; //will be used to check if collided with purple goal
    public string blueGoalTag; //will be used to check if collided with blue goal

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(purpleGoalTag)) //ball touched purple goal
        {
            area.GoalTouched(CreatureAgent.Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            area.GoalTouched(CreatureAgent.Team.Purple);
        }
    }
}
