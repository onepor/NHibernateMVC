<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="config.aspx.cs" Inherits="ZAJCZN.MIS.Web.admin.config" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowHeader="false" ShowBorder="false"
            BodyPadding="5px" runat="server">
            <Items>
                <f:SimpleForm ID="SimpleForm1" runat="server" LabelWidth="120px" BodyPadding="5px"
                    Width="600px" LabelAlign="Top" ShowBorder="false"
                    ShowHeader="false">
                    <Items>
                        <f:TextBox ID="tbxTitle" runat="server" Label="网站标题" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <%--<f:DropDownList ID="ddlTheme" Label="网站主题" runat="server" Required="true" ShowRedStar="true">
                            <f:ListItem Text="海卫一（Triton）" Value="Triton" />
                            <f:ListItem Text="小清新（Crisp）" Value="Crisp" />
                            <f:ListItem Text="海王星（Neptune）" Selected="true" Value="Neptune" />
                            <f:ListItem Text="蓝色（Blue）" Value="Blue" />
                            <f:ListItem Text="灰色（Gray）" Value="Gray" />
                        </f:DropDownList>--%>
                        <f:TriggerBox ID="tbTheme" runat="server" Label="网站主题"
                            TriggerIcon="Search" EnablePostBack="false" EnableEdit="false"
                            Required="true" ShowRedStar="true">
                            <Listeners>
                                <f:Listener Event="triggerclick" Handler="onThemeTriggerClick" />
                            </Listeners>
                        </f:TriggerBox>
                        <f:DropDownList ID="ddlMenuType" Label="菜单样式" runat="server" Required="true" ShowRedStar="true">
                            <f:ListItem Text="树型菜单" Selected="true" Value="tree" />
                            <f:ListItem Text="手风琴菜单" Value="accordion" />
                        </f:DropDownList>
                        <f:NumberBox ID="nbxPageSize" runat="server" Label="表格默认记录数" Required="true" ShowRedStar="true">
                        </f:NumberBox>
                        <f:TextArea runat="server" ID="tbxHelpList" Height="320" Label="帮助下拉列表" Required="true"
                            ShowRedStar="true" Hidden="true">
                        </f:TextArea>
                        <f:Button ID="btnSave" runat="server" Icon="SystemSave" OnClick="btnSave_OnClick"
                            ValidateForms="SimpleForm1" ValidateTarget="Top" Text="保存设置">
                        </f:Button>
                    </Items>
                </f:SimpleForm>
                <f:Window ID="windowThemeRoller" Title="主题仓库" Hidden="true" EnableIFrame="true" ClearIFrameAfterClose="false"
                    runat="server" IsModal="true" Width="1000px" Height="600px" EnableClose="true" Target="Top"
                    EnableMaximize="true" EnableResize="true">
                </f:Window>
            </Items>
        </f:Panel>
    </form>
    <script>

        var windowThemeRollerClientID = '<%= windowThemeRoller.ClientID %>';
        var tbThemeClientID = '<%= tbTheme.ClientID %>';

        function onThemeTriggerClick(event) {
            var url = F.baseUrl + 'admin/themes.aspx?selected=' + encodeURIComponent(F(tbThemeClientID).getValue());
            F(windowThemeRollerClientID).show(url);
        }

        function returnFromThemeWindow(themeName) {
            F(tbThemeClientID).setValue(themeName);
        }

    </script>
</body>
</html>
