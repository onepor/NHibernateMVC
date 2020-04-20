<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinnerOrderMealManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.DinnerOrderMealManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../res/css/tabies.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AjaxAspnetControls="TabieS" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:Panel runat="server" ShowHeader="false" ShowBorder="false">
            <Items>
                <f:Form runat="server" ShowHeader="false" ShowBorder="false">
                    <Items>
                        <f:FormRow>
                            <Items>
                                <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
                                    <Regions>
                                        <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Width="190px" Position="Left" Layout="Fit" runat="server">
                                            <Items>
                                                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                                                    DataKeyNames="ID" AllowSorting="false" SortField="TypeName" SortDirection="DESC"
                                                    AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                                                    <Columns>
                                                        <f:TemplateField Width="170px" HeaderText="菜品分类">
                                                            <ItemTemplate>
                                                                <f:Label runat="server" CssStyle="font-size:20px;margin-top: 6px" Text='<%# Eval("ClassName").ToString() %>'></f:Label>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                    </Columns>
                                                </f:Grid>
                                            </Items>
                                        </f:Region>
                                        <f:Region ID="Region2" CssStyle="blackground-color:#000000" ShowHeader="false" ShowBorder="false" BodyPadding="5px" Layout="Fit" runat="server">
                                            <Toolbars>
                                                <f:Toolbar runat="server">
                                                    <Items>
                                                        <f:Label runat="server" CssStyle="font-size:25px;color: red;" ID="labArea" ShowLabel="false"></f:Label>
                                                        <f:Button ID="btnReady" runat="server" Width="90px" Height="45px" CssStyle="font-size:17px" Icon="Bell" EnablePostBack="true" OnClick="btnReady_Click" Text="叫  菜">
                                                        </f:Button>
                                                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                                        </f:ToolbarSeparator>
                                                        <f:Button ID="btnGo" runat="server" Width="90px" Height="45px" CssStyle="font-size:17px" Icon="BellGo" EnablePostBack="true" OnClick="btnGo_Click" Text="走  菜">
                                                        </f:Button>
                                                        <f:ToolbarFill ID="ToolbarFill2" runat="server">
                                                        </f:ToolbarFill>
                                                        <f:Button ID="btnReturn" Width="110px" runat="server" Height="45px" CssStyle="font-size:20px" Icon="bulletleft" EnablePostBack="true" OnClick="btnReturn_Click" Text="返回餐台">
                                                        </f:Button>
                                                        <f:Button ID="Button1" Width="110px" Height="45px" CssStyle="font-size:20px" Icon="cartadd" runat="server" Text="浏览菜单" OnClick="btnUp_Click"></f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Items>
                                                <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                                                    <Regions>
                                                        <f:Region ID="Region3" ShowHeader="false" ShowBorder="false" Width="750px" Position="Left" BodyPadding="5px" Layout="Fit" runat="server">
                                                            <Items>
                                                                <f:ContentPanel ShowHeader="false" ShowBorder="false" ID="ContentPanel1" runat="server" >
                                                                    <div id="TabieS" runat="server"></div>
                                                                </f:ContentPanel>
                                                            </Items>
                                                        </f:Region>
                                                        <f:Region ID="Region4" ShowBorder="false" ShowHeader="false" Layout="Fit" runat="server">
                                                            <Items>
                                                                <f:Grid ID="Grid2" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                                                                    DataKeyNames="ID" OnRowDoubleClick="Grid2_RowDoubleClick" EnableRowDoubleClickEvent="true" AllowPaging="false" EnableMultiSelect="false">
                                                                    <Columns>
                                                                        <f:BoundField DataField="DishesName" SortField="DishesName" Width="130px" HeaderText="已点菜品" />
                                                                        <f:BoundField DataField="DishesCount" SortField="DishesCount" Width="54px" HeaderText="数量" />
                                                                    </Columns>
                                                                </f:Grid>
                                                            </Items>
                                                        </f:Region>
                                                    </Regions>
                                                </f:RegionPanel>
                                            </Items>
                                        </f:Region>
                                    </Regions>
                                </f:RegionPanel>
                            </Items>
                        </f:FormRow>
                    </Items>
                </f:Form>
            </Items>
        </f:Panel>
        <f:HiddenField ID="Hiddenid" runat="server"></f:HiddenField>
        <f:Window ID="WindowSelectCount" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="220px"
            Height="150px" EnableClose="false" OnClose="WindowSelectCount_Close">
        </f:Window>
    </form>
    <script>
        var hiddenid = '<%= Hiddenid.ClientID %>';
        var TabieS = '<%= TabieS.ClientID %>';

        $(document).on("click", "#" + TabieS + " td", function () {
            var using = F(hiddenid).getValue();
            var id = $(this).find("input").attr("value");
            $(this).find("input").prop("checked", true);
            F('WindowSelectCount').show('/Dinner/DSelectDishesCount.aspx?id=' + id + '&usingid=' + using, '菜品数量');
            return false;
        })
    </script>
</body>
</html>
