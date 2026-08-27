<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="CMNirdesh.report" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" AsyncRendering="false" ProcessingMode="Local" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
