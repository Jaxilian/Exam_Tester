using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    string version = "0.1.0";
    Data data = new Data();
    [SerializeField] Question_Manager qm;
    [SerializeField] UI_Manager ui;

    private void Awake()
    {
        Debug.Log(Application.persistentDataPath + "/data.db");
        if (File.Exists(Application.persistentDataPath + "/data.db"))
        {
            Load();
        }
        else
        {
            Save();
        }

        ui.version = version;
    }


    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/data.db", FileMode.Open);
        data = (Data)bf.Deserialize(file);
        file.Close();

        UpdateData();


    }

    public void Save()
    {
        data.currentQuestion = qm.GetCurrentQuestionInt();
        data.tests = qm.tests;
        data.currentTest = qm.GetCurrentTestInt();
        data.version = version;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/data.db");
        bf.Serialize(file, data);
        file.Close();
    }


    public void CheckTests(Data oldData, Data newData)
    {

        foreach (Test newTest in newData.tests)
        {
            foreach (Test oldTest in oldData.tests)
            {
                if (newTest.name == oldTest.name)
                {
                    
                    break;
                }
            }

        }
    }

    public void UpdateData()
    {
        Data dataToAdd = new Data();

        Debug.Log(version + "  /  " + data.version);
        if (version != data.version)
        {
            Debug.Log("Update Required");

            dataToAdd.tests = qm.tests;
            dataToAdd.currentTest = qm.GetCurrentTestInt();
            dataToAdd.currentQuestion = qm.GetCurrentQuestionInt();

            foreach (Test testToAdd in dataToAdd.tests)
            {
                bool testExists = false;

                foreach (Test test in data.tests)
                {
                    if (test.name == testToAdd.name)
                    {
                        testExists = true;

                        Debug.Log(testToAdd.name + " does already exist in data\nCheck test's questions");

                        foreach (Question questionToAdd in testToAdd.questions)
                        {
                            bool questionExists = false;


                            foreach (Question question in test.questions)
                            {
                                if(questionToAdd.question == question.question)
                                {
                                    Debug.Log(questionToAdd.question + "does already exist in data");
                                    questionExists = true;
                                    break;
                                }

                            }

                            if (!questionExists)
                            {
                                test.questions.Add(questionToAdd);
                                Debug.Log(questionToAdd + "did NOT exist in data and has been added");
                            }

                        }


                        break;
                    }
                }

                if (!testExists)
                {
                    data.tests.Add(testToAdd);
                    Debug.Log(testToAdd.name + "did NOT exist in data and has been added");
                }


            }
            dataToAdd = null;
            Debug.Log("Update successfull");
            qm.tests = this.data.tests;
            qm.SetCurrentQuestion(this.data.currentQuestion);
            qm.SetCurrentTest(this.data.currentTest);
            version = this.data.version;
            Save();
        }

        else
        {

            Debug.Log("No update required");
            qm.tests = this.data.tests;
            qm.SetCurrentQuestion(this.data.currentQuestion);
            qm.SetCurrentTest(this.data.currentTest);
            version = this.data.version;
        }
    }
}







[System.Serializable]
public class Data
{
    public List<Test> tests;
    public int currentQuestion;
    public int currentTest;
    public string version;
}