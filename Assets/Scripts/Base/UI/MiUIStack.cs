using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BXB
{
    namespace Core
    {
        [System.Serializable]
        public sealed class MiUIStack<T> : MiBaseClass where T : Object
        {
            [SerializeField] List<T> pool;
            [SerializeField] int index = -1;
            public MiUIStack()
            {
                pool = new List<T>(new T[5]);
            }
            public async Task Push(T obj)
            {
                await MiAsyncManager.Instance.Default();
                if (index + 1 >= pool.Count)
                {
                    Log(color: Color.black, $"Data Stack - Count None Interspace - Added five Interspace  ! ! ! ");
                    pool.AddRange(new T[5]);
                }

                if (index >= 0 && pool[index].name == obj.name)
                    return;
                if (pool[++index] != obj)
                {
                    pool[index] = obj;
                    for (int i = index + 1; i < pool.Count; i++)
                        if (pool[i] != null)
                            pool[i] = null;
                        else
                            break;
                }

                string str = "";
                foreach (var parameter in pool)
                {
                    if (parameter != null)
                    {
                        str += parameter.GetType() + " - ";
                    }
                }
                Log(color: Color.black, str);
            }
            public async Task<T> Previous()
            {
                await MiAsyncManager.Instance.Default();
                if (index > 0)
                    return pool[--index];
                Log(color: Color.black, $"Data Stack - Already Is Start One ! ! ! ");
                return null;
            }
            public async Task<T> Next()
            {
                await MiAsyncManager.Instance.Default();
                if (index + 1 < pool.Count && pool[index + 1] != null)
                    return pool[++index];
                Log(color: Color.black, $"Data Stack - Already Is End One ! ! ! ");
                return null;
            }
            public async Task<T> Remove(T obj)
            {
                await MiAsyncManager.Instance.Default();
                if (pool.Contains(obj))
                {
                    int k = 0;
                    for (int i = 0; i < pool.Count; i++)
                    {
                        if (pool[i] == obj)
                        {
                            k = i;
                            break;
                        }
                    }

                    for (int i = k; i < pool.Count; i++)
                    {
                        if (i + 1 < pool.Count && pool[i+1] != null)
                        {
                            pool[i] = pool[i + 1];
                            pool[i + 1] = null;
                        }
                    }
                }
                return obj;
            }

            public async Task Clear()
            {
                pool.Clear();
            }
        }
    }
}
