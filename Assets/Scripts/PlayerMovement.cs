using System.Collections;
using UnityEngine;
public enum Player
{
    Player1, Player2
}
public class PlayerMovement : MonoBehaviour
{
    Vector3 newDirection;
    Vector3 headPos;
    [SerializeField] float timeStep = 0.5f;
    [SerializeField] float step = 0.5f;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] Transform[] bodies;
    [SerializeField] Player player;
    int length;
    void Awake()
    {
        length = -1;
    }
    void Start()
    {
        if (player == Player.Player1)
            newDirection = Vector3.right;
        if (player == Player.Player2)
            newDirection = Vector3.left;
        bodies = GetComponentsInChildren<Transform>();
        length = bodies.Length;
        if (length < 1)
        {
            Debug.LogError("No body segments found");
            return;
        }
        StartCoroutine(MoveHead());
    }
    IEnumerator MoveHead()
    {
        while (true)
        {
            Vector3[] oldPositions = new Vector3[length];
            for (int i = 0; i < length; i++)
            {
                oldPositions[i] = bodies[i].position;
            }

            headPos = oldPositions[1];
            headPos += new Vector3(step * newDirection.x, step * newDirection.y, 0);
            bodies[1].position = headPos;

            for (int i = 2; i < length; i++)
            {
                if (bodies[i] != null && oldPositions[i - 1] != null)
                    MoveBody(i, oldPositions[i - 1]);
            }
            yield return new WaitForSeconds(timeStep);
        }
    }
    void MoveBody(int index, Vector3 prevPos)
    {
        bodies[index].transform.position = prevPos;
    }
    void Update()
    {
        if (player == Player.Player1)
        {
            if (Input.GetKeyDown(KeyCode.W) && newDirection != Vector3.down)
                newDirection = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.S) && newDirection != Vector3.up)
                newDirection = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.A) && newDirection != Vector3.right)
                newDirection = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D) && newDirection != Vector3.left)
                newDirection = Vector3.right;
        }
        if (player == Player.Player2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && newDirection != Vector3.down)
                newDirection = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && newDirection != Vector3.up)
                newDirection = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && newDirection != Vector3.right)
                newDirection = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow) && newDirection != Vector3.left)
                newDirection = Vector3.right;
        }
    }
    public Transform[] GetBodiesTransform()
    {
        bodies = GetComponentsInChildren<Transform>();
        return bodies;
    }
    [ContextMenu("Add Body")]
    public void AddBody()
    {
        StartCoroutine(AddBodyWaiter());
    }
    IEnumerator AddBodyWaiter()
    {
        Vector3 currentDirection = newDirection;
        Debug.Log("Time to wait : " + (timeStep * (length - 1)));
        Debug.Log(Time.time);
        yield return new WaitForSeconds(timeStep * (length - 1));
        Debug.Log(Time.time);
        Vector3 oldPos = bodies[length - 1].position;
        GameObject newBody = Instantiate(bodyPrefab, oldPos + new Vector3(step * currentDirection.x, step * currentDirection.y, 0), Quaternion.identity);
        newBody.transform.parent = this.transform;
        newBody.transform.SetAsLastSibling();
        length++;
        bodies = GetComponentsInChildren<Transform>();
    }
}
