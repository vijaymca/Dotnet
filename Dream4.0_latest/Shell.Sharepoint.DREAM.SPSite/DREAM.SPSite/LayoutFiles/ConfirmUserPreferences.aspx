<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ConfirmUserPreferences.aspx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.ConfirmUserPreferences" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Preferences</title>
    <link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <table cellpadding="10px" cellspacing="0" width="100%">
        <tr>
        <td> 
         <table class="tableAdvSrchBorder"  cellpadding="5px" cellspacing="0" width="100%">
                <tr>
                    <td class="tdBasinAdvSrchHeader" colspan="3">
                        User Preferences</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        <br />
                        <br />
                        <br />
                        <div align="center">
                            <asp:Button ID="cmdClose" runat="server" CssClass="buttonAdvSrch" Text="Close" OnClientClick="window.close();"
                                Width="60px" />
                        </div>
                        <br />
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
