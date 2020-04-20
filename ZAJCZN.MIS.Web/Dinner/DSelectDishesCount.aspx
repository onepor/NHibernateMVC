<%@ Page AutoEventWireup="true" CodeBehind="DSelectDishesCount.aspx.cs" Inherits="ZAJCZN.MIS.Web.DSelectDishesCount" Language="C#" %>

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
            ShowHeader="false" Title="就餐人数">
            <Items>
                <f:Form runat="server" ShowBorder="false" ShowHeader="false">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:Button runat="server" Width="60px" Height="45px" ID="btnDelete" Icon="delete" EnablePostBack="true" OnClick="btnDelete_Click"></f:Button>
                                <f:NumberBox Width="100px" Height="50px" runat="server" ID="numCount"></f:NumberBox>
                                <f:Button runat="server" Width="68px" Height="45px" ID="btnAdd" Icon="Add" EnablePostBack="true" OnClick="btnAdd_Click"></f:Button>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:Button runat="server" Width="95px" Height="45px" ID="btnClose" Icon="systemclose" Text="取消"></f:Button>
                                <f:Button runat="server" Width="100px" Height="45px" ID="btnSave" Icon="accept" OnClick="btnSave_Click" Text="确定"></f:Button>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
