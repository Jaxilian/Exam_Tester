using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question_Manager : MonoBehaviour
{
  
    public List<Test> tests;



    public int[] answers;
    public int[] facit;

    int currentQuestionInt = 0;
    int currentTestInt = 0;



 

    // SETS
    public void SetUserAnswer(int answer)
    {
        GetCurrentQuestion().userAnswer = answer;
    }
    public void IncrementQuestion()
    {
        if (currentQuestionInt < GetAmountQuestions() - 1)
        {
            currentQuestionInt++;
        }
        else
        {
            GetComponent<UI_Manager>().GenerateEndStats();
            Debug.Log("Show stats");
            
        }
    }
    public void DecrementQuestion()
    {
        if (currentQuestionInt > 0)
        {
            currentQuestionInt--;
        }
        else
        {
            GetComponent<UI_Manager>().ClearForms("AddQuestion");
            GetComponent<UI_Manager>().OpenWindow(GetComponent<UI_Manager>().addQuestionWindow);
        }
    }
    public void SetCurrentQuestion(int value)
    {
        currentQuestionInt = value;
    }
    public void SetCurrentTest(int value)
    {
        currentTestInt = value;
    }


    // GETS

    public int GetCurrentTestInt()
    {
        return currentTestInt;
    }
    public Test GetCurrentTest()
    {
        return tests[currentTestInt];
    }
    public int GetUserAnswer()
    {
        return (GetCurrentQuestion().userAnswer);
    }
    public Question GetCurrentQuestion()
    {
        return tests[currentTestInt].questions[currentQuestionInt];
    }
    public int GetCurrentQuestionInt()
    {
        return currentQuestionInt;
    }
    public int GetAmountQuestions()
    {
        return tests[GetCurrentTestInt()].questions.Count;
    }
    public int GetAmountOfAnswers()
    {
        return GetCurrentQuestion().answers.Length;
    }
    public string[] GetAnswers()
    {
        return GetCurrentQuestion().answers;
    }
    public int GetResult()
    {
        facit = new int[GetAmountQuestions()];
        answers = new int[GetAmountQuestions()];

        int ii = 0;
        foreach(Question question in tests[currentTestInt].questions)
        {
            answers[ii] = question.userAnswer;
            facit[ii] = question.rightAnswer;
            ii++;
        }

        int rightAnswers = 0;

        for(int i = 0; i < GetAmountQuestions(); i++)
        {
            if(answers[i] == facit[i])
            {
                rightAnswers++;
            }
        }

        return rightAnswers;
    }
    public void AddQuestion(string question, string[] answers, int rightAnswer)
    {
        foreach(Question i in GetCurrentTest().questions)
        {
            if(i.question == question)
            {
                GetComponent<UI_Manager>().Error("That question already exists!");
                Debug.Log("That question already exists!");
                return;
            }
        }

        Question questionToAdd = new Question()
        {
            question = question,
            answers = answers,
            rightAnswer = rightAnswer
        };

        tests[currentTestInt].questions.Add(questionToAdd);
    }
    public void DeleteQuestion(int index)
    {
        tests[currentTestInt].questions.RemoveAt(index);
    }
    public void AddTest(string _name, string _author)
    {
        Test testToAdd = new Test()
        {
            name = _name,
            author = _author,
            questions = new List<Question>()
        };

        tests.Add(testToAdd);
    }
    public void DeleteTest(int index)
    {
        tests.RemoveAt(index);
    }

    public void ShuffleAll()
    {

        //Copy the list to shuffle
        List<Question> copy = GetCurrentTest().questions.ToList(); //
        List<Question> list = GetCurrentTest().questions; //

        //for every item in the list, replace it with a random item in the copied list and then remove it
        for (int item = 0; item < list.Count; item++)
        {
            int randomItem = (int)Random.Range(0, copy.Count);

            list[item] = copy[randomItem];


            string rightAnswer = list[item].answers[list[item].rightAnswer];

            list[item].answers = OrderIn.Shuffle(list[item].answers);

            int i = 0;
            foreach(string answer in list[item].answers)
            {
                if(answer == rightAnswer)
                {
                    list[item].rightAnswer = i;
                    break;
                }
                else
                {
                    i++;
                }
            }



            copy.RemoveAt(randomItem);

        }

        Debug.Log(list.Count);

    }
}


[System.Serializable]
public class Test
{
    public string name;
    public string author;
    public string language;
    public List<Question> questions;

}

[System.Serializable]
public class Question
{
    public string question;
    public string[] answers;
    public int userAnswer;
    public int rightAnswer;


}

