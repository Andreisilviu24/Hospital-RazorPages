using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Pacienti
{
    public class CreateModel : PageModel
    {
        public List<PacientInfo> listaPacientiAdd = new List<PacientInfo>();
        public PacientInfo pacientInfo = new PacientInfo();
        public String errorMessage = "";
        public String errorMessage2 = "";
        public String errorMessage3 = "";
        public String successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
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

            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    connection1.Open();
                    String sql1 = "INSERT INTO Pacienti " + "(Nume, Prenume, CNP, Localitate) VALUES " +
                             "(@nume, @prenume, @cnp, @localitate);";
                    using (SqlCommand command1 = new SqlCommand(sql1, connection1))
                    {
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

            pacientInfo.nume = "";
            pacientInfo.prenume = "";
            pacientInfo.cnp = "";
            pacientInfo.localitate = "";
            successMessage = "Pacient nou adaugat cu succes!";

            Response.Redirect("/Pacienti/Index");

        }
    }
}
