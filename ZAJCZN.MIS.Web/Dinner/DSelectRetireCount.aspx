<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DSelectRetireCount.aspx.cs" Inherits="ZAJCZN.MIS.Web.DSelectRetireCount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="退菜数量">
            <Items>
                <f:Form runat="server" ShowBorder="false" ShowHeader="false">
                    <Rows>
                        <f:FormRow MarginLeft="10px">
                            <Items>
                                <f:Label ID="lblName" runat="server" CssStyle="font-size:18px" ShowLabel="false"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow MarginTop="6px" MarginLeft="10px">
                            <Items>
                                <f:Button runat="server" Width="60px" Height="45px" ID="btnDelete" Icon="delete" EnablePostBack="true" OnClick="btnDelete_Click"></f:Button>
                                <f:NumberBox Width="100px" Height="50px" runat="server" ID="numCount"></f:NumberBox>
                                <f:Button runat="server" Width="68px" Height="45px" ID="btnAdd" Icon="Add" EnablePostBack="true" OnClick="btnAdd_Click"></f:Button>
                            </Items>
                        </f:FormRow>
                        <f:FormRow MarginTop="6px" MarginLeft="10px">
                            <Items>
                                <f:Button runat="server" Width="100px" Height="50px" CssStyle="font-size:17px" ID="btnClose" Icon="systemclose" Text="取 消"></f:Button>
                                <f:Button runat="server" Width="100px" Height="50px" CssStyle="font-size:17px" ID="btnSave" Icon="accept" OnClick="btnSave_Click" Text="确 定"></f:Button>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
