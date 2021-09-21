using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class CandidateControl : MonoBehaviour
{
    #region Arrays
    [SerializeField]
    private static string[] _names= {"John","Josh","Neil","Tom","Tommy","Sam","Thomas","Carter","Max","Luis","Samuel"};
    [SerializeField]
    private static string[] _departments = {"Developer", "Artist", "Analyst"};
    #endregion

    #region Fields
    Animator animator;
    NavMeshAgent agent;

    private static Text nameText, salaryText, departmentText, experienceText;

    private static int _salaryLimit=10, _experienceLimit = 10;
    private static PlayerControl playerControl;

    private static Transform sittingPos, door;
    private static Vector3 chairPos, doorPos;
    private string _candidateName, _department;

    private int _experience, _salary;
    private bool _goToChair = false, _goToDoor = false;
    #endregion

    #region Properties
    public string candidateName => _candidateName;
    public string department => _department;

    public int experience => _experience;
    public int salary => _salary;
    public int salaryLimit => _salaryLimit;
    public int experienceLimit => _experienceLimit;

    public string[] departments => _departments;
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
        GetReferences();
        SetCandidate›nfos();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        chairPos = new Vector3(sittingPos.position.x, transform.position.y, sittingPos.position.z);
        doorPos= new Vector3(door.position.x, transform.position.y, door.position.z);
    }

    void GetReferences()
    {
        nameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<Text>();
        salaryText = GameObject.FindGameObjectWithTag("SalaryText").GetComponent<Text>();
        departmentText = GameObject.FindGameObjectWithTag("DepartmentText").GetComponent<Text>();
        experienceText = GameObject.FindGameObjectWithTag("ExperienceText").GetComponent<Text>();

        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        sittingPos = GameObject.FindGameObjectWithTag("Player").transform;
        door = GameObject.FindGameObjectWithTag("Door").transform;
    }

    void SetCandidate›nfos()
    {

        _candidateName = _names[Random.Range(0, _names.Length)];
        _salary = Random.Range(3, 15) * 1000;
        _department = _departments[Random.Range(0, _departments.Length)];
        _experience = Random.Range(0, 20);
    }

    public void GoToChair()
    {
        agent.enabled = true;
        agent.SetDestination(chairPos);
        animator.SetBool("Walk", true);

    }

    public void GoToDoor()
    {
        agent.enabled = true;
        agent.SetDestination(doorPos);
        playerControl.ChangeCandidate();
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
    void ShowCandidatePanel() //Called from sitting animation event.
    {
        nameText.text = "Name : " + _candidateName;
        salaryText.text = "Salary : " + _salary.ToString() + "Ä";
        departmentText.text = "Department : " + _department;
        experienceText.text = "Experience : " + _experience.ToString() + " years";

        salaryText.transform.parent.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }
    void HideCandidatePanel() //Called from clapping and disbelief animation events.
    {
        salaryText.transform.parent.gameObject.GetComponent<CanvasGroup>().DOFade(0,0.5f);
        
    }

}
