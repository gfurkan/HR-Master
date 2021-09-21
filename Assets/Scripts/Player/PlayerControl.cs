using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject candidatePrefab;
    [SerializeField]
    private int candidateCount;
    [SerializeField]
    private Text salaryExpectationText, departmentExpectationText, experienceExpectationText, totalCandidatesText, trueDecisionsText, falseDecisionText;
    [SerializeField]
    private GameObject levelFinishPanel;
    [SerializeField]
    private Button startGameButton;

    private List<GameObject> candidates = new List<GameObject>();
    private int candidateNumber = 0, trueDecisions = 0, falseDecisions = 0, salaryExpectation, experienceExpectation;
    private string departmentExpectation;
    private float candidateZPos = -2;

    InputManager inputManager;
    CandidateControl candidateControl;

    private bool canDrag = false;

    #endregion

    void Start()
    {
        inputManager = InputManager.Instance;
        for(int i=0;i< candidateCount; i++)
        {
            var cand = Instantiate(candidatePrefab, new Vector3(-3f, 0, candidateZPos),Quaternion.Euler(new Vector3(0,180,0)), transform.root);
            candidateZPos += 2;
            candidates.Add(cand);
        }

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

    public void ChangeCandidate()
    {
        if (candidateNumber+1 != candidateCount) 
        {
            candidateNumber++;
            candidateControl = candidates[candidateNumber].GetComponent<CandidateControl>();
            candidateControl.GoToChair();
        }
        else
        {
            LevelFinish();
            LevelManager.Instance.LevelCompleted();
        }
    }

    void MakeElection()
    {

        if (inputManager.directionVec.x > 0)
        {
            candidateControl.ElectionResult(false);
            canDrag = false;

            if (candidateControl.salary <= salaryExpectation && candidateControl.department == departmentExpectation && candidateControl.experience >= experienceExpectation)
            {
                trueDecisions++;
            }
            else
            {
                falseDecisions++;
            }
        }
        if (inputManager.directionVec.x < 0)
        {
            candidateControl.ElectionResult(true);
            canDrag = false;

            if (candidateControl.salary > salaryExpectation || candidateControl.department != departmentExpectation || candidateControl.experience < experienceExpectation)
            {
                trueDecisions++;
            }
            else
            {
                falseDecisions++;
            }
        }
    }

    IEnumerator CanDrag()
    {
        yield return new WaitForSeconds(1);
        canDrag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Candidate")
         {
            other.transform.rotation = Quaternion.Euler(0,180,0);

            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<Animator>().SetBool("Sit", true);

            StartCoroutine("CanDrag");
            other.GetComponent<Collider>().enabled = false;
         }
    }
    void SetExpectations()
    {
        salaryExpectation = Random.Range(5, candidateControl.salaryLimit)*1000;
        experienceExpectation = Random.Range(0, candidateControl.experienceLimit);
        departmentExpectation = candidateControl.departments[Random.Range(0, candidateControl.departments.Length)];


        salaryExpectationText.text = "Salary <= " + salaryExpectation + "€";
        departmentExpectationText.text = "Department : " + departmentExpectation;
        experienceExpectationText.text = "Experience => " + experienceExpectation + " yrs.";
    }
    void LevelFinish()
    {
        totalCandidatesText.text = "Total Candidates : " + candidateCount;
        trueDecisionsText.text = "True Decisions : " + trueDecisions;
        falseDecisionText.text = "False Decisions : " + falseDecisions;

        levelFinishPanel.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }

    public void StartGameButton()
    {
        candidateControl = candidates[candidateNumber].GetComponent<CandidateControl>();
        candidateControl.GoToChair();
        SetExpectations();
        startGameButton.GetComponent<CanvasGroup>().DOFade(0, 0.25f);
    }
}
