using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IdleState : Internal.State<EnemyState, EnemyCommand, EnemyInfo>
    {
        float movingInterval;
        float movingTime;
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
            movingTime = info.movingTime;
            observationInterval = info.observationInterval;
            speedMultiplier = info.speedMultiplier;
            angleAtEachObservation = info.angleAtEachObservation;

            InitializeObservation();
            movingDeltaTime = movingInterval;
            stopping = true;

            Debug.Log("Enter idle mode");
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
                if (stopping)
                {
                    movingDeltaTime = movingTime;
                    stopping = false;
                    info.Movement.SetSpeed(speedMultiplier);
                }
                else
                {
                    movingDeltaTime = movingInterval;
                    stopping = true;
                    info.Movement.SetSpeed(0);
                }
            }

            CheckSensors();

            return null;
        }

        void CheckSensors()
        {
            var sensorInfo = info.Sensor.GetCurrentStatus();
            foreach (var sensor in sensorInfo)
            {
                string tag = (sensor.visibleObject == null) ? string.Empty : sensor.visibleObject.tag;
                bool isAttackTarget = info.RelationshipTable.RelationshipTo(tag) == Internal.Relationship.Attack;
                if (isAttackTarget)
                {
                    info.target = sensor.visibleObject;
                    ChangeState(EnemyState.Track);
                    break;
                }
            }
        }

        public override void Deactivate()
        {
            info.Movement.AbortAllTurn();
            info.Movement.SetSpeed(0);
        }
    }
}