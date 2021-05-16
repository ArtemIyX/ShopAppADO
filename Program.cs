using System;
using System.Data.SqlClient;

namespace Shop
{
    class Program
    {
        public static readonly string ConnectionStr =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=shop;Integrated Security=True";
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionStr);
            sqlConnection.Open();
            try
            {
                Console.Write("Enter new category name: ");
                string NewCategory = Console.ReadLine();
                SqlCommand InsertCategoryCommand = sqlConnection.CreateCommand();
                InsertCategoryCommand.CommandText = $"INSERT INTO dbo.Categories ([Name]) VALUES ('{NewCategory}')";
                Console.WriteLine(InsertCategoryCommand.CommandText);
                InsertCategoryCommand.ExecuteNonQuery();

                string CategoryName, ProductName;
                decimal Price;
                int Quantity;
                Console.Write("Enter category name: ");
                CategoryName = Console.ReadLine();
                Console.Write("Enter new product name: ");
                ProductName = Console.ReadLine();
                Console.Write("Enter price: ");
                Price = decimal.Parse(Console.ReadLine() ?? "1,0");
                Console.Write("Enter quantity: ");
                Quantity = int.Parse(Console.ReadLine() ?? "0");


                SqlCommand FindCategoryIdCommand = sqlConnection.CreateCommand();
                FindCategoryIdCommand.CommandText = $"SELECT Id FROM dbo.Categories WHERE Categories.[Name] LIKE '%{CategoryName}%'";
                object result = FindCategoryIdCommand.ExecuteScalar();
                if (result is null)
                {
                    throw new Exception("Category is null");
                }
                else
                {
                    int CategoryId = (int)result;
                    SqlCommand insertProductCommand = sqlConnection.CreateCommand();
                    insertProductCommand.CommandText =
                        $"INSERT INTO Products ([Name],CategoryId,Price,Quantity) VALUES ('{ProductName}',{CategoryId},'{Price}',{Quantity})";
                    insertProductCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }

            Console.ReadLine();
        }
    }
}
