<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="HallREservation.Home" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>Name of the Organization:</td>
            <td>
                <asp:TextBox runat="server" ID="txtOrganization">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                    ControlToValidate="txtOrganization"
                    runat="server" ErrorMessage="Please enter value." />
            </td>
        </tr>
        <tr>
            <td>Phone Number:</td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPhone" ErrorMessage="Invalid Phone Number ! (Ex: (541) 754-3010)" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>Email Address:</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email ! (Ex:xyz@abc.com)" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>Credit Card:</td>
            <td>
                <asp:TextBox ID="txtCr" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCr" ErrorMessage="Invalid Credit Card Number ! (Ex: 378282246310005)" ValidationExpression="\b(?:3[47]\d|(?:4\d|5[1-5]|65)\d{2}|6011)\d{12}\b"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>Number of days to Reservation:</td>
            <td>
                <asp:TextBox runat="server" ID="txtResevation"></asp:TextBox>                
                <asp:RangeValidator ID="MinSizeRangeV" runat="server" ControlToValidate="txtResevation" ErrorMessage="Enter number between 1 and 5" Type="Integer" MinimumValue="1" MaximumValue="5"></asp:RangeValidator>
                <%--<asp:CompareValidator ID="cv" runat="server" ControlToValidate="txtResevation" Type="Integer"
   Operator="DataTypeCheck" ErrorMessage="Value must be an integer!" />--%>
       </td>

            </td>
        </tr>
        <tr><td>Start Date:</td><td>
            <asp:TextBox ID="txtCalendar" ClientIDMode="Static" runat="server" ></asp:TextBox> 

                                </td></tr>
        <tr>
            <td>Rooms:</td>
            <td>
                <asp:DropDownList runat="server" ID="drpRooms">
                    <asp:ListItem Text="-Select Room-" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Ballroom A" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Ballroom B" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Conference Room A" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Conference Room B" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Board Meeting Room" Value="5"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Non Profit Ogranization:</td>
            <td>
                <asp:CheckBox runat="server" ID="chknonProfitOrg" /></td>
        </tr>
        <tr><td></td><td>
            <asp:Button ID="btnCal" runat="server" Text="Complete Reservation" OnClick="btnCal_Click" />
            <asp:Button ID="btnClr" runat="server" Text="Clear" OnClick="btnClr_Click" />
                                                                            </td></tr>
        <tr><td colspan="2"><asp:Label runat="server" ID="lblStatus"></asp:Label></td></tr>
        <tr>
            <td colspan="2">
                <asp:AdRotator ID="AdRotator1" runat="server" AdvertisementFile="~/adfile.xml" BorderStyle="None" Height="150px" Target="_blank" Width="400px" />
                            </td>
        </tr>
    </table>
 
    <script>
        $("#txtCalendar").datepicker({
            beforeShowDay: $.datepicker.noWeekends
        });
    </script>
</asp:Content>
