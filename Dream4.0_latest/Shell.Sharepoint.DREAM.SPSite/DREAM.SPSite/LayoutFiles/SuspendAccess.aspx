<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuspendAccess.aspx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.SuspendAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Suspend Access</title>
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
                    <td colspan="2" style="height: 100px">
                        <p>
                            &nbsp;</p>
                    </td>
                </tr>
                <tr>
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
                                <td colspan="2" height="10px">
                                    <table class="tableAdvSrchBorder" width="600px" height="100%" border="0"
                                        cellpadding="4" cellspacing="0" align="center">
                                        <tr>
                                            <td class="tdAccessData" colspan="2">
                                                <asp:Label ID="lblMessage" CssClass="labelMessage" runat="server" Text="" Font-Size="15px"></asp:Label></td>
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
            </table>
        </div>
    </form>
</body>
</html>
