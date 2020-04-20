<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractPayPrint.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractPayPrint" %>

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
            font-size: 14px;
        }

        th {
            border-right-color: black;
            border-left-color: white;
            border-top-color: white;
            border-bottom-color: white;
            border-width: 1px;
            border-style: solid;
            text-align: center;
            font-size: 16px;
        }

        tr {
            border-bottom-style: solid;
            border-bottom-width: 2px;
            border-bottom-color: black;
            border-left-style: solid;
            border-left-width: 2px;
            border-left-color: black;
            text-align: left;
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
                    <th style="width: 100%; font-size: x-large; padding-bottom: 30px; border-right-color: white;" colspan="10">重庆喜莱克家具有限公司（家喜林门）收款单</th>
                </tr>
                <tr style="border-top-style: solid; border-top-width: 2px; border-top-color: black;">
                    <td style="width: 100px;">供货方</td>
                    <td style="width: 100px; text-align: left;"><%= GetOrderInfo("2")%></td>
                    <td style="width: 100px;">联系电话</td>
                    <td style="width: 100px; text-align: left;"><%= GetOrderInfo("3")%></td>
                    <td style="width: 120px;">地址</td>
                    <td style="width: 100px; text-align: left;"><%= GetOrderInfo("1")%></td>
                </tr>
                <tr>
                    <td style="width: 100px;">客户信息</td>
                    <td style="width: 100px; text-align: left;" colspan="5"><%= GetPayInfo("1")%></td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 35px;"></td>
                </tr>
                <tr>
                    <th>序号</th>
                    <th>收款方式</th>
                    <th>金额</th>
                    <th colspan="3">备注</th>
                </tr>
                <tr>
                    <td>1</td>
                    <td><%= GetPayInfo("4")%></td>
                    <td><%= GetPayInfo("5")%></td>
                    <td colspan="3"><%= GetPayInfo("6")%></td>
                </tr>
                <tr style="height: 80px; border: none;">
                    <th style="width: 100px; text-align: right; border-color: white;" colspan="5"><%= GetPayInfo("2")%></th>
                    <th style="width: 100px; text-align: center; border-color: white;"><%= GetPayInfo("3")%></th>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
