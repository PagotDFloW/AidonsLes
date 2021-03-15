<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AidonsLes._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Aidons-Les</h1>
        <p class="lead">Bienvenue sur le site de réservation de l&#39;assiociation Aidons-Les</p>
        <p><a href="Account/Register" class="btn btn-primary btn-lg">S'inscrire&raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Qu&#39;est-ce que ce site</h2>
            <p>
                Cher membre de l&#39;association, ce site vous permettra de réserver un endroit où vous pourrer effectuer votre démarchage.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Vous vous êtes trompé de site?</h2>
            <p>
                Pas de soucis, on vous renvoie vers le site &quot;officiel&quot; de notre association</p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Redirection &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Reste ici si tu veux nous rejoindre l&#39;intrus</h2>
            <p>
                Clique sur le bouton pour lancer ton inscription, l&#39;équipe RH s&#39;occupera de ton dossier.</p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Rejoindre &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
