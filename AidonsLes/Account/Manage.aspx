<%@ Page Title="Gérer le compte" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="AidonsLes.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body, .jumbotron { padding: 30px; }
    html      {
      overflow-y: scroll;
    }
    body      {
      background: #EFEFEF;
    }
    ul        {
      margin-bottom: 0;
    }
    .accordion  {
      background: #FFF;
      border-radius: 5px;
      padding: 30px;
    }

    .question {
      border-top: 1px solid #EEE;
      padding: 20px;
      cursor: pointer;
      position: relative;
    }
    .question h2  {
      font-size: 20px;
      margin: 0;
      color: #C00C00;
    }
    .question .glyphicon {
      position: absolute;
      right: 20px;
      top: 0;
      height: 100%;
      display: flex;
      align-items: center;
      color: #C00C00;
      transition: 1s cubic-bezier(0.175, 0.885, 0.32, 1.275) all;
    }

    .answer   {
      max-height: 0;
      overflow: hidden;
      transition: 1s cubic-bezier(0.175, 0.885, 0.32, 1.275) all;
    }
    .answer p {
      padding: 20px;
    }

    .accordion li.open .question .glyphicon {
      transform: rotate(180deg);
    }
    .accordion li.open .answer  {
      max-height: 150px;
    }
    </style>
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Modifier vos paramètres de compte</h4>
                <hr />
                <dl class="dl-horizontal">
                    <div id="generateMySpot" runat="server">
                    </div>
                    <dt>Mot de passe :</dt>
                    <dd>
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Changer]" Visible="false" ID="ChangePassword" runat="server" />
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Créer]" Visible="false" ID="CreatePassword" runat="server" />
                    </dd>
                    <dd>
                        <% if (TwoFactorEnabled)
                          { %> 
                        <%--
                        Enabled
                        <asp:LinkButton Text="[Disable]" runat="server" CommandArgument="false" OnClick="TwoFactorDisable_Click" />
                        --%>
                        <% }
                          else
                          { %> 
                        <%--
                        Disabled
                        <asp:LinkButton Text="[Enable]" CommandArgument="true" OnClick="TwoFactorEnable_Click" runat="server" />
                        --%>
                        <% } %>
                    </dd>
                </dl>
                <div id="result">
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                </div>

    &nbsp;<input type="hidden" id="hide" runat="server" /><input type="button" id="btn" value="Vérouiller mon choix"><asp:Button ID="submitReserv" runat="server" Text="Confirmer ma réservation" Onclick="submitReserv_Click" />

    
      
    &nbsp;

    <script>

        const accordion = document.querySelector('.accordion');
        const items = accordion.querySelectorAll('li');
        const questions = accordion.querySelectorAll('.question');

        function toggleAccordion() {
            const thisItem = this.parentNode;

            items.forEach(item => {
                if (thisItem == item) {
                    thisItem.classList.toggle('open');
                    return;
                }

                item.classList.remove('open');
            });
        }

        questions.forEach(question => question.addEventListener('click', toggleAccordion));

        const btn = document.querySelector('#btn');
        // handle click button
        btn.onclick = function () {
            const rbs = document.querySelectorAll('input[name="ReservRequire"]');
            let selectedValue;
            for (const rb of rbs) {
                if (rb.checked) {
                    selectedValue = rb.value;
                    break;
                }
            }
            var msg = selectedValue;
            alert(msg);
            var hiddenControl = '<%= hide.ClientID %>';
            document.getElementById(hiddenControl).value = msg;
        };
       

        

        
            
        







    </script>
            </div>
        </div>
    </div>

</asp:Content>
