using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IdleState : Internal.State<EnemyState, EnemyCommand, EnemyInfo>
    {
        float movingInterval;
        float observationInterval;

        float speedMultiplier;
        float angleAtEachObservation;

        float observationTime;
        float observationDeltaTime;
        float movingDeltaTime;
        bool stopping;

        public override void Activate()
        {
            movingInterval = info.movingInterval;
            observationInterval = info.observationInterval;
            speedMultiplier = info.speedMultiplier;
            angleAtEachObservation = info.angleAtEachObservation;

            InitializeObservation();
            movingDeltaTime = movingInterval;
            stopping = true;
        }

        void InitializeObservation()
        {
            observationTime = observationInterval + angleAtEachObservation / info.Movement.AngularSpeed;
            observationDeltaTime = observationTime;
            info.Movement.AddTurn(angleAtEachObservation);
        }

        public override HashSet<EnemyCommand> Update()
        {
            movingDeltaTime -= Time.deltaTime;
            observationDeltaTime -= Time.deltaTime;

            if (observationDeltaTime < 0)
            {
                InitializeObservation();
            }
            if (movingDeltaTime < 0)
            {
                movingDeltaTime = movingInterval;
                if (stopping)
                {
                    stopping = false;
                    info.Movement.SetSpeed(speedMultiplier);
                }
                else
                {
                    stopping = true;
                    info.Movement.SetSpeed(0);
                }
            }
            return null;
        }

        public override void Deactivate()
        {
            info.Movement.AbortAllTurn();
            info.Movement.SetSpeed(0);
        }
    }
}