<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WellBook.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.WellBook" %>
<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />


<!--Display Progress bar Frame starts here-->
<%--<iframe id="BusyBoxIFrame" visible="false" style="border:3px double #D2D2D2" name="BusyBoxIFrame" frameBorder="0" scrolling="no" ondrop="return false;">
</iframe>
<SCRIPT>
	// Instantiate our BusyBox object
	var busyBox = new BusyBox("BusyBoxIFrame", "busyBox", 1, "processing", ".gif", 125, 147, 207);	
</SCRIPT>--%>
<!--Display Progress bar Frame Ends here-->
<div id="AdvancedSearchContainer">                        
<asp:Panel ID="pnlTemplate" runat="server" Width="100%">
<table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
    <tr>
        <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: normal">
            &nbsp;<asp:Label ID="lblWellBookHeading" runat="server" Text="Add Book" Width="80%" Font-Bold="True"></asp:Label></td>
    </tr>
</table>
<br />
<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">    
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>
    <asp:HiddenField ID="hdnListitemStatus" runat="server" />
<table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%"> 
    <tr>
            <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px"><b>Details</b>
                &nbsp;[ <span class ="DWBMandatoryMessage">* indicates mandatory field</span>
            ]</td>
        </tr>             
    <tr>
        <td class="DWItemText" style="width: 17%; height: 28px;">
          <span class ="DWBMandatory">*</span>
            <span>Name</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 28px;">
            <asp:TextBox ID="txtWellBookTitle" runat="server" Width="44%" CssClass="DWBqueryfieldmini" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 17%; height: 26px;">
         <span class ="DWBMandatory">*</span>
            <span>Team</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
            <asp:DropDownList ID="cboTeam" runat="server" CssClass="DWBdropdownAdvSrch" Width="28%" AutoPostBack="True" OnSelectedIndexChanged="cboTeam_SelectedIndexChanged">
                    <asp:ListItem>--Select--</asp:ListItem>
                   <%-- <asp:ListItem>Well</asp:ListItem>
                    <asp:ListItem>Field</asp:ListItem>
                    <asp:ListItem>Wellbore</asp:ListItem>--%>
                </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 17%; height: 26px">
            Owner</td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px">
            <asp:DropDownList ID="cboOwner" runat="server" CssClass="DWBdropdownAdvSrch" Width="28%" AutoPostBack="false" >
              
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td align="center"  style=" height: 28px" colspan ="2">
            &nbsp;<asp:Button ID="cmdOK" runat="server" Text="Save"  CssClass="DWBbuttonAdvSrch"   width="10%" OnClientClick="javascript:return ValidateDWBWellBook();" OnClick="cmdOK_Click" />
            <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch" OnClick="cmdCancel_Click" width="10%" /></td>
        
  
    </tr>
</table>     
</asp:Panel>
</div>

