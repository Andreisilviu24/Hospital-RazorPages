using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Prescriptii_si_Pacienti
{
    public class IndexModel : PageModel
    {
        public List<PPInfo> listaPP = new List<PPInfo>();
        public async Task OnGetAsync(string searchString)
        {
            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "SELECT Pacient_si_Prescriptie_Medicament.RelatieID, Pacienti.Nume, Prescriptii_Medicament.NumePrescriptie " +
                        "FROM Pacient_si_Prescriptie_Medicament " +
                        "JOIN Pacienti ON Pacienti.PacientID = Pacient_si_Prescriptie_Medicament.PacientID " +
                        "JOIN Prescriptii_Medicament ON Prescriptii_Medicament.Prescriptie_MedicamentID = Pacient_si_Prescriptie_Medicament.Prescriptie_MedicamentID";

                    String sql2 = "select PP.RelatieID, P.Nume, PM.NumePrescriptie From Pacienti P " +
                        "inner join Pacient_si_Prescriptie_Medicament PP on PP.PacientID = P.PacientID " +
                        "inner join Prescriptii_Medicament PM on PM.Prescriptie_MedicamentID = PP.Prescriptie_MedicamentID " +
                        "where PP.Prescriptie_MedicamentID in (select PP1.Prescriptie_MedicamentID from Pacient_si_Prescriptie_Medicament PP1 where PP1.Prescriptie_MedicamentID = (select PM.Prescriptie_MedicamentID from Prescriptii_Medicament PM " +
                        "where PM.NumePrescriptie = '" + searchString + "'))";

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
                                PPInfo ppInfo = new PPInfo();
                                ppInfo.id = "" + reader.GetInt32(0);
                                ppInfo.id_pacient = "" + reader.GetString(1);
                                ppInfo.id_prescriptie = "" + reader.GetString(2);

                                listaPP.Add(ppInfo);
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

    public class PPInfo
    {
        public String id;
        public String id_pacient;
        public String id_prescriptie;
    }
}