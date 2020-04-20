<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreConfig.aspx.cs" Inherits="ZAJCZN.MIS.Web.admin.StoreConfig" %>

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
                        <f:TextBox ID="tbxTitle" runat="server" Label="店铺名称" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxAddress" runat="server" Label="店铺地址" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxContractPhone" runat="server" Label="联系电话" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxInstallPhone" runat="server" Label="安装售后" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxDesignPhone" runat="server" Label="量尺设计" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <f:TextBox ID="tbxComplaintPhone" runat="server" Label="投诉电话" Required="true" ShowRedStar="true">
                        </f:TextBox>
                        <f:Button ID="btnSave" runat="server" Icon="SystemSave" OnClick="btnSave_OnClick"
                            ValidateForms="SimpleForm1" ValidateTarget="Top" Text="保存设置">
                        </f:Button>
                    </Items>
                </f:SimpleForm>

            </Items>
        </f:Panel>
    </form>

</body>
</html>
