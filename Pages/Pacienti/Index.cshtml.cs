using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Pacienti
{
    public class IndexModel : PageModel
    {
        public List<PacientInfo> listaPacienti = new List<PacientInfo>();
        public void OnGet()
        {
            try
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
                                PacientInfo pacientInfo = new PacientInfo();
                                pacientInfo.id = "" + reader.GetInt32(0);
                                pacientInfo.nume = reader.GetString(1);
                                pacientInfo.prenume = reader.GetString(2);
                                pacientInfo.cnp = reader.GetString(3);
                                pacientInfo.localitate = reader.GetString(4);

                                listaPacienti.Add(pacientInfo);
                            }
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class PacientInfo
    {
        public String id;
        public String nume;
        public String prenume;
        public String cnp;
        public String localitate;
    }
}
