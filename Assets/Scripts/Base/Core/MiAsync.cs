using System;
using System.Threading.Tasks;

namespace BXB
{
    namespace Core
    {
        public class MiAsyncManager : Core.MiSingleton<MiAsyncManager>
        {
            #region  StartAsync
            public void StartAsync(Func<Task> func)
            {
                func.Invoke();
            }
            public void StartAsync<T0>(T0 t0, Func<T0,Task> func)
            {
                func.Invoke(t0);
            }
            public void StartAsync<T0, T1>(T0 t0, T1 t1, Func<T0, T1, Task> func)
            {
                func.Invoke(t0, t1);
            }
            public void StartAsync<T0, T1, T2>(T0 t0, T1 t1, T2 t2, Func<T0, T1, T2, Task> func)
            {
                func.Invoke(t0, t1, t2);
            }
            public void StartAsync<T0, T1, T2, T3>(T0 t0, T1 t1, T2 t2, T3 t3, Func<T0, T1, T2, T3, Task> func)
            {
                func.Invoke(t0, t1, t2, t3);
            }
            #endregion

            public async Task Default()
            {
                await Task.Delay(TimeSpan.FromSeconds(0));
            }
        }
    }
}