using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public GameObject addQuestionWindow;

    public GameObject error, loading;
    [SerializeField] GameObject stats, returnToStats;
    [SerializeField] GameObject box;
    [SerializeField] GameObject boxGrid;
    GameObject[] boxes;

    [SerializeField] Text title, question, footer;
    [SerializeField] Text[] answers;
    [SerializeField] Text[] checks;

    [SerializeField] InputField questionField, answerField0, answerField1, answerField2, answerField3;

    InputField[] tabable;
    int selectedInput = 0;

    Question_Manager qM;

    public string version;



    private void Start()
    {
        qM = GetComponent<Question_Manager>();

        qM.ShuffleAll();
        UpdateQuestion();
        UpdateCheckBox();

        footer.text = "@Wildsoft.se 2019 / Build: " + version;

        tabable = new InputField[5]
        {
            questionField,
            answerField0,
            answerField1,
            answerField2,
            answerField3    
        };

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            selectedInput++;
            if(selectedInput > 4)
            {
                selectedInput = 0;
            }

            tabable[selectedInput].ActivateInputField();

        }
    }
    public void JumpToQuestion(int value)
    {
        Debug.Log(value);
        qM.SetCurrentQuestion(value);
        RefreshQuestion();
    }
    public void IncrementQuestion()
    {
        qM.IncrementQuestion();
        UpdateCheckBox();
        UpdateQuestion();
    }
    public void DecrementQuestion()
    {
        qM.DecrementQuestion();
        UpdateCheckBox();
        UpdateQuestion();
    }
    public void RefreshQuestion()
    {
        UpdateCheckBox();
        UpdateQuestion();
    }
    public void UpdateCheckBox()
    {
        foreach (Text check in checks)
        {
            check.text = "";
        }
        checks[qM.GetUserAnswer()].text = "X";
    }
    public void DeleteQuestion()
    {
        qM.DeleteQuestion(qM.GetCurrentQuestionInt());
        RefreshQuestion();
    }
    public void GenerateEndStats()
    {
        returnToStats.SetActive(true);

        float percent = (float)qM.GetResult() / (float)qM.GetAmountQuestions();
        if (boxes != null)
        {
            foreach (GameObject box in boxes)
            {
                Destroy(box);
            }
            boxes = null;
        }


        qM.GetResult();

        string succ;
        if(qM.GetResult() / qM.GetAmountQuestions() >= 0.6)
        {
            succ = "You are approved!";
        }
        else
        {
            succ = "You are not approved!";
        }

        stats.transform.GetChild(2).GetComponent<Text>().text = "You had " + qM.GetResult() + " correct answers" +
            " of " + qM.GetAmountQuestions() + " (" + (int)(percent * 100) + "%" + ")" + "\nCollage exam points: " + (qM.GetResult() - qM.GetAmountQuestions()) +
            "\n" + succ;

        OpenWindow(stats);
        boxes = new GameObject[qM.GetAmountQuestions()];

        for (int i = 0; i < qM.GetAmountQuestions(); i++)
        {
            boxes[i] = Instantiate(box, boxGrid.transform);
            boxes[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            if (qM.answers[i] == qM.facit[i])
            {
                boxes[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            }
            else
            {
                boxes[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
            boxes[i].AddComponent<EndQuestion>();
            boxes[i].GetComponent<EndQuestion>().ui = this;
            boxes[i].GetComponent<EndQuestion>().question = i;
            boxes[i].GetComponent<EndQuestion>().qm = qM;


        }





    }
   private void UpdateQuestion()
    {
        question.text = qM.GetCurrentQuestion().question;
        title.text = (qM.GetCurrentQuestionInt()+1) + "/" + qM.GetAmountQuestions();
        for (int i = 0; i < qM.GetAmountOfAnswers(); i++)
        {
            answers[i].text = qM.GetAnswers()[i];
        }
       
    }
    public void CloseWindow(GameObject window)
    {

        window.SetActive(false);
    }
    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }
    public void ClearForms(string form)
    {
        switch (form)
        {
            case "AddQuestion":
                answerField0.text = "";
                answerField1.text = "";
                answerField2.text = "";
                answerField3.text = "";
                questionField.text = "";
                break;

            default:
                Debug.Log("There is no form with the name " + form);
                break;
        }


    }
    public void AddQuestion()
    {


        string[] answersToAdd = new string[4]
        {
            answerField0.text,
            answerField1.text,
            answerField2.text,
            answerField3.text
        };

        answersToAdd = OrderIn.Shuffle<string>(answersToAdd);

        int i = 0;
        foreach(string answer in answersToAdd)
        {
            if(answer == answerField0.text)
            {
                break;
            }
            else
            {
                i++;
            }
        }
        qM.AddQuestion(questionField.text,answersToAdd, i);

    }
    public void Error(string message)
    {
        error.SetActive(true);
        error.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Error : \n" + message;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


}


public static class OrderIn
{
    public static T[] Shuffle<T>(T[] listToShuffle)
    {
        List<T> copy = listToShuffle.ToList<T>();

        for (int item = 0; item < listToShuffle.Length; item++)
        {
            int randomItem = (int)Random.Range(0, copy.Count);

            listToShuffle[item] = copy[randomItem];

            copy.RemoveAt(randomItem);

        }

        return listToShuffle;
    }

    public static Object[] ShuffleObjects<Object>(Object[] objectsToShuffle)
    {
        List<Object> copy = objectsToShuffle.ToList<Object>();

        for (int item = 0; item < objectsToShuffle.Length; item++)
        {
            int randomItem = (int)Random.Range(0, copy.Count);

            objectsToShuffle[item] = copy[randomItem];

            copy.RemoveAt(randomItem);

        }

        return objectsToShuffle;
    }
}

