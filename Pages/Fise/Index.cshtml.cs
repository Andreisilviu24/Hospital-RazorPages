using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Spitalix.Pages.Fise
{
    public class IndexModel : PageModel
    {
        public List<FisaInfo> listaFise = new List<FisaInfo>();
        public async Task OnGetAsync(string searchString)
        {
            try
            {
                String connectionString = "Data Source=SILVIU-ANDREI-S\\SQLEXPRESS;Initial Catalog=Spital;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "SELECT Fise.FisaID, Medici.Nume, Pacienti.Nume, Fise.Diagnostic, Fise.Urgenta, Fise.Data_Internare, Fise.Data_Externare" +
                        " FROM Fise" +
                        " JOIN Medici ON Medici.MedicID = Fise.MedicID" +
                        " JOIN Pacienti ON Pacienti.PacientID = Fise.PacientID";

                    String sql2 = "select F.FisaID, M.Nume, P.Nume, F.Diagnostic, F.Urgenta, F.Data_Internare, F.Data_Externare from Fise F " +
                        " inner join Medici M on F.MedicID = M.MedicID " +
                        " inner join Pacienti P on F.PacientID = P.PacientID " +
                        " where M.MedicID = (select M2.MedicID from Medici M2 where M2.Nume = '" + searchString + "')" +
                        " order by (select F1.Urgenta From Fise F1 where F1.FisaID = F.FisaID) desc";
                        

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
                                FisaInfo fisaInfo = new FisaInfo();
                                fisaInfo.id = "" + reader.GetInt32(0);
                                fisaInfo.id_medic = "" + reader.GetString(1);
                                fisaInfo.id_pacient = "" + reader.GetString(2);
                                fisaInfo.diagnostic = reader.GetString(3);

                                if (reader.GetBoolean(4) == true)
                                    fisaInfo.urgenta = "Da";
                                else
                                    fisaInfo.urgenta = "Nu";

                                fisaInfo.data_int = "" + reader.GetDateTime(5);
                                fisaInfo.data_ext = "" + reader.GetDateTime(6);

                                listaFise.Add(fisaInfo);
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

    public class FisaInfo
    {
        public String id;
        public String id_medic;
        public String id_pacient;
        public String diagnostic;
        public String urgenta;
        public String data_int;
        public String data_ext;
    }
}
