using Kun.Tool;
using UnityEngine;

public class PlayerController : FlowComponent
{
    [SerializeField]
    float speed = 5f;

    [EasyInject]
    InputController input;

    InteractInputProvider InteractInput => input.InteractInput;

    protected override void DoUpdate (float deltaTime)
    {
        base.DoUpdate (deltaTime);

        Vector2Int moveInput = InteractInput.GetMoveInput ();
        Vector3 moveDirection = new Vector3 (moveInput.x, moveInput.y, 0f);

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize ();
        }

        transform.position += moveDirection * speed * deltaTime;
    }
}
