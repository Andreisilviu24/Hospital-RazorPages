using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Pacienti
{
    public class EditModel : PageModel
    {
        public List<PacientInfo> listaPacientiEdit = new List<PacientInfo>();
        public PacientInfo pacientInfo = new PacientInfo();
        public String errorMessage = "";
        public String errorMessage2 = "";
        public String errorMessage3 = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Pacienti WHERE Pacienti.PacientID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pacientInfo.id = "" + reader.GetInt32(0);
                                pacientInfo.nume = reader.GetString(1);
                                pacientInfo.prenume = reader.GetString(2);
                                pacientInfo.cnp = reader.GetString(3);
                                pacientInfo.localitate = reader.GetString(4);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            pacientInfo.id = Request.Form["id"];
            pacientInfo.nume = Request.Form["nume"];
            pacientInfo.prenume = Request.Form["prenume"];
            pacientInfo.cnp = Request.Form["cnp"];
            pacientInfo.localitate = Request.Form["localitate"];
            

            if (pacientInfo.nume.Length == 0 || pacientInfo.prenume.Length == 0 ||
                pacientInfo.cnp.Length == 0 || pacientInfo.localitate.Length == 0)
            {
                errorMessage = "Toate campurile trebuie completate!";
                return;
            }

            if (pacientInfo.cnp.Length - 13 != 0)
            {
                errorMessage2 = "CNP incomplet/gresit!";
                return;
            }

            /* try
             {
                 String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                 using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                     connection.Open();
                     String sql = "SELECT * FROM Pacienti";
                     using (SqlCommand command = new SqlCommand(sql, connection))
                     {
                         using (SqlDataReader reader = command.ExecuteReader())
                         {
                             while (reader.Read())
                             {
                                 PacientInfo pacientInfo1 = new PacientInfo();
                                 pacientInfo1.id = "" + reader.GetInt32(0);
                                 pacientInfo1.nume = reader.GetString(1);
                                 pacientInfo1.prenume = reader.GetString(2);
                                 pacientInfo1.cnp = reader.GetString(3);
                                 pacientInfo1.localitate = reader.GetString(4);

                                 listaPacientiAdd.Add(pacientInfo1);
                             }
                         }
                     }
                 }
             }

             catch (Exception ex)
             {
                 Console.WriteLine("Exception: " + ex.ToString());
             }

             for (int i = 0; i < listaPacientiAdd.Count; i++)
             {
                 if (pacientInfo.cnp == listaPacientiAdd[i].cnp)
                 {
                     errorMessage3 = "Acest CNP deja exista in baza de date!";
                     return;
                 }
             }*/

            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    connection1.Open();
                    String sql1 = "UPDATE Pacienti " + "SET Nume=@nume, Prenume=@prenume, CNP=@cnp, Localitate=@localitate " +
                        "WHERE Pacienti.PacientID=@id";

                    using (SqlCommand command1 = new SqlCommand(sql1, connection1))
                    {
                        command1.Parameters.AddWithValue("@id", pacientInfo.id);
                        command1.Parameters.AddWithValue("@nume", pacientInfo.nume);
                        command1.Parameters.AddWithValue("@prenume", pacientInfo.prenume);
                        command1.Parameters.AddWithValue("@cnp", pacientInfo.cnp);
                        command1.Parameters.AddWithValue("@localitate", pacientInfo.localitate);

                        command1.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Pacienti/Index");
        }
    }
}
