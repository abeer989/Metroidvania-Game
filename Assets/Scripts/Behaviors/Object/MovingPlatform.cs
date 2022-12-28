using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] bool isMovable;
    
    [Space]
    [SerializeField] Transform[] movePoints;

    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;

    int currentTargetedPointIndex;
    float waitCounter;

    private void OnEnable()
    {
        waitCounter = waitTime;
        currentTargetedPointIndex = 0;
        movePoints[0].transform.parent.SetParent(null);
    }

    private void Update()
    {
        if (isMovable)
        {
            float yDistance = Mathf.Abs(transform.position.y - movePoints[currentTargetedPointIndex].position.y);

            if (yDistance > 0)
            {
                transform.position = Vector3.MoveTowards
                                     (current: transform.position,
                                     target: new Vector3(transform.position.x, movePoints[currentTargetedPointIndex].position.y, transform.position.z),
                                     maxDistanceDelta: moveSpeed * Time.deltaTime);
            }

            else
            {
                waitCounter -= Time.deltaTime;

                if (waitCounter <= 0)
                {
                    waitCounter = waitTime;
                    currentTargetedPointIndex++;

                    if (currentTargetedPointIndex >= movePoints.Length)
                        currentTargetedPointIndex = 0;
                }
            }
        }
    }
}
