<!--
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: AccessApproval.aspx
-->

<%@ Page Language="C#" AutoEventWireup="True" Codebehind="AccessApproval.aspx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.AccessApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DREAM - Access Request</title>

    <script type="text/javascript" src="/_Layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>

    <link href="/_Layouts/DREAM/Styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="0" cellpadding="4" cellspacing="0">
                <tr>
                    <td class="tdAccessHeader" valign="middle" width="5%">
                        <img src="/_layouts/DREAM/images/logo.gif" alt="" />
                    </td>
                    <td valign="middle" class="tdAccessHeader" align="left">
                        <asp:Label ID="lblRegionTitle" runat="server" Text=""></asp:Label>
                </tr>
                <tr>
                    <td colspan="2" class="AccessApproval1">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="100px">
                        <p>
                            &nbsp;</p>
                    </td>
                </tr>
                <tr runat="server" id="trAccessApproval">
                    <td colspan="2" align="center">
                        <br />
                        <table class="tableAdvSrchBorder" width="700" border="0" cellpadding="4" cellspacing="0">
                            <tr>
                                <td colspan="2" height="30px">
                                    <p>
                                        &nbsp;</p>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="5px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="5px">
                                    <asp:Panel ID="ExceptionPanel" Visible="false" runat="server">
                                        <asp:Label ID="lblException" runat="server" Text="" CssClass="labelMessage"></asp:Label>
                                    </asp:Panel>
                                    <table id="Table1" class="tableAdvSrchBorder" width="500px" height="100%" border="0"
                                        cellpadding="4" cellspacing="0" align="center" runat="server">
                                        <tr>
                                            <td colspan="2" align="center" class="AccessApprovalLogin">
                                                <b>
                                                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                                                    <asp:Label ID="lblTeamName" runat="server"></asp:Label></b></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="AccessApprovalLogin2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" height="5px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%" class="tdAccessData">
                                                Shell User A/c</td>
                                            <td width="70%" class="tdAccessData">
                                                <asp:TextBox ID="txtUserAcc" ReadOnly="true" runat="server" CssClass="textboxAdvSrch"
                                                    Width="145px"></asp:TextBox>
                                                <span class="spanColor">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%" class="tdAccessData">
                                                Shell Region</td>
                                            <td width="70%" class="tdAccessData">
                                                <asp:TextBox ID="txtRegion" ReadOnly="true" runat="server" CssClass="textboxAdvSrch"
                                                    Width="145px"></asp:TextBox>
                                                <span class="spanColor">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdAccessData" width="30%">
                                                Role</td>
                                            <td class="tdAccessData" width="70%">
                                                <asp:DropDownList ID="ddlRoles" runat="server" Width="145px">
                                                    <asp:ListItem>---Select---</asp:ListItem>
                                                    <asp:ListItem Value="NormalUser">Normal User</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="spanColor">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdAccessData" width="30%">
                                                Purpose</td>
                                            <td class="tdAccessData" width="70%">
                                                <asp:TextBox ID="txtPurpose" runat="server" CssClass="textboxAdvSrch" Width="145px"
                                                    TextMode="MultiLine"></asp:TextBox>
                                                <span class="spanColor">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdAccessData" colspan="2" style="font-weight: bold;">
                                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                &nbsp;</td>
                                            <td width="80%">
                                                <asp:Button ID="cmdSubmit" runat="server" CssClass="buttonAdvSrch" OnClientClick="return Validate()"
                                                    Text="Submit" Width="77px" OnClick="cmdSubmit_Click" />
                                                <asp:Button ID="cmdReset" runat="server" CssClass="buttonAdvSrch" OnClientClick="return ResetFields()"
                                                    Text="Reset" Width="77px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="30px">
                                    <p>
                                        &nbsp;</p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trSiteMaintenance">
                    <td colspan="2">
                        <table id="Table2" class="tableAdvSrchBorder" width="500px" height="100%" border="1"
                            cellpadding="4" cellspacing="12" align="center" runat="server">
                            <tr>
                                <td colspan="2" class="tdAccessHeader">
                                    <asp:Label ID="lblSiteMaintenanceMessage" runat="server" Text="" Font-Bold="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" runat="server" id="hidTeamOwner" />
    </form>
</body>
</html>
