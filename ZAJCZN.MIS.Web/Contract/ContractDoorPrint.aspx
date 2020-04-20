<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractDoorPrint.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractDoorPrint" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .ctrls {
            margin: 0 auto;
            width: 100%;
            text-align: right;
            padding: 20px 0;
        }


        .Button {
            width: 100px;
            font-size: 20px;
        }

        table {
            width: 100%;
            border-color: black;
            border-style: solid;
            border-width: 1px;
            border-collapse: collapse;
        }

        td {
            border-right-color: black;
            border-left-color: white;
            border-top-color: white;
            border-bottom-color: white;
            border-width: 1px;
            border-style: solid;
            text-align: center;
            font-size: 12px;
            height: 14px;
        }

        th {
            border-right-color: black;
            border-left-color: white;
            border-top-color: white;
            border-bottom-color: white;
            border-width: 1px;
            border-style: solid;
            text-align: center;
            font-size: 13px;
            height: 14px;
        }

        tr {
            border-bottom-style: solid;
            border-bottom-width: 2px;
            border-bottom-color: black;
            border-left-style: solid;
            border-left-width: 2px;
            border-left-color: black;
            text-align: left;
            height: 14px;
        }
    </style>
    <style type="text/css" media="print">
        .ctrls {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ctrls">
            <asp:Button ID="Button1" Text="打印" OnClick="btnPrinting" runat="server" CssClass="Button" />
        </div>
        <div>
            <asp:Repeater ID="rpInfoList" runat="server">
                <HeaderTemplate>
                    <table>
                        <tr style="border: none;">
                            <th style="width: 100%; font-size: x-large; padding-bottom: 20px; border-right-color: white;" colspan="19">重庆喜莱克家具有限公司（家喜林门）销售订单</th>
                        </tr>
                        <tr style="border-top-style: solid; border-top-width: 2px; border-top-color: black; height: 40px;">
                            <td style="width: 100px;" colspan="2">订单号</td>
                            <td style="width: 100px; text-align: left;" colspan="3"><%= GetOrderInfo("1")%></td>
                            <td style="width: 120px;">供货单位</td>
                            <td style="width: 100px; text-align: left;" colspan="5"><%= GetOrderInfo("2")%></td>
                            <td style="width: 120px;" colspan="2">联系电话</td>
                            <td style="width: 100px; text-align: left;" colspan="6"><%= GetOrderInfo("3")%></td>
                        </tr>
                        <tr style="height: 40px;">
                            <td style="width: 100px;" colspan="2">客户地址</td>
                            <td style="width: 100px; text-align: left;" colspan="9"><%= GetOrderInfo("4")%></td>
                            <td style="width: 100px;" colspan="2">联系电话</td>
                            <td style="width: 100px; text-align: left;" colspan="6"><%= GetOrderInfo("5")%></td>
                        </tr>
                        <tr>
                            <th>序号</th>
                            <th>位置</th>
                            <th>高</th>
                            <th>宽</th>
                            <th>厚</th>
                            <th>线条</th>
                            <th>材质</th>
                            <th>颜色</th>
                            <th>款式</th>
                            <th>商品名称</th>
                            <th>开启方向</th>
                            <th>玻璃款式</th>
                            <th>单位</th>
                            <th>单价</th>
                            <th>数量</th>
                            <th>商品价格</th>
                            <th>运输安装及五金</th>
                            <th>总价</th>
                            <th>备注</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="height: 30px;">
                        <td style="width: 80px;">
                            <%# Eval("ID") %>
                        </td>
                        <td style="width: 200px; text-align: left;"><%# Eval("GoodsLocation")%></td>
                        <td style="width: 80px;">
                            <%# Eval("GHeight")%>
                        </td>
                        <td style="width: 80px;">
                            <%# Eval("GWide")%>
                        </td>
                        <td style="width: 80px;">
                            <%# Eval("GThickness")%>
                        </td>
                        <td style="width: 120px;">
                            <%# Eval("LineName")%>
                        </td>
                        <td style="width: 250px;">
                            <%# Eval("TypeName")%>
                        </td>
                        <td style="width: 130px; text-align: left;"><%# Eval("DoorColor")%></td>
                        <td style="width: 180px; text-align: left;">
                            <%# Eval("ModelName") %>
                        </td>
                        <td style="width: 250px;"><%# Eval("GoodsName")%></td>
                        <td style="width: 160px;"><%# Eval("DoorDirection")%></td>
                        <td style="width: 200px;"><%# Eval("GlassRemark")%></td>
                        <td style="width: 80px;">
                            <%# Eval("GoodUnit")%>
                        </td>
                        <td style="width: 120px;">
                            <%# Eval("GPrice")%>
                        </td>
                        <td style="width: 120px;">
                            <%# Eval("OrderNumber")%>
                        </td>
                        <td style="width: 150px;">
                            <%# Eval("GoodsAmount")%>
                        </td>
                        <td style="width: 150px;">
                            <%# Eval("InstallCost")%>
                        </td>
                        <td style="width: 150px;">
                            <%# Eval("OrderAmount")%>
                        </td>
                        <td style="width: 300px; text-align: left;">
                            <%# Eval("Remark")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%# rpInfoList.Items.Count == 0 ? "没有选择打印数据" : ""%>
                    <tr>
                        <th style="font-size: 16px; text-align: right;" colspan="17">合计：</th>
                        <td><%= GetOrderInfo("7")%></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="width: 100px;" colspan="19"><%= GetOrderInfo("6")%></td>
                    </tr>
                    <tr style="border: none;">
                        <td style="width: 100px; padding-top: 30px; text-align: right; border-right-color: white;" colspan="10">客户确认签字</td>
                        <td style="width: 100px; padding-top: 30px; border-right-color: white;" colspan="9">货方签字（盖章）</td>
                    </tr>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
