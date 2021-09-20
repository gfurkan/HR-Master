using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CandidateControl : MonoBehaviour
{
    #region Arrays
    [SerializeField]
    private static string[] names= {"John","Josh","Neil","Tom","Tommy","Sam","Thomas","Carter","Max","Luis","Samuel"};
    [SerializeField]
    private static string[] departments = { "IT", "Developer", "Artist", "Analyst", "Growth Manager", "CEO", "CTO",};
    #endregion

    #region Fields
    Animator animator;
    NavMeshAgent agent;

    private static Transform chair, door;
    private static Vector3 chairPos, doorPos;
    private string _name, _department;

    private int _experience, _salary;
    private bool _goToChair = false, _goToDoor = false;
    #endregion

    #region Properties
    public string name => _name;
    public string department => _department;

    public int experience => _experience;
    public int salary => _salary;

    public bool goToChair
    {
        get
        {
            return _goToChair;
        }
        set
        {
            _goToChair = value;
        }
    }
    public bool goToDoor
    {
        get
        {
            return _goToDoor;
        }
        set
        {
            _goToDoor = value;
        }
    }
    #endregion

    void Start()
    {
        chair = GameObject.FindGameObjectWithTag("ChairPos").transform;
        door = GameObject.FindGameObjectWithTag("Door").transform;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        _name = names[Random.Range(0, names.Length)];
        _salary = Random.Range(3, 15)*1000;
        _department=departments[Random.Range(0, departments.Length)];
        _experience = Random.Range(0, 20);

        chairPos = new Vector3(chair.position.x, transform.position.y, chair.position.z);
        doorPos= new Vector3(door.position.x, transform.position.y, door.position.z);
    }

    private void Update()
    {
        if (_goToChair)
        {
            GoToChair();
        }
        if (_goToDoor)
        {
            GoToDoor();
        }
    }
    void GoToChair()
    {
        agent.enabled = true;
        agent.SetDestination(chairPos);
        animator.SetBool("Walk", true);

    }
    void GoToDoor()
    {
        agent.enabled = true;
        animator.SetBool("Walk", true);
        agent.SetDestination(doorPos);
    }
    public void ElectionResult (bool eliminated)
    {
        if (eliminated)
        {
            animator.SetBool("Sad", true);
            animator.SetBool("Walk", true);
        }
        if (!eliminated)
        {
            animator.SetBool("Happy", true);
            animator.SetBool("Walk", true);
        }
    }
}
