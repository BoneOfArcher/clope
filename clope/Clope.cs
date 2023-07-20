namespace CLOPE;

public class Clope
{
    public static Cluster[] Clustering(Transaction[] transactions, double r)
    {
        _clusters.Clear();
        int emptyCluster = AddCluster();

        foreach (Transaction transaction in transactions)
        {
            int bestChose = emptyCluster;
            double maxCost = 0;

            for (int i = 0; i < _clusters.Count; i++)
            {
                Cluster cluster = _clusters[i];
                double newCost = cluster.GetAddCost(transaction, r);

                if (!(newCost > maxCost)) continue;

                bestChose = i;
                maxCost = newCost;
            }

            if (bestChose == emptyCluster)
            {
                emptyCluster = AddCluster();
            }

            _clusters[bestChose].AddTransaction(transaction);
        }

        bool moved;

        do
        {
            moved = false;

            foreach (var transaction in transactions)
            {
                double maxCost = 0;
                int prevCluster = GetTransactionClusterId(transaction);
                double removeCost = _clusters[prevCluster].GetRemoveCost(transaction, r);
                int bestChose = prevCluster;

                for (int i = 0; i < _clusters.Count; i++)
                {
                    if (i == prevCluster) continue;

                    Cluster cluster = _clusters[i];
                    double fullRemoveCost = cluster.GetAddCost(transaction, r) + removeCost;

                    if (!(fullRemoveCost > maxCost)) continue;

                    maxCost = fullRemoveCost;
                    bestChose = i;
                }

                if (bestChose == prevCluster || !(maxCost > 0)) continue;
                if (bestChose == emptyCluster)
                {
                    emptyCluster = AddCluster();
                }

                _clusters[prevCluster].RemoveTransaction(transaction);
                _clusters[bestChose].AddTransaction(transaction);
                moved = true;
            }
        } while (moved);

        ClearClusters();

        return _clusters.ToArray();
    }

    private static List<Cluster> _clusters = new List<Cluster>();

    /// <summary>
    /// Добавляет новый кластер
    /// </summary>
    private static int AddCluster()
    {
        _clusters.Add(new Cluster());
        return _clusters.Count - 1;
    }

    /// <summary>
    /// Поиск кластера, в котором находится транзакция
    /// </summary>
    private static int GetTransactionClusterId(Transaction transaction)
    {
        for (int i = 0; i < _clusters.Count; i++)
        {
            if (_clusters[i].IsGotTransaction(transaction)) return i;
        }

        throw new Exception($"Не удалось найти кластер к котором находится транзакция {transaction.Id}");
    }

    private static void ClearClusters()
    {
        _clusters = _clusters.Where((cluster, i) => cluster.N != 0).ToList();
    }
}