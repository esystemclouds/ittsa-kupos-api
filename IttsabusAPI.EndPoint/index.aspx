<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="IttsabusAPI.EndPoint.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main>
        <div class="container bg-warning" >
            <div class="container p-3">
                <h1>EndPoint</h1>
                <h3>Kupos - IttSabus v1.0</h3>
                <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" OnClick="Test">LinkButton</asp:LinkButton>
            </div>
        </div>
        </main>
    </form>
</body>
</html>
