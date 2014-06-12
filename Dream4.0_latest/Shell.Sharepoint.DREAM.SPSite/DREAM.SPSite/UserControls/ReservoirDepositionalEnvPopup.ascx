<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ReservoirDepositionalEnvPopup.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.ReservoirDepositionalEnvPopup" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/JavaScript" language="javascript" src="/_Layouts/DREAM/Javascript/SRPJavaScriptFunctionsRel3_0.js"></script>

<br />
<table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
    <tr>
        <td class="tdAdvSrchHeader" colspan="4">
            <b>Depositional Environment </b>
        </td>
    </tr>
</table>
<br />
<table width="100%" cellpadding="0" cellspacing="0" border="1">
    <tr>
        <td width="50%">
            <telerik:RadTreeView ID="radTreeViewDepositinalEnv" runat="server" Height="300px"
                Width="100%" OnClientNodeClicking="ReservoirDepositionalOnNodeClicking" OnNodeExpand="PopulateNodeOnDemand" />
        </td>
        <td width="50%" valign="top">
            &nbsp;&nbsp;<table id="DepositinalEnvTreeViewSelectedNodeTable">
            </table>
            &nbsp;&nbsp;<table width="100%">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <input type="button" class="shell-buttonAlternate" name="btnConfirm" value=" Confirm "
                                        style="font-weight: bold; display: none" onclick="OnConfirmButtonClickOfDepositional(this,'<%= hidFldDepositional.ClientID %>','<%= hidDepostionalLevel.ClientID %>');"
                                        depositionalvalue="" depositionallevel="" depositionaltext="" />
                                </td>
                                <%-- <td>
                                    <input type="button" class="shell-buttonAlternate" name="btClose" value=" Close "
                                        style="font-weight: bold; display: none" onclick="Javascript:window.close();" /></td>--%>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<input type="hidden" id="hidFldDepositional" runat="server" />
<input type="hidden" id="hidDepostionalLevel" runat="server" />

<script type="text/javascript">
  
setWindowTitle('Reservoir Depositional Environment Search');
</script>

