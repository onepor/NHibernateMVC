<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinnerTabieUsingManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.DinnerTabieUsingManager" %>

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
            ShowHeader="false" Title="餐区管理">
            <Items>
                <f:GroupPanel ID="groupTabie" runat="server" Title="餐台信息">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:DatePicker ID="dltStart" runat="server" Label="开台时间" LabelWidth="70"
                                    DateFormatString="yyyy-MM-dd HH:mm:ss" AutoPostBack="true"
                                    OnTextChanged="dltStart_TextChanged" Width="230px">
                                </f:DatePicker>
                                <f:TextBox ID="tbxPeople" Width="130px" LabelWidth="70px" EnableBlurEvent="true"
                                    OnBlur="tbxPeople_Blur" runat="server" Label="就餐人数">
                                </f:TextBox>
                                <f:TextBox ID="tbxVipID" LabelWidth="70px" EnableBlurEvent="true" OnBlur="tbxVipID_Blur"
                                    runat="server" Label="会员卡号" Width="200px">
                                </f:TextBox>
                                <f:DropDownList ID="ddlGroup" Label="团购套餐" LabelWidth="70px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" Width="200px"
                                    runat="server" DataTextField="SetMealName" DataValueField="PreferentialPrice">
                                </f:DropDownList>
                                <f:TextBox ID="tbGroup" LabelWidth="70px" EnableBlurEvent="true" OnBlur="tbGroup_Blur"
                                    runat="server" Label="团购券号" Width="200px">
                                </f:TextBox>
                                <f:ToolbarFill runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" Width="100px" Height="35px" CssStyle="font-size:17px" OnClick="btnReturn_Click" runat="server" Icon="bulletleft" Text="返回餐台"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Form ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel runat="server" Title="点菜详情">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" 
                            EnableCheckBoxSelect="true" EnableCollapse="true" AllowCellEditing="true" ClicksToEdit="2"
                            DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="DishesID" Height="400px"
                            SortDirection="ASC" AllowPaging="false" IsDatabasePaging="true" EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button Width="80px" Height="35px" CssStyle="font-size:17px" ID="btnNew" runat="server" Icon="BasketAdd" OnClick="btnNew_Click" Text="点 菜">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                        </f:ToolbarSeparator>
                                        <f:Button Width="80px" Height="35px" CssStyle="font-size:17px" ID="btnBack" runat="server" Icon="BasketDelete" OnClick="btnBack_Click" Text="退 菜">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                        </f:ToolbarSeparator>
                                        <f:Button ID="btnReady" runat="server" Width="80px" Height="35px" CssStyle="font-size:17px" Icon="Bell" EnablePostBack="true" OnClick="btnReady_Click" Text="叫  菜">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                        </f:ToolbarSeparator>
                                        <f:Button ID="btnGo" runat="server" Width="80px" Height="35px" CssStyle="font-size:17px" Icon="BellGo" EnablePostBack="true" OnClick="btnGo_Click" Text="走  菜">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                        </f:ToolbarSeparator>
                                        <f:Button ID="Button1" Width="126px" Height="35px" CssStyle="font-size:17px;margin-left:5xp" Icon="Pencil" EnablePostBack="false" runat="server"
                                            Text="菜品赠送">
                                            <Menu ID="Menu1" runat="server">
                                                <f:MenuButton ID="MenuButton1" Margin="5px 0" OnClick="btnEnableIsFree_Click" runat="server" Text="不赠送">
                                                </f:MenuButton>
                                                <f:MenuButton ID="MenuButton2" Margin="5px 0" OnClick="btnDisableIsFree_Click" runat="server" Text="赠送">
                                                </f:MenuButton>
                                            </Menu>
                                        </f:Button><f:Button ID="Button2" Width="126px" Height="35px" CssStyle="font-size:17px;margin-left:5xp" Icon="Pencil" EnablePostBack="false" runat="server"
                                            Text="套餐菜品">
                                            <Menu ID="Menu2" runat="server">
                                                <f:MenuButton ID="btnEnableGroup" Margin="5px 0" OnClick="btnEnableGroup_Click" runat="server" Text="团购套餐">
                                                </f:MenuButton>
                                                <f:MenuButton ID="btnDisableGroup" Margin="5px 0" OnClick="btnDisableGroup_Click" runat="server" Text="非团购">
                                                </f:MenuButton>
                                            </Menu>
                                        </f:Button>
                                        <f:ToolbarFill runat="server"></f:ToolbarFill>
                                        <f:Label ID="labMoneys" CssStyle="font-size:17px;color: red" LabelWidth="135px" LabelAlign="Right" runat="server" Label="金额小计"></f:Label>
                                        <f:Button ID="btnPay" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="Money" runat="server" Text="付 款"></f:Button>
                                        <f:Button ID="btnPrint" Width="100px" Height="35px" CssStyle="font-size:17px" OnClick="btnPrint_Click" runat="server" Icon="printer" Text="打 单"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <PageItems>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </f:ToolbarSeparator>
                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                </f:ToolbarText>
                                <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                                    runat="server">
                                    <f:ListItem Text="10" Value="10" />
                                    <f:ListItem Text="20" Value="20" />
                                    <f:ListItem Text="50" Value="50" />
                                    <f:ListItem Text="100" Value="100" />
                                </f:DropDownList>
                            </PageItems>
                            <Columns>
                                <f:RowNumberField TextAlign="Center" Width="35px" EnablePagingNumber="true" />
                                <f:TemplateField Width="200px" HeaderText="菜品名称">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# GetDishesNmae(Eval("DishesID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:RenderField Width="100px" ColumnID="DishesCount" DataField="DishesCount" FieldType="Double"
                                    HeaderText="菜品数量">
                                    <Editor>
                                        <f:NumberBox ID="numDishesCount" NoNegative="true" MinValue="1"
                                            MaxValue="5000" runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="Price" SortField="Price" Width="100px" HeaderText="菜品单价" />
                                <f:BoundField DataField="Moneys" SortField="Moneys" Width="100px" HeaderText="小计" />
                                <f:TemplateField Width="100px" HeaderText="菜品类型">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# GetDishesType(Eval("DishesType").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="100px" HeaderText="赠送菜品">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# GetDishesFree(Eval("IsFree").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:Window ID="WindowSelectRetireCount" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="240px"
            Height="200px" OnClose="WindowSelectRetireCount_Close">
        </f:Window>
        <f:Window ID="WindowPay" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="400px"
            Height="560px" OnClose="btnReturn_Click">
        </f:Window>
    </form>
</body>
</html>
