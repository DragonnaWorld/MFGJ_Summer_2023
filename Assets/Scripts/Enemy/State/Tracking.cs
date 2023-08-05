using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class TrackState : Internal.State<EnemyState, EnemyCommand, EnemyInfo>
    {
        Vector3 lastSeenPosition;
        float acceptableRange;

        public override void Activate()
        {
            Debug.Log("Entered tracking mode");
            lastSeenPosition = info.target.transform.position;
            acceptableRange = info.acceptableRange;
        }

        public override HashSet<EnemyCommand> Update()
        {
            var sensorsInfo = info.Sensor.GetCurrentStatus();

            bool targetStillInSight = false;
            bool inAcceptableRange = false;
            foreach (var sensor in sensorsInfo)
                if (sensor.visibleObject == info.target)
                {
                    targetStillInSight = true;
                    lastSeenPosition = sensor.visibleObject.transform.position;
                    inAcceptableRange = sensor.ratio * info.Sensor.Length <= acceptableRange;
                    break;
                }
    
            if (!targetStillInSight)
                inAcceptableRange = Vector3.Distance(info.transform.position, lastSeenPosition) <= acceptableRange;   

            if (!inAcceptableRange)
                AdvanceToPosition(lastSeenPosition);
            else if (targetStillInSight)
            {
                StopMovement();
                // Do some attack I guess
            }
            else
            {
                StopMovement();
                ChangeState(EnemyState.Idle);
            }

            return null;
        }

        void StopMovement()
        {
            info.Movement.AbortAllTurn();
            info.Movement.SetSpeed(0F);
        }

        void AdvanceToPosition(Vector3 position)
        {
            var direction = position - info.transform.position;
            float directionAngleToOx = Mathf.Rad2Deg * Mathf.Acos(direction.x / direction.magnitude) * Mathf.Sign(direction.z);
            float angleDelta = AngleNormalizer.ShortestDifference360(info.Movement.CurrentAngleToOx, directionAngleToOx);
            info.Movement.AbortAllTurn();
            info.Movement.SetSpeed(1F);
            info.Movement.AddTurn(-angleDelta);
        }
    }
}