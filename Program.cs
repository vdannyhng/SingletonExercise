using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public sealed class Logger
{
    // 1) privater ctor: verhindert "new Logger()" - Somit kann von aussen kein neuer Singleton aufgerufen werden
    private Logger() { }

    // 2) Lazy<T> liefert threadsicheres Lazy-Init out of the box - Nur eine Instanz für alle
    private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());

    // 3) globale Zugriffsstelle
    public static Logger Instance => _instance.Value;

    private int _messageCount;

    // Hier funktioniert dies wie eine Art getter-Methode
    public int MessageCount => _messageCount;

    public void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
        // messageNumber += 1 ist nicht atomar, bei parallelen Logs kann der Zähler eventuell falsche Werte liefern
        // messageNumber += 1;
        Interlocked.Increment(ref _messageCount);
    }
}

public sealed class AppConfig
{
    private AppConfig() { }

    private static readonly Lazy<AppConfig> _instance = new Lazy<AppConfig>(() => new AppConfig());

    public static AppConfig Instance => _instance.Value;

    private readonly ConcurrentDictionary<string, string> _kv = new();
    public void Set(string key, string value) => _kv[key] = value;

    public bool TryGet(string key, out string value) => _kv.TryGetValue(key, out value);

    public void DumpToConsole()
    {
        foreach (var kv in _kv)
            Console.WriteLine($"{kv.Key} = {kv.Value}");
    }
}
class Program
{
    static async Task Main()
    {
        // Zwei parallele Zugriffe - Zeigt Threadsicherheit
        // Es wird immer die gleiche Instanz aufgerufen
        var t1 = Task.Run(() => Logger.Instance.Log("Log aus Task 1"));
        var t2 = Task.Run(() => Logger.Instance.Log("Log aus Task 2"));
        await Task.WhenAll(t1, t2);

        var t3 = Task.Run(() => AppConfig.Instance.Set("Hi", "Danny"));
        var t4 = Task.Run(() => AppConfig.Instance.Set("Hii", "Darien"));
        await Task.WhenAll(t3, t4);

        // Prüfen, ob wirklich dieselbe Instanz
        Console.WriteLine($"Gleiche Instanz? {ReferenceEquals(Logger.Instance, Logger.Instance)}");
        Console.WriteLine($"Messages: {Logger.Instance.MessageCount}");

        Console.WriteLine($"AppConfig contents:");
        AppConfig.Instance.DumpToConsole();
    }
}