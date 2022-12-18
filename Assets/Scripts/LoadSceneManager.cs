using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static string nextScene; //이동될 씬
    public Image progressBar; //로딩진행이미지
    private void Start()
    {
        StartCoroutine(LoadScene()); //로딩실행
    }
    //로드하고자 하는 씬을 로드할때 로딩시간에 로딩씬을 보여줌
    //다른 스크립트에서도 사용할 수 있게함
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene"); // 로딩씬 불러오기
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //로딩의 진행정도
        op.allowSceneActivation = false;
        float timer = 0.0f;

        //로딩이 끝날때 까지
        while (!op.isDone)
        {
            yield return null;  // 화면의 로딩바 진행
            timer += Time.deltaTime; 

            // 90%까지 진행될때까지 로딩진행도에 따라서 진행바를 채움
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); //진행바 채우기
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            // 나머지 10%를 1초에 거쳐서 채운뒤 씬 로드
            else
            {
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); //나머지 10% 채우기
                // 로딩 진행 바가 다 채워지면 nextScene을 불러옴
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }
}
