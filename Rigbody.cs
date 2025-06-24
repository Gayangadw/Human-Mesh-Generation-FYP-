using UnityEngine;

public class AutoWalker : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 2f;
    public float turnSpeed = 90f; // Degrees per second

    private enum State
    {
        WalkingForward1,
        TurningRight,
        WalkingForward2,
        TurningLeft,
        WalkingForward3,
        Done
    }

    private State currentState = State.WalkingForward1;
    private float stateTimer = 0f;
    private float walkDuration = 2f; // How long to walk in seconds
    private float turnAngle = 90f; // Degrees to turn
    private float turnedAngle = 0f;

    void Update()
    {
        switch (currentState)
        {
            case State.WalkingForward1:
                Walk();
                stateTimer += Time.deltaTime;
                if (stateTimer >= walkDuration)
                    TransitionTo(State.TurningRight);
                break;

            case State.TurningRight:
                Turn(true);
                break;

            case State.WalkingForward2:
                Walk();
                stateTimer += Time.deltaTime;
                if (stateTimer >= walkDuration)
                    TransitionTo(State.TurningLeft);
                break;

            case State.TurningLeft:
                Turn(false);
                break;

            case State.WalkingForward3:
                Walk();
                stateTimer += Time.deltaTime;
                if (stateTimer >= walkDuration)
                    TransitionTo(State.Done);
                break;

            case State.Done:
                animator.SetFloat("Speed", 0f);
                break;
        }
    }

    void Walk()
    {
        animator.SetFloat("Speed", 1f);
        transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
    }

    void Turn(bool right)
    {
        animator.SetFloat("Speed", 0f);
        float turnStep = turnSpeed * Time.deltaTime;
        float direction = right ? 1f : -1f;
        transform.Rotate(0f, turnStep * direction, 0f);
        turnedAngle += turnStep;

        if (turnedAngle >= turnAngle)
        {
            turnedAngle = 0f;
            TransitionTo(right ? State.WalkingForward2 : State.WalkingForward3);
        }
    }

    void TransitionTo(State nextState)
    {
        currentState = nextState;
        stateTimer = 0f;
        turnedAngle = 0f;
    }
}
