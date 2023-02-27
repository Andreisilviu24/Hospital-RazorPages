using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Medici
{
    public class EditModel : PageModel
    {
        public List<MedicInfo> listaMediciEdit = new List<MedicInfo>();
        public MedicInfo medicInfo = new MedicInfo();
        public String errorMessage = "";
        public String errorMessage2 = "";
        public String errorMessage3 = "";
        public String errorMessage4 = "";
        public String successMessage = "";
        public String aux = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Medici WHERE Medici.MedicID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                medicInfo.id = "" + reader.GetInt32(0);
                                medicInfo.nume = reader.GetString(1);
                                medicInfo.prenume = reader.GetString(2);
                                medicInfo.cnp = reader.GetString(3);
                                medicInfo.specializare = reader.GetString(4);
                                medicInfo.functie = reader.GetString(5);
                                medicInfo.id_sectie = "" + reader.GetInt32(6);

                            }
                        }
                    }

                    String sql1 = "SELECT Nume FROM Sectii WHERE Sectii.SectieID=" + medicInfo.id_sectie;
                    using (SqlCommand command1 = new SqlCommand(sql1, connection))
                    {
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                medicInfo.id_sectie = reader.GetString(0);
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
            medicInfo.id = Request.Form["id"];
            medicInfo.nume = Request.Form["nume"];
            medicInfo.prenume = Request.Form["prenume"];
            medicInfo.cnp = Request.Form["cnp"];
            medicInfo.specializare = Request.Form["specializare"];
            medicInfo.functie = Request.Form["functie"];
            medicInfo.id_sectie = Request.Form["id_sectie"];

            if (medicInfo.nume.Length == 0 || medicInfo.prenume.Length == 0 ||
                medicInfo.cnp.Length == 0 || medicInfo.specializare.Length == 0 ||
                medicInfo.functie.Length == 0 || medicInfo.id_sectie.Length == 0)
            {
                errorMessage = "Toate campurile trebuie completate!";
                return;
            }

            if (medicInfo.cnp.Length - 13 != 0)
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
                    String sql = "SELECT * FROM Medici";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MedicInfo medicInfo1 = new MedicInfo();
                                medicInfo1.id = "" + reader.GetInt32(0);
                                medicInfo1.nume = reader.GetString(1);
                                medicInfo1.prenume = reader.GetString(2);
                                medicInfo1.cnp = reader.GetString(3);
                                medicInfo1.specializare = reader.GetString(4);
                                medicInfo1.functie = reader.GetString(5);
                                medicInfo1.id_sectie = "" + reader.GetInt32(6);

                                listaMediciEdit.Add(medicInfo1);
                            }
                        }
                    }


                    for (int i = 0; i < listaMediciEdit.Count; i++)
                    {
                        String sql1 = "SELECT Nume FROM Sectii WHERE Sectii.SectieID=" + listaMediciEdit[i].id_sectie;
                        using (SqlCommand command1 = new SqlCommand(sql1, connection))
                        {
                            using (SqlDataReader reader = command1.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    listaMediciEdit[i].id_sectie = reader.GetString(0);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            int ok = 0;

            for (int i = 0; i < listaMediciEdit.Count; i++)
            {
                if (medicInfo.id_sectie == listaMediciEdit[i].id_sectie)
                    ok = 1;
            }

            if (ok == 0)
            {
                errorMessage4 = "Aceasta sectie nu exista!";
                return;
            }*/

            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    connection1.Open();
                    String sql2 = "SELECT SectieID FROM Sectii WHERE Sectii.Nume='" + medicInfo.id_sectie + "'";
                    using (SqlCommand command2 = new SqlCommand(sql2, connection1))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                aux = "" + reader.GetInt32(0);

                            }
                        }
                    }
                    
                    String sql1 = "UPDATE Medici " + "SET Nume=@nume, Prenume=@prenume, CNP=@cnp, Specializare=@specializare, Functie=@functie, SectieID=@id_sectie " +
                        "WHERE Medici.MedicID=@id";
                    
                    using (SqlCommand command1 = new SqlCommand(sql1, connection1))
                    {
                        command1.Parameters.AddWithValue("@id", medicInfo.id);
                        command1.Parameters.AddWithValue("@nume", medicInfo.nume);
                        command1.Parameters.AddWithValue("@prenume", medicInfo.prenume);
                        command1.Parameters.AddWithValue("@cnp", medicInfo.cnp);
                        command1.Parameters.AddWithValue("@specializare", medicInfo.specializare);
                        command1.Parameters.AddWithValue("@functie", medicInfo.functie);
                        command1.Parameters.AddWithValue("@id_sectie", Int32.Parse(aux));

                        command1.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Medici/Index");
        }
    }
}
