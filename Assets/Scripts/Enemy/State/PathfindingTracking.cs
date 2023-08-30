using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Enemy
{
    public class PathfindingTrackState : Internal.State<EnemyState, EnemyInfo>
    {
        Vector3 lastSeenPosition;
        Vector3 goal;
        Status status;

        float thinkingTime;
        List<Vector3> path;

        int currentNodeIndex;

        float waitTimeBeforeIdle;

        enum Status
        {
            PathGenerating,
            PathOnGoing,
            Scouting
        }

        public override void Activate()
        {
            lastSeenPosition = info.TargetOfInterest.transform.position;
            goal = lastSeenPosition;
            thinkingTime = info.PathfindingTime;
            currentNodeIndex = 0;
            
            info.Animator.PlayContinue(EnemyInfo.IdleName);

            status = Status.PathGenerating;

            info.Movement.SetTurn(0F);
            info.Movement.SetSpeed(0F);
        }

        public override void Update()
        {
            bool stillSeeTarget = info.TargetInSight(out Sensor.HitInfo hit);
            if (stillSeeTarget)
            {
                lastSeenPosition = hit.visibleObject.transform.position;
                bool canAttack = Vector3.Distance(lastSeenPosition, info.Movement.transform.position) < info.AcceptedRange;
                if (canAttack)
                {
                    ChangeState(EnemyState.Attack);
                    return;
                }
            }

            if (status == Status.PathGenerating)
            {
                thinkingTime -= Time.deltaTime;
                if (thinkingTime < 0F)
                {
                    status = Status.PathOnGoing;
                    currentNodeIndex = 0;
                    info.Animator.PlayRestart(EnemyInfo.MoveName);
                    if (info.Pathfinder.FindPathToPosition(goal, out path))
                        status = Status.PathOnGoing;
                    else
                        status = Status.Scouting;
                }
            }
            else if (status == Status.PathOnGoing)
            {
                Vector3 offset = path[currentNodeIndex] - info.Movement.transform.position;
                bool isInNode = offset.magnitude < info.NodeRange;
                if (!isInNode)
                {
                    float targetAngle = Mathf.Abs(offset.z) <= 1e-5F ? (offset.x >= 0 ? 90 : -90) : Mathf.Rad2Deg * Mathf.Acos(offset.z / offset.magnitude) * (offset.x >= 0 ? 1 : -1); ;
                    float deltaAngle = AngleNormalizer.ShortestDifference360(info.Movement.CurrentAngle, targetAngle);
                    info.Movement.SetTurn(deltaAngle / 60F);
                    float angleFactor = 1F - Mathf.Clamp01(deltaAngle / 90F);
                    float distanceFactor = Mathf.Clamp01(offset.magnitude / 4F / info.NodeRange);
                    info.Movement.SetSpeed(angleFactor * distanceFactor);
                }
                else
                {
                    if (currentNodeIndex == path.Count - 1)
                    {
                        status = Status.Scouting;
                        info.Movement.SetSpeed(0F);
                        info.Movement.SetTurn(1F);
                        waitTimeBeforeIdle = info.FailWaitTime;
                        info.Animator.PlayRestart(EnemyInfo.IdleName);
                    }
                    else
                        currentNodeIndex++;
                }
            }
            else if (status == Status.Scouting)
            {
                waitTimeBeforeIdle -= Time.deltaTime;
                if (waitTimeBeforeIdle < 0F)
                    ChangeState(EnemyState.Idle);
            }
        }
    }
}