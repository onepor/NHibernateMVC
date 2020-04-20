<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TabieUsingEdit.aspx.cs" Inherits="KZKJ.IOA.Web.TabieUsingEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true"
                            DataKeyNames="ID,TypeName" AllowSorting="true" OnSort="Grid2_Sort" SortField="TypeName"
                            SortDirection="DESC" AllowPaging="false" IsDatabasePaging="false"
                             OnPageIndexChange="Grid2_PageIndexChange">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnSave" Icon="TableSave" runat="server" Text="保存" OnClick="btnSave_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <PageItems>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </f:ToolbarSeparator>
                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                </f:ToolbarText>
                                <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged" runat="server">
                                    <f:ListItem Text="10" Value="10"></f:ListItem>
                                    <f:ListItem Text="20" Value="20"></f:ListItem>
                                    <f:ListItem Text="50" Value="50"></f:ListItem>
                                    <f:ListItem Text="100" Value="100"></f:ListItem>
                                </f:DropDownList>
                            </PageItems>
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="DishesCode" SortField="DishesCode" Width="80px" HeaderText="菜品编码"></f:BoundField>
                                <f:BoundField DataField="DishesName" SortField="DishesName" Width="130px"  HeaderText="菜品名称"></f:BoundField>
                                <f:BoundField DataField="DishesPY" SortField="DishesPY" Width="90px" HeaderText="菜品拼音"></f:BoundField>
                                <f:TemplateField Width="80px" HeaderText="菜品类别">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetClassName(Eval("ClassID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="60px" HeaderText="启用">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="SellPrice" SortField="SellPrice" Width="100px" HeaderText="售价(元)"></f:BoundField>
                                <f:BoundField DataField="MemberPrice" SortField="MemberPrice" Width="100px" HeaderText="会员价(元)"></f:BoundField>
                                <f:BoundField DataField="ShowIndex" SortField="ShowIndex" Width="60px" HeaderText="排序"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
