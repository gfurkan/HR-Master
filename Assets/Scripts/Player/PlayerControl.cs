using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private GameObject candidatePrefab;
    [SerializeField]
    private int candidateCount;
    [SerializeField]
    private Text salaryText, departmentText, experienceText;

    private List<GameObject> candidates = new List<GameObject>();
    private int candidateNumber = 0;

    private float candidateZPos=-2;
    InputManager inputManager;
    CandidateControl candidateControl;

    private bool canDrag = false;

    void Start()
    {
        inputManager = InputManager.Instance;
        for(int i=0;i< candidateCount; i++)
        {
            var cand = Instantiate(candidatePrefab, new Vector3(-3f, 0, candidateZPos),Quaternion.Euler(new Vector3(0,180,0)), transform.root);
            candidateZPos += 2;
            candidates.Add(cand);
        }
        candidateControl = candidates[candidateNumber].GetComponent<CandidateControl>();
        candidateControl.goToChair = true;
    }

    void Update()
    {
        if (canDrag)
        {
            MakeElection();
        }
        else
        {
            inputManager.directionVec = Vector3.zero;
        }
    }

    void ChangeCandidate()
    {
        if (candidateNumber+1 != candidateCount) 
        {
            candidateNumber++;
            candidateControl = candidates[candidateNumber].GetComponent<CandidateControl>();
            candidateControl.goToChair = true;
        }
        salaryText.text = "Salary : ";
        departmentText.text = "Department : ";
        experienceText.text = "Experience : ";
    }

    void MakeElection()
    {
        if (inputManager.directionVec.x > 0)
        {
            candidateControl.ElectionResult(false);
            float waitingTime = 3f;
            StartCoroutine("GoToDoor",waitingTime);

            canDrag = false;

        }
        if (inputManager.directionVec.x < 0)
        {
            candidateControl.ElectionResult(true);
            float waitingTime = 4f;
            StartCoroutine("GoToDoor", waitingTime);

            canDrag = false;
        }
    }

    IEnumerator GoToDoor(float time)
    {
        yield return new WaitForSeconds(time);
        candidateControl.goToDoor = true;
        ChangeCandidate();
    }

    IEnumerator CanDrag()
    {
        yield return new WaitForSeconds(1);
        salaryText.text ="Salary : "+ candidateControl.salary.ToString() + "€";
        departmentText.text ="Department : " +candidateControl.department;
        experienceText.text ="Experience : "+ candidateControl.experience.ToString() + " years";

        canDrag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Candidate")
         {
            candidateControl.goToChair = false;
            other.transform.rotation = Quaternion.Euler(0,180,0);

            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<Animator>().SetBool("Sit", true);

            StartCoroutine("CanDrag");

            other.GetComponent<Collider>().enabled = false;
         }
    }

}
