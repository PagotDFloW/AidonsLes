using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using AidonsLes.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace AidonsLes.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            // Activer ceci après la configuration de l'authentification à 2 facteurs
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (!IsPostBack)
            {
                // Déterminer les sections à afficher
                if (HasPassword(manager))
                {
                    ChangePassword.Visible = true;
                }
                else
                {
                    CreatePassword.Visible = true;
                    ChangePassword.Visible = false;
                }

                // Afficher le message de réussite
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Enlever la chaîne de requête de l'action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Votre mot de passe a été modifié."
                        : message == "SetPwdSuccess" ? "Votre mot de passe a été défini."
                        : message == "RemoveLoginSuccess" ? "Le compte a été supprimé."
                        : message == "AddPhoneNumberSuccess" ? "Le numéro de téléphone a été ajouté"
                        : message == "RemovePhoneNumberSuccess" ? "Le numéro de téléphone a été supprimé"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }


            generateMySpot.InnerHtml = "<div class='accordion'>" +
                    "<sul class='list-unstyled'>";

            //CONNEXION A LA BASE 
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            //RECUPERATION DONNEES SPOT
            string SelecSql = "SELECT ville.Ville, spot_reserv.idReserv, spots.nom_Spot, spots.Adresse_Spot, spots.lien, spot_reserv.date_reserv, spot_reserv.idHor, horaires.HorDeb, horaires.HorFer, spot_reserv.idSpot FROM spot_reserv INNER JOIN spots ON spot_reserv.idSpot = spots.idSpot INNER JOIN ville ON spots.idVille = ville.idVille INNER JOIN horaires ON spot_reserv.idHor = horaires.idHor WHERE (spot_reserv.idUser = '"+Session["login"]+"')";
            SqlCommand commande = new SqlCommand(SelecSql, conn);
            SqlDataReader read = commande.ExecuteReader();

            string nomSpot = "";
            string villeSpot = "";
            string AdressSpot = "";
            string linkSpot = " ";

            while (read.Read())
            {
                villeSpot = read.GetValue(0).ToString();
                string idReserv = read.GetValue(1).ToString();
                nomSpot = read.GetValue(2).ToString();
                AdressSpot = read.GetValue(3).ToString();
                linkSpot = read.GetValue(4).ToString();
                string dateReserv = read.GetValue(5).ToString();
                string idHor = read.GetValue(6).ToString();
                string horDeb = read.GetValue(7).ToString();
                string horFer = read.GetValue(8).ToString();
                string idSpot = read.GetValue(9).ToString();

                int TrueIdReserv = Convert.ToInt32(idReserv);
                int TrueIdHor = Convert.ToInt32(idHor);

                string TrueDateReserv = dateReserv.Replace("00:00:00", " ");



                string HiddenSql = "DELETE spot_reserv WHERE idReserv="+TrueIdReserv+" AND idSpot= "+idSpot+" AND date_reserv=["+dateReserv+"[ AND idUser=["+Session["login"]+"[ AND idHor="+TrueIdHor ;

                generateMySpot.InnerHtml += "<li>" +
                 "<div class='question'>" +
                   "<input id='ReservRequire' name='ReservRequire' type='radio' value='" + HiddenSql + "'/>" + "<h2><strong>" + villeSpot + "</strong> - " + nomSpot + " le "+TrueDateReserv+" de " + horDeb + "h à " + horFer + "h</h2>" +
                   "<span class='glyphicon glyphicon-chevron-down'></span>" +
                 "</div>" +
                 "<div class='answer'>" +
                   "<p><strong>Adresse : </strong>" + AdressSpot + "</br> <a href=" + linkSpot + " class='linkMaps'> lien vers google maps</a></p>";


            }

            read.Close();
            commande.Dispose();
            conn.Close();


            generateMySpot.InnerHtml += "</ul>" +
                  "</div>";
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Supprimer le numéro de téléphone de l'utilisateur
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage");
        }

        protected void submitReserv_Click(object sender, EventArgs e) { 


            generateMySpot.InnerHtml = "<h2>Votre créneau a été annulé avec succès</h2>";

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
            adapter.DeleteCommand = new SqlCommand(sql, conn);
            adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            conn.Close();


        }
    }
}