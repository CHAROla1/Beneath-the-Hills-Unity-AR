using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform target { get; set; }
    Transform player;

    [SerializeField] float radius = 5f;
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] float arrowHeight = -2f;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent;
    }

    void Update()
    {

        if (target != null && Vector3.Distance(player.position, new Vector3(target.position.x, player.position.y, target.position.z)) < 5f)
        {
            WayPointsManager.Instance.NextPoint();
        }
        if (WayPointsManager.Instance.currentWayPoint && target != WayPointsManager.Instance.currentWayPoint.transform)
            target = WayPointsManager.Instance.currentWayPoint.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        FaceToDirection(); // update the arrow position and rotation to the target
    }

    void FaceToDirection()
    {
        // calculate the direction from the player to the target
        Vector3 targetDirection = target.position - player.position;
        targetDirection.y = 0f; // ignore the height difference

        // calculate the position of the arrow in the circle
        Vector3 circlePosition = (player.position + new Vector3(0, arrowHeight, 0)) + targetDirection.normalized * radius;

        // move the arrow to the calculated position
        transform.position = Vector3.Lerp(transform.position, circlePosition, smoothSpeed * Time.deltaTime);

        // rotate the arrow to face the target
        transform.LookAt(new Vector3(target.position.x, player.position.y + arrowHeight, target.position.z));
    }
}
