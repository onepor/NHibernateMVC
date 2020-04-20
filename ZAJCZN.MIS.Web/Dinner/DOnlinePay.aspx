<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DOnlinePay.aspx.cs" Inherits="ZAJCZN.MIS.Web.DOnlinePay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel runat="server" ID="Panel1" ShowHeader="false">
            <Toolbars>
                <f:Toolbar Position="Bottom" ToolbarAlign="Center" runat="server">
                    <Items>
                        <f:Button runat="server" Width="185px" Height="40px" CssStyle="font-size:19px" ID="btnAccept" Icon="Accept" OnClick="btnAccept_Click" Text="确 认"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TextBox MarginTop="13px" FocusOnPageLoad="true" LabelWidth="90px" LabelAlign="Right" MarginRight="5px" runat="server" ID="tbxBarcode" Label="条码"></f:TextBox>
            </Items>
        </f:Panel>
        <f:Timer ID="Timer1" Interval="5" Enabled="false" OnTick="Timer1_Tick" EnableAjax="false" runat="server">
        </f:Timer>
        <f:HiddenField ID="hfdout_trade_no" runat="server"></f:HiddenField>
    </form>
    <script>
        var tbxBarcode ='<%= tbxBarcode.ClientID %>'

        $(document).on("click", "td", function () {
            F(tbxBarcode).focus(true, 300);
        })

    </script>
</body>
</html>
