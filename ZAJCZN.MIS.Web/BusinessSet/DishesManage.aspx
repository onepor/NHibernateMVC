<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DishesManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.DishesManage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Width="140px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                            DataKeyNames="ID" AllowSorting="false" SortField="TypeName" SortDirection="DESC"
                            AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                            <Columns>
                                <f:BoundField DataField="ClassName" HeaderText="菜品类别" Width="140px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Form ID="Form3" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                            <Rows>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:TwinTriggerBox ID="ttbSearchUser" runat="server" ShowLabel="false" EmptyText="在菜品名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchUser_Trigger2Click" OnTrigger1Click="ttbSearchUser_Trigger1Click">
                                        </f:TwinTriggerBox>
                                        <f:Label runat="server">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                        <f:Grid ID="Grid2" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true"
                            DataKeyNames="ID,TypeName" AllowSorting="true" OnSort="Grid2_Sort" SortField="TypeName"
                            SortDirection="DESC" AllowPaging="false" IsDatabasePaging="false"
                            OnPreDataBound="Grid2_PreDataBound" OnRowCommand="Grid2_RowCommand" OnPageIndexChange="Grid2_PageIndexChange">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnDeleteSelected" Icon="Delete" Hidden="true" runat="server" Text="移除选中菜品" OnClick="btnDeleteSelected_Click">
                                        </f:Button>
                                        <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                            Text="设置启用状态">
                                            <Menu ID="Menu1" runat="server">
                                                <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                                </f:MenuButton>
                                                <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server" Text="停用选中记录">
                                                </f:MenuButton>
                                            </Menu>
                                        </f:Button>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" OnClick="btnNew_Click" Text="新增菜品">
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
                                <f:RowNumberField Hidden="true"></f:RowNumberField>
                                <%--<f:ImageField DataImageUrlField="DishesPicture" Width="140px" ImageWidth="100px" ImageHeight="75px" HeaderText="菜品图片"></f:ImageField>--%>
                                <f:BoundField DataField="DishesCode" SortField="DishesCode" Width="80px" HeaderText="菜品编码"></f:BoundField>
                                <f:BoundField DataField="DishesName" SortField="DishesName" Width="130px" HeaderText="菜品名称"></f:BoundField>
                                <f:BoundField DataField="DishesPY" SortField="DishesPY" Width="90px" HeaderText="菜品拼音"></f:BoundField>
                                <f:TemplateField Width="80px" HeaderText="菜品类别">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetClassName(Eval("ClassID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="100px" HeaderText="默认打印机">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetPrintName(Eval("PrinterID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="60px" HeaderText="启用">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="SellPrice" SortField="SellPrice" Width="90px" HeaderText="售价(元)"></f:BoundField>
                                <f:BoundField DataField="MemberPrice" SortField="MemberPrice" Width="90px" HeaderText="会员价(元)"></f:BoundField>
                                <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑菜品"
                                    WindowID="Window1" Title="编辑菜品" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/BusinessSet/DishesEdit.aspx?id={0}"
                                    Width="60px" />
                                <f:LinkButtonField HeaderText="配料" TextAlign="Center" Icon="Basket" ToolTip="查看菜品配料明细"
                                    CommandName="Basket" Width="60px" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="从当前记录中移除此菜品" Hidden="true"
                                    ConfirmText="确定移除此菜品？" ConfirmTarget="Top" CommandName="Delete" Width="50px">
                                </f:LinkButtonField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true"
            Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true"
            EnableIFrame="true" IFrameUrl="about:blank" Width="700px" Height="600px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
