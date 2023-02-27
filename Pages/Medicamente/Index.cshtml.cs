using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Medicamente
{
    public class IndexModel : PageModel
    {
        public List<MedicamentInfo> listaMedicamente = new List<MedicamentInfo>();
        public async Task OnGetAsync(string searchString)
        {
            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "SELECT Medicamente.MedicamentID, Medicamente.Nume, Medicamente.Concentratie, Medicamente.Din_Programul_National, Medicamente.Pret, " +
                        " COUNT(Prescriptii_Medicament.MedicamentID) AS Nr_Prescrieri" +
                        " FROM Prescriptii_Medicament" +
                        " JOIN Medicamente ON Prescriptii_Medicament.MedicamentID = Medicamente.MedicamentID" +
                        " GROUP BY Medicamente.MedicamentID, Medicamente.Nume, Medicamente.Concentratie, Medicamente.Din_Programul_National, Medicamente.Pret";

                    String sql2 = "SELECT M.MedicamentID, M.Nume, M.Concentratie, M.Din_Programul_National, M.Pret, " +
                        " COUNT(Prescriptii_Medicament.MedicamentID) AS Nr_Prescrieri" +
                        " FROM Prescriptii_Medicament" +
                        " JOIN Medicamente M ON Prescriptii_Medicament.MedicamentID = M.MedicamentID" +
                        " GROUP BY M.MedicamentID, M.Nume, M.Concentratie, M.Din_Programul_National, M.Pret" +
                        " HAVING M.Pret in (select M2.Pret from Medicamente M2 where M2.Pret < " + searchString + " and M2.Din_Programul_National = 1) ";
                        

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
                                MedicamentInfo medicamentInfo = new MedicamentInfo();
                                medicamentInfo.id = "" + reader.GetInt32(0);
                                medicamentInfo.nume = reader.GetString(1);
                                medicamentInfo.concentratie = "" + reader.GetInt32(2) + " mg";

                                if (reader.GetBoolean(3) == true)
                                    medicamentInfo.din_pn = "Da";
                                else
                                    medicamentInfo.din_pn = "Nu";

                                medicamentInfo.pret = "" + reader.GetInt32(4) + " lei";
                                medicamentInfo.nr_prescrieri = "" + reader.GetInt32(5);

                                listaMedicamente.Add(medicamentInfo);
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

    public class MedicamentInfo
    {
        public String id;
        public String nume;
        public String concentratie;
        public String din_pn;
        public String pret;
        public String nr_prescrieri;
    }
}
