using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Bulkcopy2
{
    class Program
    {
        static void Main(string[] args)
        {
            string Ddb = ConfigurationManager.ConnectionStrings["DestinationDB"].ConnectionString;
            string Sdb = ConfigurationManager.ConnectionStrings["SourceDB"].ConnectionString;
            using (SqlConnection SourceConnection = new SqlConnection(Sdb))
            {
                string cmdText = "select * from tblStudents";
                SqlCommand cmd = new SqlCommand(cmdText, SourceConnection);
                SourceConnection.Open();
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    using (SqlConnection DestinationConnection = new SqlConnection(Ddb))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(Ddb))
                        {
                            sqlBulkCopy.DestinationTableName = "tblStudents";
                            sqlBulkCopy.BatchSize = 500;
                            sqlBulkCopy.NotifyAfter = 100;
                            sqlBulkCopy.SqlRowsCopied += (sender, e) =>
                            {
                                Console.WriteLine(e.RowsCopied + " Loading...");
                            };
                            sqlBulkCopy.WriteToServer(sqlDataReader);
                            
                        } 

                    }
                }


            }
            Console.WriteLine("All rows copied successfully");
            Console.Read();


        }

       
    }
}
