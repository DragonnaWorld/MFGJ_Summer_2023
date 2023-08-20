using Internal;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class IdleAndMovingState : State<PlayerState, PlayerInfo>
    {
        public override void Activate()
        {
            info.Animator.Play(PlayerInfo.IdleName);
        }

        public override void FixedUpdate()
        {
            Vector3 direction = new();
            if (Input.GetKey(KeyCode.A))
                direction.x -= 1F;
            if (Input.GetKey(KeyCode.S))
                direction.z -= 1F;
            if (Input.GetKey(KeyCode.D))
                direction.x += 1F;
            if (Input.GetKey(KeyCode.W))
                direction.z += 1F;
            Vector3 velocity = direction.normalized * info.Speed;
            info.Rgbody.velocity = new Vector3(velocity.x, info.Rgbody.velocity.y, velocity.z);
        }

        public override void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                ChangeState(PlayerState.Attack);
        }
    }
}