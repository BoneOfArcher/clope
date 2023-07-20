namespace CLOPE;

class Programm
{
    static void Main()
    {
        Transaction[] data = Data.GetData();
        Cluster[] result = Clope.Clustering(data, 2.7);

        foreach (var cluster in result)
        {
            Console.WriteLine($"N = {cluster.N}");
            Console.WriteLine($"W = {cluster.W}");
            Console.WriteLine($"S = {cluster.S}");
            Console.WriteLine("---------");
        }

        Console.WriteLine($"Итого кластеров {result.Length}");
    }
}