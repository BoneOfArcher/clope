namespace CLOPE;

public class Transaction
{
    public Transaction(int id, string[] data)
    {
        Id = id;
        Data = data;
    }
    public int Id { get; }
    public string[] Data;
}

public class Cluster
{
    public Cluster()
    {
        Transactions = new HashSet<int>();
        HashMap = new Dictionary<string, int>();
    }

    /// <summary>
    /// Id транзацкий клaстера
    /// </summary>
    private HashSet<int> Transactions;

    /// <summary>
    /// Количество транзакций
    /// </summary>
    public int N => Transactions.Count;

    /// <summary>
    /// Количество вхождений каждого элемента
    /// </summary>
    private Dictionary<string, int> HashMap;

    /// <summary>
    /// Ширина кластера
    /// </summary>
    public int W => HashMap.Count;

    /// <summary>
    /// Количество элементов
    /// </summary>
    public int S => HashMap.Sum(pair => pair.Value);

    /// <summary>
    /// Расчёт добавления в кластер
    /// </summary>
    public double GetAddCost(Transaction transaction, double r)
    {
        int Snew = S + transaction.Data.Length;
        int Wnew = W + transaction.Data.Count(data => !HashMap.TryGetValue(data, out int count));

        return Snew * (N + 1) / Math.Pow(Wnew, r) - S * N / Math.Pow(W, r);
    }

    /// <summary>
    /// Расчёт удаления из кластера
    /// </summary>
    public double GetRemoveCost(Transaction transaction, double r)
    {
        int Snew = S - transaction.Data.Length;
        int Wnew = W - transaction.Data.Count(data => HashMap.TryGetValue(data, out int count) && count == 1);

        return Snew * (N - 1) / Math.Pow(Wnew, r) - S * N / Math.Pow(W, r);
    }

    /// <summary>
    /// Добавляет транзакцию в кластер
    /// </summary>
    public void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction.Id);
        foreach (string data in transaction.Data)
        {
            if (HashMap.TryGetValue(data, out int count))
            {
                HashMap[data] += 1;
            }
            else
            {
                HashMap.Add(data, 1);
            }
        }
    }

    /// <summary>
    /// Удаляет транзакцию из кластера
    /// </summary>
    public void RemoveTransaction(Transaction transaction)
    {
        Transactions.Remove(transaction.Id);

        foreach (string data in transaction.Data)
        {
            if (HashMap[data] == 1)
            {
                HashMap.Remove(data);
            }
            else
            {
                HashMap[data] -= 1;
            }
        }
    }

    public bool IsGotTransaction(Transaction transaction)
    {
        return Transactions.Contains(transaction.Id);
    }
}
