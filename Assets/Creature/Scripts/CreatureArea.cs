using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgentsExamples;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class CreatureState
{
    public int playerIndex;
    [FormerlySerializedAs("agentRB")]
    public Rigidbody agentRb;
    public Vector3 startingPos;
    [FormerlySerializedAs("agentScript")] public CreatureAgent creatureAgentScript;
    public float ballPosReward;
}

public class CreatureArea : Area
{
    public GameObject egg;
    [FormerlySerializedAs("ballRB")]
    [HideInInspector]
    public Rigidbody ballRb;
    public GameObject ground;
    public GameObject centerPitch;
    Egg mBall;
    public List<CreatureState> playerStates = new List<CreatureState>();
    [HideInInspector]
    public Vector3 ballStartingPos;
    [HideInInspector]
    public bool canResetBall;

    EnvironmentParameters m_ResetParams;

    void Awake()
    {
        canResetBall = true;
        ballRb = egg.GetComponent<Rigidbody>();
        mBall = egg.GetComponent<Egg>();
        mBall.area = this;
        ballStartingPos = egg.transform.position;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

  
    public void GoalTouched(CreatureAgent.Team scoredTeam)
    {
        foreach (var ps in playerStates)
        {
            if (ps.creatureAgentScript.team == scoredTeam)
            {
                ps.creatureAgentScript.AddReward(1 + ps.creatureAgentScript.timePenalty);
            }
            else
            {
                ps.creatureAgentScript.AddReward(-1);
            }
            ps.creatureAgentScript.EndEpisode();  //all agents need to be reset
            
        }
    }

    public void ResetBall()
    {
        egg.transform.position = ballStartingPos;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

        var ballScale = m_ResetParams.GetWithDefault("ball_scale", 0.015f);
      //ballRb.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
    }
    
    [FormerlySerializedAs("agentChicken")] public CreatureAgent creatureAgent;
    public GameObject Baby;
    public Food foodPrefab;
    public Text cumulativeRewardText;

    [HideInInspector]
    public float fishSpeed = 0f;
    [HideInInspector]
    public float feedRadius = 1f;

    private List<GameObject> fishList;

    public override void ResetArea()
    {
        RemoveAllFood();
        PlaceCreature();
        PlaceBaby();
        SpawnFood(4, fishSpeed);
    }

    public void RemoveSpecificFish(GameObject fishObject)
    {
        fishList.Remove(fishObject);
        Destroy(fishObject);
    }

    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;

        if (maxRadius > minRadius)
        {
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        return center + Quaternion.Euler(0f, UnityEngine.Random.Range(minAngle, maxAngle), 0f) * Vector3.forward * radius;
    }

    private void RemoveAllFood()
    {
        if (fishList != null)
        {
            for (int i = 0; i < fishList.Count; i++)
            {
                if (fishList[i] != null)
                {
                    Destroy(fishList[i]);
                }
            }
        }

        fishList = new List<GameObject>();
    }

    private void PlaceCreature()
    {
        creatureAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        creatureAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void PlaceBaby()
    {
        Baby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        Baby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void SpawnFood(int count, float fishSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fishObject = Instantiate<GameObject>(foodPrefab.gameObject);
            fishObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            fishObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            fishObject.transform.parent = transform;
            fishList.Add(fishObject);
            fishObject.GetComponent<Food>().Speed = fishSpeed;
        }
    }

    private void Update()
    {
        cumulativeRewardText.text = creatureAgent.GetCumulativeReward().ToString("0.00");
    }
}

# region Legacy Academy
//
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using MLAgents;
//
// public class PenguinAcademy : Academy
// {
//     private PenguinArea[] penguinAreas;
//
//     public override void AcademyReset()
//     {
//         // Get the penguin areas
//         if (penguinAreas == null)
//         {
//             penguinAreas = FindObjectsOfType<PenguinArea>();
//         }
//
//         // Set up areas
//         foreach (PenguinArea penguinArea in penguinAreas)
//         {
//             penguinArea.fishSpeed = resetParameters["fish_speed"];
//             penguinArea.feedRadius = resetParameters["feed_radius"];
//             penguinArea.ResetArea();
//         }
//     }
// }
//

# endregion
