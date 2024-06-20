using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class APIManager : MonoBehaviour
{

    private static APIManager monoBehaviour;

    private const string API_URL = "https://geometric-farm-api.vercel.app"; //"http://localhost:3000"; 


    private static void Init()
    {
        if (monoBehaviour == null)
        {
            GameObject obj = new GameObject("APIManager");
            monoBehaviour = obj.AddComponent<APIManager>();
        }
    }

    #region REQUESTS


    private class CreateStudentBody
    {
        public int listNumber;
        public string group;
    }


    public static void VerifyStudent(int listNum, string group, Action onSuccess = null, Action onError = null)
    {
        Init();

        IEnumerator Request()
        {
            string URL = $"{API_URL}/student/verify";


            var body = new CreateStudentBody()
            {
                group = group,
                listNumber = listNum
            };

            UnityWebRequest webRequest = UnityWebRequest.Post(URL, JsonUtility.ToJson(body), "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                if (onError != null)
                    onError();

                Debug.LogError("Error no student found");
                Debug.LogError(webRequest.error);
            }
        }


        monoBehaviour.StartCoroutine(Request());

    }


    private class CreateLevel1StatBody
    {
        public int studentListNum;
        public string studentGroup;
        public string selectedShape;
        public string expectedShape;
        public bool isCorrect;
    }


    public static void CreateLevel1Stat(bool isCorrect, string expectedShape, string selectedShape, Action onSuccess = null, Action onError = null)
    {
        Init();

        IEnumerator Request()
        {
            string URL = $"{API_URL}/stats/level1";


            var body = new CreateLevel1StatBody
            {
                studentListNum = SettingsManager.studentListNum,
                studentGroup = SettingsManager.studentGroup,
                expectedShape = expectedShape,
                isCorrect = isCorrect,
                selectedShape = selectedShape
            };

            UnityWebRequest webRequest = UnityWebRequest.Post(URL, JsonUtility.ToJson(body), "application/json");

            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                if (onError != null)
                    onError();

                Debug.LogError("Error POST level1 stat ");
                Debug.LogError(webRequest.error);
            }


        }

        monoBehaviour.StartCoroutine(Request());
    }


    private class CreateLevel2StatBody
    {
        public int studentListNum;
        public string studentGroup;
        public string operationType;
        public bool isCorrect;

    }


    public static void CreateLevel2Stat(bool isCorrect, string operationType, Action onSuccess = null, Action onError = null)
    {
        Init();

        IEnumerator Request()
        {
            string URL = $"{API_URL}/stats/level2";


            var body = new CreateLevel2StatBody
            {
                studentListNum = SettingsManager.studentListNum,
                studentGroup = SettingsManager.studentGroup,
                isCorrect = isCorrect,
                operationType = operationType,
            };

            UnityWebRequest webRequest = UnityWebRequest.Post(URL, JsonUtility.ToJson(body), "application/json");

            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                if (onError != null)
                    onError();

                Debug.LogError("Error POST level2 stat ");
                Debug.LogError(webRequest.error);
            }

        }

        monoBehaviour.StartCoroutine(Request());
    }



    private class CreateLevel3StatBody
    {
        public int studentListNum;
        public string studentGroup;
        public string operationType;
        public string shapeType;
        public bool isCorrect;
    }


    public static void CreateLevel3Stat(bool isCorrect, string operationType, string shapeType, Action onSuccess = null, Action onError = null)
    {
        Init();

        IEnumerator Request()
        {
            string URL = $"{API_URL}/stats/level3";


            var body = new CreateLevel3StatBody
            {
                studentListNum = SettingsManager.studentListNum,
                studentGroup = SettingsManager.studentGroup,
                isCorrect = isCorrect,
                operationType = operationType,
                shapeType = shapeType,

            };

            UnityWebRequest webRequest = UnityWebRequest.Post(URL, JsonUtility.ToJson(body), "application/json");

            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                if (onError != null)
                    onError();

                Debug.LogError("Error POST level3 stat ");
                Debug.LogError(webRequest.error);
            }

        }

        monoBehaviour.StartCoroutine(Request());
    }




    private class CreateLevelScoreBody
    {
        public int studentListNum;
        public string studentGroup;
        public int score;
        public int level;
    }
    public static void CreateLevelScore(int score, int level, Action onSuccess = null)
    {
        Init();

        IEnumerator Request()
        {
            string URL = $"{API_URL}/scores";


            var body = new CreateLevelScoreBody
            {
                studentListNum = SettingsManager.studentListNum,
                studentGroup = SettingsManager.studentGroup,
                level = level,
                score = score,
            };

            UnityWebRequest webRequest = UnityWebRequest.Post(URL, JsonUtility.ToJson(body), "application/json");

            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                Debug.LogError("Error POST level score stat ");
                Debug.LogError(webRequest.error);
            }
        }

        monoBehaviour.StartCoroutine(Request());
    }



    #endregion
}
