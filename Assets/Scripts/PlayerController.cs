using Kun.Tool;
using UnityEngine;

public class PlayerController : FlowComponent
{
    [SerializeField]
    float speed = 5f;

    [EasyInject]
    InputController input;

    [EasyInject]
    BlockManager blockManager;

    InteractInputProvider InteractInput => input.InteractInput;

    [SerializeField]
    BoxCollider2D edge;

    protected override void Init ()
    {
        base.Init ();
    }

    protected override void DoUpdate (float deltaTime)
    {
        base.DoUpdate (deltaTime);

        Vector2Int moveInput = InteractInput.GetMoveInput ();
        Vector3 moveDirection = new Vector3 (moveInput.x, moveInput.y, 0f);

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize ();
        }

        Vector3 moveDelta = moveDirection * speed * deltaTime;

        transform.position = blockManager.CheckEdge (transform.position, moveDelta, edge);
    }
}
