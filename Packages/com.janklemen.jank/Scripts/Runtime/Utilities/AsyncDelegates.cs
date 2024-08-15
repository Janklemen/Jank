using Cysharp.Threading.Tasks;

namespace Jank.Utilities
{
    public delegate UniTask ActionAsync();
    public delegate UniTask ActionAsync<T1>(T1 t1);
    public delegate UniTask ActionAsync<T1, T2>(T1 t1, T2 t2);
    public delegate UniTask ActionAsync<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
    public delegate UniTask ActionAsync<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
    
    public delegate UniTask<TOut> FuncAsync<TOut>();
    public delegate UniTask<TOut> FuncAsync<T1, TOut>(T1 t1);
    public delegate UniTask<TOut> FuncAsync<T1, T2, TOut>(T1 t1, T2 t2);
    public delegate UniTask<TOut> FuncAsync<T1, T2, T3, TOut>(T1 t1, T2 t2, T3 t3);
    public delegate UniTask<TOut> FuncAsync<T1, T2, T3, T4, TOut>(T1 t1, T2 t2, T3 t3, T4 t4);
}