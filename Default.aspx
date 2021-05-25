<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UrlShortener._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron pnlMainHeader">
        <h1>URL Shortener</h1>
        <p>Input your URL and hit Enter</p>
        <asp:TextBox ID="txtInputUrl" runat="server" placeholder="Enter a URL to shorten" onkeypress="return UrlSubmitted(event, this);"></asp:TextBox>
        <asp:Panel ID="pnlResponse" runat="server" CssClass="pnlMainHeaderRow"></asp:Panel>
    </div>

    <div class="row">
        
    </div>


</asp:Content>
