<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="AidonsLes.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Conctatez nous si vous avez des questions.</h3>
    <address>
        QG Association Aidons-Les <br />
        2 Rue de la réussite 972000 NulPart sur Seine<br />
        <abbr title="Phone">P:</abbr>
        0612345678
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@example.com">Support@aidonsles.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@aidonsles.com</a>
    </address>
</asp:Content>
