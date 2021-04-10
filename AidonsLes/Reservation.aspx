<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reservation.aspx.cs" Inherits="AidonsLes.Reservation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
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
    <h2 style="text-align: center; font-family: Arial, Helvetica, sans-serif">Réservation des spots&nbsp;&nbsp; </h2>
    <div class="selecDate"><label>Sélectionner votre date de réservation : </label>
        <input type="date" id="dateReserv" name="dateReserv"  /><asp:Button ID="submit" runat="server" OnClick="submit_Click1" Text="Valider" />
        
        <br />
        </div>    
        
    

    <div runat="server" id="generateSpot">
    
    </div>
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
            alert("Vous avez vérouillé votre créneau à réserver");
            var hiddenControl = '<%= hide.ClientID %>';
            document.getElementById(hiddenControl).value = msg;
        };
       

        

        
            
        







    </script>



</asp:Content>
