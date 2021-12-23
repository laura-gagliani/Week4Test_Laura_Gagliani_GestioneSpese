using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4Test.GestioneSpese.ConsoleApp
{
    internal class RepositoryADODisconnected
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GestioneSpese;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        internal static bool AddExpense(Spesa newExpense)
        {
            DataSet expenseDS = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //inizializzo l'adapter
                SqlDataAdapter expenseAdapter = InitializeExpenseAdapter(connection);
                //faccio il Fill
                expenseAdapter.Fill(expenseDS, "Spesa");
                connection.Close();

                DataRow newRow = expenseDS.Tables["Spesa"].NewRow();
                newRow["Data"] = newExpense.Data;
                newRow["CategoriaId"] = newExpense.CategoriaId;
                newRow["Descrizione"] = newExpense.Descrizione;
                newRow["Utente"] = newExpense.Utente;
                newRow["Importo"] = newExpense.Importo;
                newRow["Approvato"] = newExpense.Approvato;

                expenseDS.Tables["Spesa"].Rows.Add(newRow);
                expenseAdapter.Update(expenseDS, "Spesa");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        internal static Dictionary<string, double> SelectTotalByCategory()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                
                connection.Open();

                SqlCommand cmd = new SqlCommand("select s.CategoriaId, sum(s.Importo) from Spesa s group by s.CategoriaId", connection);
                
                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<string, double> result = new Dictionary<string, double>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    double total = (double)reader.GetDecimal(1);
                    
                    string categName = GetAllCategories().Where(c => c.Id == id).Select(c => c.NomeCategoria).SingleOrDefault();

                    result.Add(categName, total);
                }

                connection.Close();
                return result;
                               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return null;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        internal static List<Spesa> GetAllExpenses()
        {
            DataSet expenseDS = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //inizializzo l'adapter
                SqlDataAdapter expenseAdapter = InitializeExpenseAdapter(connection);
                //faccio il Fill
                expenseAdapter.Fill(expenseDS, "Spesa");
                connection.Close();

                List<Spesa> expenses = new List<Spesa>();

                foreach (DataRow row in expenseDS.Tables["Spesa"].Rows)
                {
                    Spesa s = new Spesa();

                    s.Id = Convert.ToInt32(row[0]);
                    s.Data = Convert.ToDateTime(row[1]);
                    s.CategoriaId = Convert.ToInt32(row[2]);
                    s.Categoria = GetAllCategories().SingleOrDefault(c => c.Id == s.CategoriaId);
                    s.Descrizione = Convert.ToString(row[3]);
                    s.Utente = Convert.ToString(row[4]);    
                    s.Importo = Convert.ToDouble(row[5]);
                    s.Approvato = Convert.ToBoolean(row[6]);   

                    expenses.Add(s);

                }
                return expenses;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return null;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        internal static bool DeleteExpense(int id)
        {
            DataSet expenseDS = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlDataAdapter expenseAdapter = InitializeExpenseAdapter(connection);
                expenseAdapter.Fill(expenseDS, "Spesa");
                connection.Close();

                DataRow rowToUpdate = expenseDS.Tables["Spesa"].Rows.Find(id);
                if (rowToUpdate != null)
                {
                    rowToUpdate.Delete();
                }
                expenseAdapter.Update(expenseDS, "Spesa");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        internal static bool UpdateExpense(int id)
        {
            DataSet expenseDS = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlDataAdapter expenseAdapter = InitializeExpenseAdapter(connection);
                expenseAdapter.Fill(expenseDS, "Spesa");
                connection.Close();

                DataRow rowToUpdate = expenseDS.Tables["Spesa"].Rows.Find(id);
                if (rowToUpdate != null)
                {
                    rowToUpdate.SetField("Approvato", 1);
                }
                expenseAdapter.Update(expenseDS, "Spesa");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        internal static List<Categoria> GetAllCategories()
        {
            DataSet categDS = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //inizializzo l'adapter
                SqlDataAdapter categAdapter = InitializeCategAdapter(connection);
                //faccio il Fill
                categAdapter.Fill(categDS, "Categoria");
                connection.Close();

                List<Categoria> categories = new List<Categoria>();

                foreach (DataRow row in categDS.Tables["Categoria"].Rows)
                {
                    Categoria c = new Categoria();

                    c.Id = Convert.ToInt32(row[0]);
                    c.NomeCategoria = Convert.ToString(row[1]);

                    categories.Add(c);

                }
                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return null;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }





        private static SqlDataAdapter InitializeCategAdapter(SqlConnection connection)
        {
            SqlDataAdapter a = new SqlDataAdapter();

            //select
            a.SelectCommand = new SqlCommand("select * from Categoria", connection);
            a.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            
            return a;
        }

        private static SqlDataAdapter InitializeExpenseAdapter(SqlConnection connection)
        {
            SqlDataAdapter a = new SqlDataAdapter();

            //select
            a.SelectCommand = new SqlCommand("select * from Spesa", connection);
            a.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //insert
            a.InsertCommand = new SqlCommand("insert into Spesa values (@data, @categId, @descr, @ut, @importo, @approv)", connection);
            a.InsertCommand.Parameters.Add(new SqlParameter("@data", SqlDbType.Date, 100, "Data"));
            a.InsertCommand.Parameters.Add(new SqlParameter("@categId", SqlDbType.Int, 0, "CategoriaId"));
            a.InsertCommand.Parameters.Add(new SqlParameter("@descr", SqlDbType.VarChar, 500, "Descrizione"));
            a.InsertCommand.Parameters.Add(new SqlParameter("@ut", SqlDbType.VarChar, 100, "Utente"));
            a.InsertCommand.Parameters.Add(new SqlParameter("@importo", SqlDbType.Decimal, 100, "Importo"));
            a.InsertCommand.Parameters.Add(new SqlParameter("@approv", SqlDbType.Bit, 1, "Approvato"));

            //update
            a.UpdateCommand = new SqlCommand("update Spesa set Approvato = 1 where Id=@id", connection);
            a.UpdateCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));

            //delete
            a.DeleteCommand = new SqlCommand("delete from Spesa where Id=@id", connection);
            a.DeleteCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));


            return a;
        }
    }
}
