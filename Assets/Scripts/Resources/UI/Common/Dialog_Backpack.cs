using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using UnityEngine.Events;
using System;

public class Dialog_Backpack : MiUIDialog
{
    [SerializeField] MiUIButton shopping;
    [SerializeField] MiUIButton onSale;
    [SerializeField] MiUIButton installBtn;

    [Header("GameObject"),
        SerializeField] GameObject article;
    [SerializeField] GameObject lattic;
    [SerializeField] GameObject hint2Primary;


    [Header("RectTransform"),
        SerializeField] RectTransform articleParent;


    [Header("List"),
        SerializeField] List<Dialog_Backpack_Lattic> latticList = new List<Dialog_Backpack_Lattic>();


    [Header("ReadOnly"),
        SerializeField,ReadOnly] bool isArticleMove = false;
    [SerializeField,ReadOnly] RectTransform selectedArticle;
    [SerializeField,ReadOnly] Dialog_Backpack_Lattic articleLattic;
    UnityEvent articleClick = new UnityEvent();
    [SerializeField, ReadOnly] MiUIDialog articleHintDialog;


    [Header("Temp Parameter"),
        SerializeField,ReadOnly] List<RectTransform> hint2Rects = new List<RectTransform>();
    public async Task SetUpShow()
    {
        await InitLattic(10);
        await InitArticle();
        await ShowAsync(DialogMode.none);
    }

    async Task InitLattic(int value)
    {
        foreach (var item in latticList)
        {
            await item.Destroy();
        }
        latticList = new List<Dialog_Backpack_Lattic>();
        for (int i = 0; i < value; i++)
        {
            //string path = "";
            //var o = SkillManager.Instance.character.GetUIElement<LocalizeUIDialogData, GameObject>(path, 1, )



            var obj = await ObjPool.GetObjectAsync(lattic);
            var rect = obj.GetComponent<RectTransform>();
            rect.SetParent(articleParent);
            rect.localScale = Vector3.one;
            rect.rotation = Quaternion.Euler(Vector3.zero);
            rect.anchoredPosition3D = Vector3.zero;
            var cs = obj.GetComponent<Dialog_Backpack_Lattic>();
            await cs.SetUp(lattic);
            obj.name = i.ToString();
            cs.click.AddOnPointerEnterClick(async () =>
            {
                articleClick.RemoveAllListeners();
                articleClick.SubscribeEventAsync(async () =>
                {
                    var item = await cs.GetArticle();
                    if (selectedArticle == null)
                    {
                        selectedArticle = null;
                        return;
                    }
                    if (item == null)
                    {
                        //调换位置
                        await cs.Put(selectedArticle);
                        Log(color: Color.black, $"{cs.name} -> {selectedArticle.name}");
                    }
                    else
                    {
                        await articleLattic.Put((await cs.TakeOut()).GetComponent<RectTransform>());
                        await cs.Put(selectedArticle);
                    }
                    selectedArticle = null;
                });

                foreach (var parameter in hint2Rects)
                {
                    await ObjPool.Repulace(hint2Primary, parameter.gameObject);
                }
                hint2Rects = new List<RectTransform>();
                var aricle = await cs.GetArticle();
                if (aricle != null)
                {
                    var articleId = await aricle.GetItemID();
                    //var data = DataManager.Master.GetTableData<LocalizeArticleData>(articleId);
                    var path = "Prefab/UI/Common";
                    //articleHintDialog = await TableManager.Instance.tableData.ShowUIDialog<LocalizeArticleData, MiUIDialog>(
                    //    path, 10104010002, CanvasLayer.System, data);
                }
            });
            cs.click.AddOnPointerExitClick(async () =>
            {
                if (articleHintDialog != null)
                {
                    articleHintDialog.Destroy();
                    articleHintDialog = null;
                }
            });
            cs.click.AddOnPointerDownClick(async () =>
            {
                var art = await cs.GetArticle();
                if (art != null)
                {
                    var artic = await cs.TakeOut();
                    selectedArticle = artic.GetComponent<RectTransform>();
                    selectedArticle.SetParent(main.GetComponent<RectTransform>());
                    articleLattic = cs;
                    isArticleMove = true;
                }
            });
            cs.click.AddOnPointerUpClick(async () =>
            {
                if (selectedArticle != null)
                {
                    articleClick.Invoke();
                }
                isArticleMove = false;
            });

            cs.click.AddOnPointerClick(async () =>
            {
                
            });

            latticList.Add(cs);
            obj.SetActive(true);
        }
    }

    public async Task InitArticle()
    {
        var characterActicles = MiDataService.GetAccountInfoData().acticles;

        foreach (var item in characterActicles)
        {
            //10104020004
            string path = CommonManager.Instance.filePath.PreUIElementPath;
            Sprite sprite = await ResourceManager.Instance.LoadAsync<Sprite>("Sprite/Icon", item.key.ToString());
            Func<Task<Sprite>> f_sprite = async () => sprite;
            Func<Task<ulong>> f_count = async () => item.value;
            Func<Task<bool>> f_isRaycast = async () => false;
            Func<Task<ulong>> f_itemId = async () => item.key;
///*            var cc = await TableManager.Instance.tableData.GetUIElement<LocalizeUIDialogData, UIElementPoolBase>(path, 10104020004, articleParent, Vector3.zero, f_sprite, */f_count, f_isRaycast, f_itemId);

            //RectTransform rectt = cc.GetComponent<RectTransform>();
            foreach (var parameter in latticList)
            {
                //if (await parameter.Put(rectt))
                    break;
            }

            //var obj = await ObjPool.GetObjectAsync(article);
            //var rect = obj.GetComponent<RectTransform>();

            //foreach (var parameter in latticList)
            //{
            //    if (await parameter.Put(rect))
            //        break;
            //}
            //var cs = obj.GetComponent<UICommon_ArticleFrame>();
            //var sprite = await MiResourcesManager.Instance.LoadAsync<Sprite>("Sprite/Icon", item.key.ToString());
            //await cs.SetUp(article, item.key, sprite, false);
            //obj.SetActive(true);
        }
    }

    private void Update()
    {
        if (isArticleMove && selectedArticle != null)
        {
            var mousePos = Input.mousePosition;
            selectedArticle.anchoredPosition3D = Vector3.Lerp(selectedArticle.anchoredPosition3D,new Vector3(mousePos.x - Camera.main.pixelWidth / 2, mousePos.y - Camera.main.pixelHeight * 0.5f), 10.0f * Time.deltaTime);
        }
    }

    public override void OnInit()
    {

    }

    public override void OnSetInit(object[] value)
    {

    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await InitLattic(10);
        await InitArticle();
        await ShowAsync(DialogMode.none);
    }
}
