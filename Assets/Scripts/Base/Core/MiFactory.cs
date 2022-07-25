using System.Threading.Tasks;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        public class MiFactory : MiSingleton<MiFactory>
        {
            public async Task<GameObject> InstantiateAsync(object obj,RectTransform rectTr = null,Transform trTr = null)
            {
                await Task.Delay(System.TimeSpan.Zero);
                GameObject gb = (GameObject)obj;
                if (rectTr != null)
                {
                    gb = GameObject.Instantiate(gb, rectTr);
                    var rect = gb.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                    rect.localScale = Vector3.one;
                    rect.localRotation = Quaternion.Euler(Vector3.zero);
                    rect.pivot = Vector2.one / 2;
                }
                else if (trTr != null)
                {
                    Log(color: Color.black, $"{gb.name}   {trTr.name}");
                    gb = GameObject.Instantiate(gb, trTr);
                    var tr = gb.GetComponent<Transform>();
                    tr.localPosition = Vector3.zero;
                    tr.localRotation = Quaternion.Euler(Vector3.zero);
                    tr.localScale = Vector3.one;
                }
                else
                {
                    gb = GameObject.Instantiate(gb, rectTr);
                    var tr = gb.GetComponent<RectTransform>();
                    tr.localPosition = Vector3.zero;
                    tr.localRotation = Quaternion.Euler(Vector3.zero);
                    tr.localScale = Vector3.one;
                }
                gb.SetActive(false);
                return gb;
            }

            public GameObject Instantiate(object obj, RectTransform rectTr = null, Transform trTr = null)
            {
                GameObject gb = obj as GameObject;
                //Log(Color.green, $"{gb != null}");
                if (rectTr != null)
                {
                    gb = GameObject.Instantiate(gb, trTr);
                    var rect = gb.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                    rect.localScale = Vector3.one;
                    rect.localRotation = Quaternion.Euler(Vector3.zero);
                    rect.pivot = Vector2.one / 2;
                }
                else if (trTr != null)
                {
                    gb = GameObject.Instantiate(gb, rectTr);
                    var tr = gb.GetComponent<Transform>();
                    tr.localPosition = Vector3.zero;
                    tr.localRotation = Quaternion.Euler(Vector3.zero);
                    tr.localScale = Vector3.one;
                }
                else
                {
                    gb = GameObject.Instantiate(gb, trTr);
                    var tr = gb.GetComponent<Transform>();
                    tr.localPosition = Vector3.zero;
                    tr.localRotation = Quaternion.Euler(Vector3.zero);
                    tr.localScale = Vector3.one;
                }
                gb.SetActive(false);
                return gb;
            }
        }
    }
}