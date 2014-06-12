<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TVDepthPopup.ascx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.TVDepthPopup" %>
<script src="/_Layouts/DREAM/Javascript/AHTVDCalculatorRel2_1.js" language="javascript" type="text/JavaScript"></script>
 <table id ="tblAHpopup" width ="100%">
        <tr>
            <td width="60%"><span id="spn1" class ="labelMessage3">TV Top Depth</span>
            </td>
            <td width="30%"><asp:TextBox ID ="txtTVTopDepth" CssClass ="AHTVDepthTextBox" runat ="server"></asp:TextBox>
            </td>
            <td width="10%"><span id ="spnTVTopDepth" class ="labelMessage2"></span>
            </td>
        </tr>
        <tr>
            <td width="60%"><span id="Span1" class ="labelMessage3">TV Bottom Depth</span>
            </td>
            <td width="30%"><asp:TextBox ID ="txtTVBottomDepth" CssClass ="AHTVDepthTextBox" runat ="server"></asp:TextBox>
            </td>
            <td width="10%"><span id ="spnTVBottomDepth" class ="labelMessage2"></span>
            </td>
        </tr>
        <tr>
            <td width="60%"><span id="Span2" class ="labelMessage3">TV Depth Interval</span>
            </td>
            <td width="30%"><asp:TextBox ID ="txtTVDepthInterval" CssClass ="AHTVDepthTextBox" runat ="server"></asp:TextBox>
            </td>       
             <td width="10%"><span id ="spnTVInterval" class ="labelMessage2"></span>
            </td>
        </tr>
        <tr>
            <td width="15%" colspan="3" align ="center" >
            <br />
            <asp:Button ID ="btnPopulateDepth" runat ="server" Text ="Populate Depths"  Font-Bold ="true" CssClass="buttonAdvSrch" OnClientClick ="return CallTVParentmethod();" OnClick="BtnPopulateDepth_Click"/>
            </td>          
        </tr>
    </table>
<script language ="javascript">
 setWindowTitle('Populate Depths');
var depthReference = getQueryStringValue("rdoValue");
spnTVTopDepth.innerText = depthReference;
spnTVBottomDepth.innerText = depthReference;
spnTVInterval.innerText = depthReference;
</script>