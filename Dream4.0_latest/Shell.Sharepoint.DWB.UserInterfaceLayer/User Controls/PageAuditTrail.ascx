<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageAuditTrail.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.PageAuditTrail" %>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<%--<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />--%>

<table border="0" width="100%" height ="100%"> 
    <tr>
            <td style="height: 18px">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="labelMessage" Visible ="false"></asp:Label>
            </td>
        </tr>             
   <tr>
   <td  style="width: 100%; height:100%; vertical-align:top" align="left">
   <div class="DWBAuditTrailFixedheader">
       <asp:GridView ID="grdAuditTrail" AutoGenerateColumns ="False" runat="server" ShowFooter="False"  Width="97%">
        <HeaderStyle  BackColor="#BDBDBD"  Font-Bold="True" />    
         <AlternatingRowStyle BackColor="#ECECEC" />
           <Columns>
                <asp:BoundField DataField="Date"  HeaderText = "Date/Time"  HeaderStyle-HorizontalAlign = "center" ItemStyle-HorizontalAlign ="left"  />
                <asp:BoundField DataField="LinkTitle" HeaderText = "Username" HeaderStyle-HorizontalAlign = "center" ItemStyle-HorizontalAlign ="left"/>
                <asp:BoundField DataField="Audit_Action" HeaderText = "Audit Action"  HeaderStyle-HorizontalAlign = "center" ItemStyle-HorizontalAlign ="left"/>
           </Columns>
       </asp:GridView>
       </div> 
       </td>
   </tr> 
 
</table>     
