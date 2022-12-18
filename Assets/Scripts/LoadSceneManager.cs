using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static string nextScene; //�̵��� ��
    public Image progressBar; //�ε������̹���
    private void Start()
    {
        StartCoroutine(LoadScene()); //�ε�����
    }
    //�ε��ϰ��� �ϴ� ���� �ε��Ҷ� �ε��ð��� �ε����� ������
    //�ٸ� ��ũ��Ʈ������ ����� �� �ְ���
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene"); // �ε��� �ҷ�����
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); //�ε��� ��������
        op.allowSceneActivation = false;
        float timer = 0.0f;

        //�ε��� ������ ����
        while (!op.isDone)
        {
            yield return null;  // ȭ���� �ε��� ����
            timer += Time.deltaTime; 

            // 90%���� ����ɶ����� �ε����൵�� ���� ����ٸ� ä��
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); //����� ä���
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            // ������ 10%�� 1�ʿ� ���ļ� ä��� �� �ε�
            else
            {
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); //������ 10% ä���
                // �ε� ���� �ٰ� �� ä������ nextScene�� �ҷ���
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }
}
