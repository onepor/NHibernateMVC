<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DTabieChange.aspx.cs" Inherits="ZAJCZN.MIS.Web.DTabieChange" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="转台">
            <Items>
                <f:GroupPanel ID="GPvip" Title="转台信息" runat="server">
                    <Items>
                        <f:Label runat="server" ID="lblTabie" Label="原餐台"></f:Label>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPPayWay" Title="转台餐台选择" runat="server">
                    <Items>
                        <f:DropDownList ID="ddlTabie" Label="转台餐台" LabelWidth="100px" runat="server"
                            DataTextField="TabieName" DataValueField="ID">
                        </f:DropDownList>
                    </Items>
                </f:GroupPanel>
                <f:Form runat="server" ShowBorder="false" ShowHeader="false" MarginTop="5px">
                    <Toolbars>
                        <f:Toolbar ToolbarAlign="Center" Position="Bottom" runat="server">
                            <Items>
                                <f:Button ID="btnSave" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="accept"
                                    runat="server" Text="确  认" OnClick="btnSave_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
