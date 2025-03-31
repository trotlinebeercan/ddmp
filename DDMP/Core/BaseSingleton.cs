namespace ddmp;

public abstract class BaseSingleton<T> where T : BaseSingleton<T>, new()
{
    private static readonly System.Lazy<T> _arbiter = new System.Lazy<T>(() => new T());
    public static T Instance => _arbiter.Value;
}
