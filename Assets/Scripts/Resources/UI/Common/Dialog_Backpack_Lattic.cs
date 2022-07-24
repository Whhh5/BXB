using System.Threading.Tasks;
using UnityEngine;
using BXB.Core;
using UnityEngine.Events;

public class Dialog_Backpack_Lattic : MiObjPoolPublicParameter
{
    [SerializeField, ReadOnly] GameObject parent;
    [SerializeField, ReadOnly] UICommon_ArticleFrame article;
    [SerializeField] RectTransform main;
    [SerializeField] MiUIButton button;


    [HideInInspector] public UnityEvent mouseDown = new UnityEvent();
    [HideInInspector] public UnityEvent mouseUp = new UnityEvent();
    public MiUIButton click => button;
    public async Task SetUp(GameObject parent)
    {
        await AsyncDefaule();

        this.parent = parent;
    }
    public async Task<bool> Put(RectTransform rect)
    {
        if (main == null)
        {
            Log(color: Color.black, $"Please Set Lattic Parameter : Main");
            return false;
        }
        if (article != null)
        {
            Log(color: Color.black, $"article is not none");
            return false;
        }
        rect.Normalization(main);
        article = rect.GetComponent<UICommon_ArticleFrame>();

        await AsyncDefaule();
        return true;
    }
    public async Task<UICommon_ArticleFrame> TakeOut()
    {
        await AsyncDefaule();

        var art = article.Clone() as UICommon_ArticleFrame;
        article = null;
        return art;
    }
    public async Task<UICommon_ArticleFrame> GetArticle()
    {
        await AsyncDefaule();
        return article;
    }

    public async new Task Destroy()
    {
        if (article != null)
        {
            article.Destroy();
            article = null;
        }
        await ObjPool.Repulace(parent, gameObject);
        gameObject.SetActive(false);
    }
}
