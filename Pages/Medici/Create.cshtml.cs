using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Medici
{
    public class CreateModel : PageModel
    {
        public List<MedicInfo> listaMediciAdd = new List<MedicInfo>();
        public MedicInfo medicInfo = new MedicInfo();
        public String errorMessage = "";
        public String errorMessage2 = "";
        public String errorMessage3 = "";
        public String errorMessage4 = "";
        public String successMessage = "";
        public String aux = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
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
                   
                    String sql1 = "INSERT INTO Medici " + "(Nume, Prenume, CNP, Specializare, Functie, SectieID) VALUES " +
                             "(@nume, @prenume, @cnp, @specializare, @functie, @id_sectie);";
                    using (SqlCommand command1 = new SqlCommand(sql1, connection1))
                    {
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

            medicInfo.nume = "";
            medicInfo.prenume = "";
            medicInfo.cnp = "";
            medicInfo.specializare = "";
            medicInfo.functie = "";
            medicInfo.id_sectie = "";
            successMessage = "Medic nou adaugat cu succes!";

            Response.Redirect("/Medici/Index");

        }
    }
}
