<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPassRate.aspx.cs" Inherits="ZAJCZN.MIS.Web.ExamPassRate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" IsFluid="true" CssClass="blockpanel" ShowBorder="true" ShowHeader="true" Title="" runat="server" EnableCollapse="false"
                            DataKeyNames="Id" EnableColumnLines="true">
                            <Columns>

                                <f:BoundField Width="150px" TextAlign="Center" ColumnID="CityName" DataField="CityName" HeaderText="城市" />
                                <f:BoundField Width="150px" TextAlign="Center" ColumnID="SubjectName" DataField="SubjectName" HeaderText="科目" />
                                <f:GroupField HeaderText="平均分" TextAlign="Center">
                                    <Columns>
                                        <f:GroupField HeaderText="合计" TextAlign="Center">
                                            <Columns>
                                                <f:BoundField ExpandUnusedSpace="true" TextAlign="Center" DataField="TopAvgScore" HeaderText="18年" />
                                                <f:BoundField ExpandUnusedSpace="true" TextAlign="Center" DataField="NowAvgScore" HeaderText="19年" />
                                            </Columns>
                                        </f:GroupField>
                                        <f:GroupField HeaderText="" TextAlign="Center">
                                            <Columns>
                                                <%--<f:TemplateField ExpandUnusedSpace="true" TextAlign="Center" ColumnID="Gender" HeaderText="合格率">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# (Convert.ToDecimal(Eval("PassRate"))*100).ToString()+"%" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField ExpandUnusedSpace="true" TextAlign="Center" ColumnID="Gender" HeaderText="合格率">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# (Convert.ToDecimal(Eval("PassRate"))*100).ToString()+"%" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>--%>
                                                <f:BoundField ExpandUnusedSpace="true" TextAlign="Center" DataField="TopAvgScore" HeaderText="18年" />
                                                <f:BoundField ExpandUnusedSpace="true" TextAlign="Center" DataField="NowAvgScore" HeaderText="19年" />
                                            </Columns>
                                        </f:GroupField>
                                    </Columns>
                                </f:GroupField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
