<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditStaffPrivilege.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.User_Controls.EditStaffPrivilege" %>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<div id="Container">
<asp:Panel ID="pnlTemp" runat="server" Width="100%">

<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">    
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>
<table cellspacing="2" cellpadding="2" border="0" width="100%">
<tr>
                <td class="DWItemText" style="width: 28%; height: 28px;">
                    
                    <asp:Label ID="lblPrivilege" runat="server" Text="Privileges"></asp:Label></td>
                    
                <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
                    <asp:DropDownList ID="cboEditPrivileges" runat="server" Width="40%" CssClass="DWBdropdownAdvSrch">
                        <asp:ListItem Value="--Select--"></asp:ListItem>
                    </asp:DropDownList>
                </td>
    </tr>
            <tr>
                <td align="center" style="height: 28px" colspan="2">
                    &nbsp;<asp:Button ID="cmdOK" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch" Width="10%" OnClick="cmdOK_Click" />
                    <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                        Font-Size="X-Small" Width="10%" OnClick="cmdCancel_Click" Onclientclick="JavaScript:return CloseWithoutPrompt();"/></td>
            </tr>
            
            </table>
</asp:Panel>
</div>