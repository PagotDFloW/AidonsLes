using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using AidonsLes.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace AidonsLes.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Activez ceci une fois que vous avez activé la confirmation du compte pour la fonctionnalité de réinitialisation du mot de passe
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Valider le mot de passe de l'utilisateur
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // Ceci ne compte pas les échecs de connexion pour le verrouillage du compte
                // Pour que les échecs de mot passe déclenchent le verrouillage, utilisez shouldLockout: true
                var result = signinManager.PasswordSignIn(login.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        //CONNEXION A LA BASE 
                        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        SqlConnection conn = new SqlConnection(connStr);


                        //SELECTION SPOT A NE PAS RESERVER POUR JOUR PAR DEFAUT
                        string output = "";
                        string sql = "SELECT Id FROM AspNetUsers WHERE UserName ='" + login.Text+"'";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            output = reader.GetValue(0).ToString();


                        }
                        reader.Close();
                        cmd.Dispose();

                        Session["login"] = output;
                        conn.Close();


                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Tentative de connexion non valide";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }
    }
}