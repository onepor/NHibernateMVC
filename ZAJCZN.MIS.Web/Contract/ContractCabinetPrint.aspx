<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractCabinetPrint.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractCabinetPrint" %>

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
            <table>
                <tr style="border: none;">
                    <th style="width: 100%; font-size: x-large; padding-bottom: 30px; border-right-color: white;" colspan="10">重庆喜莱克家具有限公司（家喜林门）定制家具收费明细表</th>
                </tr>
                <tr style="border-top-style: solid; border-top-width: 2px; border-top-color: black;">
                    <td style="width: 100px;" colspan="2">订单号</td>
                    <td style="width: 100px; text-align: left;"><%= GetOrderInfo("1")%></td>
                    <td style="width: 120px;">供货单位</td>
                    <td style="width: 100px; text-align: left;" colspan="3"><%= GetOrderInfo("2")%></td>
                    <td style="width: 120px;">联系电话</td>
                    <td style="width: 100px; text-align: left;" colspan="2"><%= GetOrderInfo("3")%></td>
                </tr>
                <tr>
                    <td style="width: 100px;" colspan="2">客户地址</td>
                    <td style="width: 100px; text-align: left;" colspan="5"><%= GetOrderInfo("4")%></td>
                    <td style="width: 100px;">联系电话</td>
                    <td style="width: 100px; text-align: left;" colspan="2"><%= GetOrderInfo("5")%></td>
                </tr>
                <tr>
                    <th>序号</th>
                    <th>房间位置及柜体名称</th>
                    <th>产品名称</th>
                    <th>高度(m)</th>
                    <th>宽度(m)</th>
                    <th>数量(块/米)</th>
                    <th>面积(㎡/米)</th>
                    <th>单价（元）</th>
                    <th>金额（元）</th>
                    <th>备注</th>
                </tr>
                <%=StrHtml %>
                <tr>
                    <th style="font-size: 16px; text-align: right;" colspan="8">合计：</th>
                    <td><%= GetOrderInfo("7")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 100px;" colspan="17"><%= GetOrderInfo("6")%></td>
                </tr>
                <tr style="border: none;">
                    <td style="width: 100px; padding-top: 30px; text-align: right;border-right-color: white;" colspan="5">客户确认签字</td>
                    <td style="width: 100px; padding-top: 30px; border-right-color: white;" colspan="5">货方签字（盖章）</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
