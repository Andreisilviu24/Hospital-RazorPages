using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Medici
{
    public class IndexModel : PageModel
    {
        public List<MedicInfo> listaMedici = new List<MedicInfo>();

        public async Task OnGetAsync(string searchString)
        {
            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "SELECT Medici.MedicID, Medici.Nume, Medici.Prenume, Medici.CNP, Medici.Specializare, Medici.Functie, Sectii.Nume " +
                        "FROM Medici " +
                        "JOIN Sectii on Sectii.SectieID = Medici.SectieID";

                    String sql2 = "SELECT Medici.MedicID, Medici.Nume, Medici.Prenume, Medici.CNP, Medici.Specializare, Medici.Functie, Sectii.Nume " +
                        "FROM Medici " +
                        "JOIN Sectii on Sectii.SectieID = Medici.SectieID WHERE Medici.Nume = '" + searchString + "'";

                    String sql;

                    if (!String.IsNullOrEmpty(searchString))
                        sql = sql2;
                    else
                        sql = sql1;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MedicInfo medicInfo = new MedicInfo();
                                medicInfo.id = "" + reader.GetInt32(0);
                                medicInfo.nume = reader.GetString(1);
                                medicInfo.prenume = reader.GetString(2);
                                medicInfo.cnp = reader.GetString(3);
                                medicInfo.specializare = reader.GetString(4);
                                medicInfo.functie = reader.GetString(5);
                                medicInfo.id_sectie = "" + reader.GetString(6);

                                listaMedici.Add(medicInfo);
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

}

public class MedicInfo
{
    public String id;
    public String nume;
    public String prenume;
    public String specializare;
    public String functie;
    public String cnp;
    public String id_sectie;
}

