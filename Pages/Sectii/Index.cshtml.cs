using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Sectii
{
    public class IndexModel : PageModel
    {
        public List<SectieInfo> listaSectii = new List<SectieInfo>();
        public async Task OnGetAsync(string searchString)
        {
            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "SELECT Sectii.SectieID, Sectii.Nume, Sectii.Etaj, Sectii.NrSaloane, Sectii.NrPaturi, Medici.Nume" +
                        " FROM Sectii" +
                        " JOIN Medici ON Medici.MedicID = Sectii.MedicSefID ";

                    String sql2 = "select S.SectieID, S.Nume, S.Etaj, S.NrSaloane, S.NrPaturi, (select M2.Nume as Nume from Medici M2 where M2.Nume = '"+ searchString +"') from Sectii S " +
                        "inner join Medici M on S.MedicSefID = M.MedicID " +
                        "where M.MedicID not in (select M2.MedicID from Medici M2 where M2.Nume != '" + searchString + "')";

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
                                SectieInfo sectieInfo = new SectieInfo();
                                sectieInfo.id = "" + reader.GetInt32(0);
                                sectieInfo.nume = reader.GetString(1);
                                sectieInfo.etaj = "" + reader.GetInt32(2);
                                sectieInfo.nr_saloane = "" + reader.GetInt32(3);
                                sectieInfo.nr_paturi = "" + reader.GetInt32(4);
                                sectieInfo.id_sef_medic = "" + reader.GetString(5);

                                listaSectii.Add(sectieInfo);
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

    public class SectieInfo
    {
        public String id;
        public String nume;
        public String etaj;
        public String nr_saloane;
        public String nr_paturi;
        public String id_sef_medic;
    }
}
