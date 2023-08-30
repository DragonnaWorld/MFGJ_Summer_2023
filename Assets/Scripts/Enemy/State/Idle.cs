using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IdleState : Internal.State<EnemyState, EnemyInfo>
    {
        public override void Activate()
        {
            info.Movement.SetSpeed(0F);
            info.Movement.SetTurn(-1F);
            info.Animator.PlayRestart(EnemyInfo.IdleName);
        }

        public override void Update()
        {
            if (AttackTargetSpotted(out Sensor.HitInfo hitInfo))
            {
                info.TargetOfInterest = hitInfo.visibleObject;
                ChangeState(EnemyState.Track);
            }
        }

        bool AttackTargetSpotted(out Sensor.HitInfo hitInfo)
        {
            hitInfo = new();
            var sensorsInfo = info.Sensor.GetCurrentStatus();

            foreach (var hit in sensorsInfo)
            {
                if (!hit.hit)
                    continue;
                var visibleThing = hit.visibleObject.tag;
                if (info.RelationshipTable.WillAttack(visibleThing))
                {
                    hitInfo = hit;
                    return true;
                }
            }
            return false;
        }
    }
}