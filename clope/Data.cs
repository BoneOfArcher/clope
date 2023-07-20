using System.Transactions;

namespace CLOPE;

class Data
{
    public static Transaction[] GetData()
    {
        string path = Directory.GetCurrentDirectory();
        string dataPath = Path.Combine(path, @"..\..\..\SourceFiles\test.txt");
        List<Transaction> transactions = new List<Transaction>();

        using (StreamReader reader = new StreamReader(dataPath))
        {
            string line;
            int id = 0;
            string[] headers = reader.ReadLine().Split(",");

            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(",");
                string[] data = new string[fields.Length];

                for (int i = 0; i < fields.Length; i++)
                {
                    data[i] = $"{headers[i]} {fields[i]}";
                }

                transactions.Add(new Transaction(id++, data));
            }
        }

        return transactions.ToArray();
    }
}