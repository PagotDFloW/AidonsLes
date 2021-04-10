using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
namespace AidonsLes
{
    public partial class Reservation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            //----------------------GENERATION DATE D'AUJOURDHUI----------------------------------
            DateTime Aujd = DateTime.Today;
            string Tday = Aujd.ToString("yyyy-MM-dd");

            //---------------------GENERATION RESERVATION SPOTS-------------------------------------


            generateSpot.InnerHtml = "<div class='accordion'>" +
                    "<sul class='list-unstyled'>";



            //CONNEXION A LA BASE 
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            //SELECTION SPOT A NE PAS RESERVER POUR JOUR PAR DEFAUT
            string[] output = new string[4];
            int increment = 0;
            string sql = "SELECT CASE WHEN ((SELECT COUNT(*) AS Expr1 FROM spot_reserv AS spot_reserv_1 WHERE(spot_reserv.date_reserv = '" + Tday + "')) = spots.maxPerso) THEN spot_reserv.idSpot ELSE '0' END AS Expr1 FROM spots INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN spot_reserv ON spots.idSpot = spot_reserv.idSpot";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                output[increment] = reader.GetValue(0).ToString();
                increment++;

            }
            int i = 0;
            reader.Close();
            cmd.Dispose();

            //RECUPERATION DERNIER IDRESERV

            int ReservOutput = 0;
            string Reservsql = "SELECT idReserv FROM spot_reserv WHERE idReserv=(SELECT MAX(idReserv) FROM spot_reserv)";
            SqlCommand ReservCmd = new SqlCommand(Reservsql, conn);
      
            SqlDataReader ReservReader = ReservCmd.ExecuteReader();

            while (ReservReader.Read())
            {
                 string ReserveOutput = ReservReader.GetValue(0).ToString();
                 ReservOutput = Convert.ToInt32(ReserveOutput);

            }
            ReservOutput += 1;
            
            ReservReader.Close();
            ReservCmd.Dispose();
            
            //RECUPERATION DONNEES SPOT
            string SelecSql = "SELECT spots.idSpot, spots.nom_Spot, ville.Ville, spots.Adresse_Spot, spots.lien, horaires.HorDeb, horaires.HorFer, horaires.idHor FROM spots INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN horaires ON spots.idSpot = horaires.idSpot WHERE(spots.idSpot <>" + output[i] + ")";
            SqlCommand commande = new SqlCommand(SelecSql, conn);
            SqlDataReader read = commande.ExecuteReader();

            string nomSpot = "";
            string villeSpot = "";
            string AdressSpot = "";
            string linkSpot = " ";

            while (read.Read())
            {
                string SpotId = read.GetValue(0).ToString();
                nomSpot = read.GetValue(1).ToString();
                villeSpot = read.GetValue(2).ToString();
                AdressSpot = read.GetValue(3).ToString();
                linkSpot = read.GetValue(4).ToString();
                string horDeb = read.GetValue(5).ToString();
                string horFer = read.GetValue(6).ToString();
                string idHor = read.GetValue(7).ToString();

                string HiddenSql = "insert into spot_reserv(idReserv, idSpot, date_reserv, idHor, idUser) values("+ReservOutput +","+ SpotId + ", [" + Tday + "[ , " + idHor + ", [" + Session["login"] + "[ )";

                generateSpot.InnerHtml += "<li>" +
                 "<div class='question'>" +
                   "<input id='ReservRequire' name='ReservRequire' type='radio' value='" + HiddenSql + "'/>" + "<h2><strong>" + villeSpot + "</strong> - " + nomSpot + " de " + horDeb + "h à " + horFer + "h</h2>" +
                   "<span class='glyphicon glyphicon-chevron-down'></span>" +
                 "</div>" +
                 "<div class='answer'>" +
                   "<p><strong>Adresse : </strong>" + AdressSpot + "</br> <a href=" + linkSpot + " class='linkMaps'> lien vers google maps</a></p>" +
                   
                i++;

            }

            read.Close();
            commande.Dispose();
            conn.Close();


            generateSpot.InnerHtml += "</ul>" +
                  "</div>";
        }


        protected void submit_Click1(object sender, EventArgs e)
        {
            string dateSelec = Request["dateReserv"];
            generateSpot.InnerHtml = "<h2>Spots disponible ce" + dateSelec + "</h2>";
            generateSpot.InnerHtml += "<div class='accordion'>" +
                    "<sul class='list-unstyled'>";

            //CONNEXION A LA BASE 
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();


            //RECUPERATION DERNIER IDRESERV

            int ReservOutput = 0;
            string Reservsql = "SELECT idReserv FROM spot_reserv WHERE idReserv=(SELECT MAX(idReserv) FROM spot_reserv)";
            SqlCommand ReservCmd = new SqlCommand(Reservsql, conn);

            SqlDataReader ReservReader = ReservCmd.ExecuteReader();

            while (ReservReader.Read())
            {
                string ReserveOutput = ReservReader.GetValue(0).ToString();
                ReservOutput = Convert.ToInt32(ReserveOutput);

            }
            ReservOutput += 1;
            ReservReader.Close();
            ReservCmd.Dispose();



            //SELECTION SPOT A NE PAS RESERVER POUR JOUR PAR DEFAUT
            string[] output = new string[4];
            int increment = 0;
            string sql = "SELECT TOP (1) CASE WHEN ((SELECT COUNT(*) AS Expr1 FROM spot_reserv AS spot_reserv_1 WHERE(spot_reserv.date_reserv = '" + dateSelec + "') GROUP BY idSpot) = spots.maxPerso) THEN spot_reserv.idSpot ELSE '0' END AS Expr1 FROM spots INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN spot_reserv ON spots.idSpot = spot_reserv.idSpot";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                output[increment] = reader.GetValue(0).ToString();
                increment++;

            }
            int i = 0;
            reader.Close();
            cmd.Dispose();

            string SelecSql = "SELECT spots.idSpot, spots.nom_Spot, ville.Ville, spots.Adresse_Spot, spots.lien, horaires.HorDeb, horaires.HorFer, horaires.idHor FROM spots INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN horaires ON spots.idSpot = horaires.idSpot WHERE(spots.idSpot <>" + output[i] + ")";
            SqlCommand commande = new SqlCommand(SelecSql, conn);
            SqlDataReader read = commande.ExecuteReader();

            string nomSpot = "";
            string villeSpot = "";
            string AdressSpot = "";
            string linkSpot = " ";

            while (read.Read())
            {
                string SpotId = read.GetValue(0).ToString();
                nomSpot = read.GetValue(1).ToString();
                villeSpot = read.GetValue(2).ToString();
                AdressSpot = read.GetValue(3).ToString();
                linkSpot = read.GetValue(4).ToString();
                string horDeb = read.GetValue(5).ToString();
                string horFer = read.GetValue(6).ToString();
                string idHor = read.GetValue(7).ToString();

                string HiddenSql = "insert into spot_reserv(idReserv, idSpot, date_reserv, idHor, idUser) values(" + ReservOutput + "," + SpotId + ", [" + dateSelec + "[, " + idHor + ", [" + Session["login"] + "[)";

                generateSpot.InnerHtml += "<li>" +
                  "<div class='question'>" +
                  "<input id='ReservRequire' name='ReservRequire' type='radio' value='"+HiddenSql+"'/>" +
                    "<h2><strong>" + villeSpot + "</strong> - " + nomSpot + " de " + horDeb + "h à " + horFer + "h</h2>" +
                    "<span class='glyphicon glyphicon-chevron-down'></span>" +
                  "</div>" +
                  "<div class='answer'>" +
                    "<p><strong>Adresse : </strong>" + AdressSpot + "</br> <a href=" + linkSpot + " class='linkMaps'> lien vers google maps</a></p>";
                    
                i++;


            }

            read.Close();
            commande.Dispose();
            conn.Close();


            generateSpot.InnerHtml += "</ul>" +
                  "</div>";
        }

        protected void submitReserv_Click(object sender, EventArgs e)
        {
            generateSpot.InnerHtml = "<h2>Votre créneau a été réservé avec succès</h2>";

            string WatRep = hide.Value;
            string ReplaceStr = WatRep.Replace("[", "'");

            //CONNEXION A LA BASE 
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            sql = ReplaceStr;
            command = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();

            command.Dispose();
            conn.Close();




        }
    }

    }
