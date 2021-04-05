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
            if (Session["login"]==null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            generateSpot.InnerHtml = "<div class='accordion'>" +
                    "<sul class='list-unstyled'>";
            //CONNEXION A LA BASE 
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            //SELECTION SPOT A NE PAS RESERVER POUR UN JOUR DONNE
            string[] output = new string[4];
            int increment = 0;
            string sql = "SELECT TOP (1) CASE WHEN ((SELECT COUNT(*) AS Expr1 FROM spot_reserv AS spot_reserv_1 WHERE(spot_reserv.date_reserv = '2021-04-05') GROUP BY idSpot) = spots.maxPerso) THEN spot_reserv.idSpot ELSE '0' END AS Expr1 FROM spots INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN spot_reserv ON spots.idSpot = spot_reserv.idSpot";
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
           
            string SelecSql = "SELECT spots.nom_Spot, ville.Ville, spots.Adresse_Spot, spots.lien, horaires.HorDeb, horaires.HorFer FROM spots INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN horaires ON spots.idSpot = horaires.idSpot WHERE(spots.idSpot <>"+output[i]+")";
            SqlCommand commande = new SqlCommand(SelecSql, conn);
            SqlDataReader read = commande.ExecuteReader();

            string nomSpot = "";
            string villeSpot = "";
            string AdressSpot = "";
            string linkSpot = " ";

            while (read.Read())
            {
                nomSpot = read.GetValue(0).ToString();
                villeSpot =  read.GetValue(1).ToString();
                AdressSpot =  read.GetValue(2).ToString();
                linkSpot = read.GetValue(3).ToString();
                string horDeb = read.GetValue(4).ToString();
                string horFer = read.GetValue(5).ToString();
                generateSpot.InnerHtml += "<li>" +
                  "<div class='question'>" +
                    "<h2><strong>" + villeSpot + "</strong> - " + nomSpot + " de "+horDeb+"h à "+horFer+"h</h2>" +
                    "<span class='glyphicon glyphicon-chevron-down'></span>" +
                  "</div>" +
                  "<div class='answer'>" +
                    "<p><strong>Adresse : </strong>" + AdressSpot + "</br> <a href=" + linkSpot + " class='linkMaps'> lien vers google maps </a></p>" +
                  "</div>" +
                "</li>";
                i++;


            }
           
            read.Close();
            commande.Dispose();
            conn.Close();


            generateSpot.InnerHtml += "</ul>"+
                  "</div>";
        }
    }
}