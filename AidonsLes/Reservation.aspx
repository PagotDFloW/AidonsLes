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
    <h2 style="text-align: center; font-family: Arial, Helvetica, sans-serif">Réservation des spots</h2>
    
        
    

    <div runat="server" id="generateSpot">

    </div>

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
    </script>



</asp:Content>
