<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinnerTabieManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.DinnerTabieManager" %>

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
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Width="140px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                            DataKeyNames="ID" AllowSorting="false" SortField="TypeName" SortDirection="DESC"
                            AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                            <Columns>
                                <f:TemplateField Width="170px" HeaderText="餐区">
                                    <ItemTemplate>
                                        <f:Label ID="Label1" runat="server" CssStyle="font-size:20px;margin-top: 6px" Text='<%# Eval("AreaName").ToString() %>'></f:Label>
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
                                <f:Button ID="btnAdd" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="DoorOpen" runat="server" Text="开  台" OnClientClick="GetIds()" OnClick="btnAdd_Click"></f:Button>
                                <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                                <f:Button ID="btnChange" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="ArrowRefresh" runat="server" Text="转  台" OnClick="btnChange_Click" OnClientClick="GetIds()"></f:Button>
                                <%--<f:Button ID="btnDelete" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="Delete" runat="server" Text="清  台" OnClick="btnDelete_Click" OnClientClick="GetIds()"></f:Button>--%>
                                <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                                <f:Button ID="btnMontage" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="Monitor" runat="server" Text="合并付款" OnClick="btnMontage_Click" OnClientClick="GetIds()"></f:Button>
                                <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                                <f:Button ID="btnContent" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="Information" runat="server" Text="餐台详情" OnClick="btnContent_Click" OnClientClick="GetIds()"></f:Button>
                                <f:ToolbarFill runat="server"></f:ToolbarFill>
                                <f:Button ID="btnDishes" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="BasketAdd" runat="server" Text="点  菜" OnClick="btnDishes_Click" OnClientClick="GetIds()"></f:Button>
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" Hidden="true"></f:ToolbarSeparator>
                                <f:Button ID="btnOut" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="BellGo" runat="server" Text="走  菜" Hidden="true" OnClick="btnOut_Click" OnClientClick="GetIds()"></f:Button>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                                <f:Button ID="btnCalc" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="Bin" runat="server" Text="交  班" OnClick="btnCalc_Click"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Timer ID="Timer1" Interval="10" OnTick="Timer1_Tick" EnableAjaxLoading="false" runat="server">
                        </f:Timer>
                        <f:ContentPanel ShowHeader="false" ShowBorder="false" ID="ContentPanel1" runat="server">
                            <div id="TabieS" runat="server"></div>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:HiddenField ID="Hiddenid" runat="server"></f:HiddenField>
        <f:Window ID="WindowSelectPeople" runat="server" IsModal="true" Hidden="true" EnableClose="false" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="410px"
            Height="330px" OnClose="WindowSelectPeople_Close">
        </f:Window>
        <f:Window ID="WindowTabieChange" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="260px"
            Height="250px" OnClose="WindowTabieChange_Close">
        </f:Window>
        <f:Window ID="WindowJiaoBan" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="600px" Height="600px">
        </f:Window>
        <f:Window ID="WindowPay" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="850px"
            Height="610px" OnClose="WindowPay_Close">
        </f:Window>
    </form>
    <script>
        var hiddenid = '<%= Hiddenid.ClientID %>';

        function GetIds() {

            var obj = $("input[type='checkbox']:checked");
            var IDs = "";
            obj.each(function () {
                IDs = IDs + $(this).val() + ",";
            });
            F(hiddenid).setValue(IDs);
        }


        $(document).on("click", "td", function () {
            var flag = $(this).find("input").prop("checked");
            if (flag) {
                $(this).find("input").removeAttr("checked");
                //$(this).removeClass("backgro");
            }
            else {
                $(this).find("input").prop("checked", true);
                //$(this).addClass("backgro");
            }
        })
    </script>
</body>
</html>
