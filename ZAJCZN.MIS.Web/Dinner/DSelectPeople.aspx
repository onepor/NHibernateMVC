<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DSelectPeople.aspx.cs" Inherits="ZAJCZN.MIS.Web.DSelectPeople" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel2" CssClass="blockpanel" BoxConfigAlign="Stretch" BoxConfigPosition="Center" runat="server" ShowBorder="false" EnableCollapse="false"
            Layout="HBox" BodyPadding="10px" ShowHeader="false"
            Title="面板">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Center" Position="Bottom">
                    <Items>
                        <f:Button runat="server" Width="70px" Height="45px" ID="btnClose" Icon="systemclose" OnClick="btnClose_Click" Text="取消"></f:Button>
                        <f:Button runat="server" Width="70px" Height="45px" ID="btnSave" Icon="accept" OnClick="btnSave_Click" Text="确定"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel1" Title="就餐基本信息" BoxFlex="1" runat="server"
                    ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:Label runat="server"></f:Label>
                        <f:TextBox FocusOnPageLoad="true" runat="server" LabelWidth="70px" Label="就餐人数" ID="tbxPeople"></f:TextBox>
                        <f:Label runat="server"></f:Label>
                        <f:TextBox runat="server" LabelWidth="70px" Label="会员卡号" ID="tbxVipCard"></f:TextBox>
                    </Items>
                </f:Panel>
                <f:Panel ID="Panel4" Title="软键盘" BoxFlex="1" runat="server"
                    BodyPadding="0 0 0 12px" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:Form runat="server" ShowBorder="false" ShowHeader="false">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="1" ID="Button1"></f:Button>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="2" ID="Button2"></f:Button>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="3" ID="Button3"></f:Button>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="4" ID="Button4"></f:Button>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="5" ID="Button5"></f:Button>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="6" ID="Button6"></f:Button>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="7" ID="Button7"></f:Button>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="8" ID="Button8"></f:Button>
                                        <f:Button runat="server" Width="52px" Height="52px" Text="9" ID="Button9"></f:Button>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Button runat="server" Width="80.5px" Height="52px" Text="0" ID="Button10"></f:Button>
                                        <f:Button runat="server" Width="80.5px" Height="52px" Text="←" ID="Button11"></f:Button>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
        <f:HiddenField runat="server" ID="hfdFocus"></f:HiddenField>
    </form>
    <script>
        var rjp ='<%= Panel4.ClientID %>';
        var hfdFocus = '<%= hfdFocus.ClientID %>';
        var tbxPeople = '<%= tbxPeople.ClientID %>';
        var tbxVipCard = '<%= tbxVipCard.ClientID %>';

        $(document).on("focus", "#" + tbxPeople, function () {
            F(hfdFocus).setValue(tbxPeople);
        });

        $(document).on("focus", "#" + tbxVipCard, function () {
            F(hfdFocus).setValue(tbxVipCard);
        });

        $(document).on("click", "#" + rjp + " a[role='button']", function () {
            var text = F(F(hfdFocus).getValue()).getValue();
            var number = $(this).find(".f-btn-text").html();
            if (number != "←")
                F(F(hfdFocus).getValue()).setValue(text + number);
            else {

                F(F(hfdFocus).getValue()).setValue(text.slice(0,-1));
            }
            $("#" + F(hfdFocus).getValue() + "-inputEl").focus();
        });

        F.ready(function () {
            $("div[id^='fineui'][style$='left: 62px;']").css("left", "55px");
            $("div[id^='fineui'][style$='left: 124px;']").css("left", "110px");
            $("div[id^='fineui'][style$='left: 94px;']").css("left", "82.5px");
        });
    </script>
</body>
</html>
