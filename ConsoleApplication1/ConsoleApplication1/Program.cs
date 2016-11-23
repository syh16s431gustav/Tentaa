using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Models
{
    class Program
    {
        static string cns = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NORTHWND;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main(string[] args)
        {
            //ProductsByCategoryName("Confections");
            //SalesByTerritory();
            //EmployeesPerRegion();
            OrdersPerEmployee();
        }

        private static void OrdersPerEmployee()
        {
            using (Model1 cx = new Model1())
            {

                var result = cx.Database.SqlQuery<GetResultsOrdersPerCustomers>("OrdersPerCustomerQ4").ToList();
                Console.WriteLine("Order per customer");
                foreach (var item in result)
                {
                    Console.WriteLine("Customer Name: " + item.FirstName + "\nCustomer LastName: " + item.LastName + "\nOrders Per customer: " + item.OrdersPerCustomer + "\n");
                }
                Console.ReadLine();

            }
        }

        private static void EmployeesPerRegion()
        {
            SqlConnection cn = new SqlConnection(cns);
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT        Region.RegionID, COUNT(Employees.EmployeeID) AS [Total Employees Per District] FROM Region INNER JOIN Territories ON Region.RegionID = Territories.RegionID INNER JOIN EmployeeTerritories ON Territories.TerritoryID = EmployeeTerritories.TerritoryID INNER JOIN Employees ON EmployeeTerritories.EmployeeID = Employees.EmployeeID GROUP BY Region.RegionID";
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Console.WriteLine(rd.GetInt32(0));
                Console.WriteLine(rd.GetInt32(1) + "\n");
            }
            rd.Close();
            cn.Close();
            Console.ReadLine();
        }

        private static void SalesByTerritory()
        {
            SqlConnection cn = new SqlConnection(cns);
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT       top 5 Territories.TerritoryDescription, SUM([Order Details].Quantity) AS [Total Sales Per District] FROM Territories INNER JOIN EmployeeTerritories ON Territories.TerritoryID = EmployeeTerritories.TerritoryID INNER JOIN Employees ON EmployeeTerritories.EmployeeID = Employees.EmployeeID INNER JOIN Orders ON Employees.EmployeeID = Orders.EmployeeID INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID GROUP BY Territories.TerritoryDescription ORDER BY[Total Sales Per District] DESC";
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Console.WriteLine(rd.GetString(0));
                Console.WriteLine(rd.GetInt32(1) + "\n");
            }
            rd.Close();
            cn.Close();
            Console.ReadLine();
        }

        private static void ProductsByCategoryName(string CategoryName)
        {
            SqlConnection cn = new SqlConnection(cns);
            cn.Open();
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT        Products.ProductName, Products.UnitPrice, Products.UnitsInStock FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID WHERE(Categories.CategoryName = @CategoryName)";
            cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Console.WriteLine(rd.GetString(0));
                Console.WriteLine(rd.GetDecimal(1));
                Console.WriteLine(rd.GetInt16(2) + "\n");
            }
            rd.Close();
            cn.Close();
            Console.ReadLine();
        }
    }
}
