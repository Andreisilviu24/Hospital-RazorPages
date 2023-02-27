using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Prescriptii
{
    public class IndexModel : PageModel
    {
        public List<PrescriptieInfo> listaPrescriptii = new List<PrescriptieInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT Prescriptii_Medicament.Prescriptie_MedicamentID, Prescriptii_Medicament.NumePrescriptie," +
                        " Medicamente.Nume, Prescriptii_Medicament.Doza, Prescriptii_Medicament.Data_Administrarii_Inceput," +
                        " Prescriptii_Medicament.Data_Administrarii_Sfarsit" +
                        " FROM Prescriptii_Medicament JOIN Medicamente ON Medicamente.MedicamentID = Prescriptii_Medicament.MedicamentID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PrescriptieInfo prescriptieInfo = new PrescriptieInfo();
                                prescriptieInfo.id = "" + reader.GetInt32(0);
                                prescriptieInfo.nume = reader.GetString(1);
                                prescriptieInfo.id_medicament = "" + reader.GetString(2);
                                prescriptieInfo.doza = "" + reader.GetInt32(3);
                                prescriptieInfo.data_initial = "" + reader.GetDateTime(4);
                                prescriptieInfo.data_final = "" + reader.GetDateTime(5);

                                listaPrescriptii.Add(prescriptieInfo);
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

    public class PrescriptieInfo
    {
        public String id;
        public String nume;
        public String id_medicament;
        public String doza;
        public String data_initial;
        public String data_final;
    }
}
