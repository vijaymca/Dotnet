<%@ Control Language="C#" AutoEventWireup="true" Codebehind="FieldAdvSearch.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.FieldAdvSearch" %>

<script type="text/javascript" language="javascript" src="/_Layouts/DREAM/Javascript/SRPJavaScriptFunctionsRel3_0.js"></script>

<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:PlaceHolder ID="plcAJAX" runat="server" />
<div id="AdvancedSearchContainer">
    <asp:Panel ID="AdvancedSearchContent" DefaultButton="cmdSearch" runat="server" Width="100%">
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td class="tdAdvSrchHeader">
                    <b>Advanced Search - Field </b>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
        <span id="spanLstBxSelectionErr" class="labelMessage" style="display: none">Some error(s)
            occurred while processing the request. Please see the fields marked in * for more
            details.<br />
            <br />
        </span>
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr id="trLogicalOperator">
                <td class="tdAdvSrchItem" width="20%">
                    Search Condition</td>
                <td class="tdAdvSrchItem" width="30%">
                    <asp:RadioButtonList Width="150px" ID="rdblSearchCond" runat="server" RepeatDirection="Horizontal"
                        CssClass="queryfieldmini" CellPadding="0" CellSpacing="0">
                        <asp:ListItem Selected="True">AND</asp:ListItem>
                        <asp:ListItem>OR</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td align="left" class="tdAdvSrchItem" colspan="2" width="50%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="20%" class="tdAdvSrchItemNbrdr">
                    Saved Search
                </td>
                <td width="30%" class="tdAdvSrchItemNbrdr">
                    <asp:DropDownList ID="cboSavedSearch" runat="server" CssClass="dropdownAdvSrch" Width="150px"
                        OnSelectedIndexChanged="cboSavedSearch_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Image ID="imgSavedSearch" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td align="left" colspan="2" width="50%" class="tdAdvSrchItemNbrdr">
                    &nbsp;<asp:Button ID="cmdSavedSearchButton" runat="server" Text="Search" CssClass="buttonAdvSrch"
                        OnClick="cmdSearch_Click" />
                    <input type="button" id="btnResetButton" runat="server" value="Reset" class="buttonAdvSrch"
                        onserverclick="cmdReset_Click" onclick="EnableButton();" /></td>
            </tr>
        </table>
        <br />
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%"
            id="TABLE1">
            <tr>
                <td colspan="5" class="fieldSearchHeader">
                    <b>Search By File</b>[<a href="javascript:ResetFileSearchCriteria()" class="LinkTxt">Reset File Search Criteria</a>]</td>
            </tr>
            <tr>
                <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                    Search By</td>
                <td colspan="4" class="tdAdvSrchItem">
                    <asp:DropDownList ID="cboSearchCriteria" runat="server" CssClass="dropdownAdvSrch"
                        Width="150px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                    Select File to search</td>
                <td colspan="4" class="tdAdvSrchItem">
                    <input type="file" id="fileUploader" runat="server" class="buttonAdvSrch" contenteditable="false" /></td>
            </tr>
            <tr>
                <td colspan="5" class="fieldSearchHeader">
                    <b>Search</b></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="20%">
                    <span>Identifier</span></td>
                <td class="tdAdvSrchItem">
                    <asp:TextBox ID="txtField_Identifier" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgIdentifier" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" colspan="3" width="50%">
                    [Wildcard = * OR %]</td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="20%">
                    Description</td>
                <td class="tdAdvSrchItem" colspan="4">
                    <asp:TextBox ID="txtDescription" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgDescription" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="20%">
                    <sup id="spanCountrySup" class="labelMessage" style="display: none">*</sup>Country</td>
                <td colspan="4" class="tdAdvSrchItem">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr valign="top">
                            <td>
                                <asp:DropDownList ID="lstCountry" runat="server" EnableViewState="true" Width="155px"
                                    CssClass="dropdownAdvSrch"></asp:DropDownList>
                                <asp:Image ID="imgCountry" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;&nbsp;
                            </td>
                            <td>
                                <span id="spanCountrySelectionError" class="labelMessage" style="display: none">The
                                    maximum number of items per selection criteria is 999.
                                    <br />
                                    You have exceeded this for "Country". Please amend your
                                    <br />
                                    criteria and try again.</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="20%">
                    Owner</td>
                <td class="tdAdvSrchItem" colspan="4">
                    <asp:TextBox ID="txtOwner" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgOwner" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="20%">
                    Field Name</td>
                <td class="tdAdvSrchItem">
                    <telerik:RadComboBox ID="radCboFieldName" runat="server" Width="155px" MarkFirstMatch="true"
                        AllowCustomText="true" EmptyMessage="Enter a Field Name" EnableLoadOnDemand="True"
                        ShowMoreResultsBox="true" EnableVirtualScrolling="true" EnableItemCaching="True"
                        OnClientItemsRequesting="HandleFieldRequestStart" OnItemsRequested="cmdRadCboFieldNameComboBox_ItemsRequested"
                        CssClass="AdvSearchRadcombo" Skin="Sitefinity">
                    </telerik:RadComboBox>
                    &nbsp;<asp:Image ID="imgFieldName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                </td>
                <td class="tdAdvSrchItem" width="20%">
                    Type of Field</td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtFieldType" runat="server" Width="120px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgTypeofField" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="20%">
                    Operator Company</td>
                <td class="tdAdvSrchItem">
                    <telerik:RadComboBox ID="radCboOperator" runat="server" Width="155px" MarkFirstMatch="true"
                        AllowCustomText="true" EnableItemCaching="True" EmptyMessage="Enter a Operator"
                        EnableLoadOnDemand="True" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                        OnClientItemsRequesting="HandleRequestStart" OnItemsRequested="cmdRadCboOperatorComboBox_ItemsRequested"
                        CssClass="AdvSearchRadcombo" Skin="Sitefinity">
                    </telerik:RadComboBox>
                    &nbsp;<input id="cmdOperator" class="buttonAdvSrch" type="button" value="..." runat="server"
                        style="vertical-align: bottom" />
                    <asp:Image ID="imgOperator" runat="server" ImageAlign="AbsBottom" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Current Status</td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtCurrentStatus" runat="server" Width="120px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgCurrentStatus" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr id="trAdvFieldSearchGeology0" style="display: block">
                <td class="fieldSearchHeader" colspan="5">
                    <input type="CHECKBOX" name="chkGeology" onclick="RowToggler(this,'trAdvFieldSearchGeology',2);"
                        checked="checked">
                    <b>Geological Search</b>
                </td>
            </tr>
            <tr style="display: block" id="trAdvFieldSearchGeology1">
                <td class="tdAdvSrchItem">
                    Basin Identifier</td>
                <td class="tdAdvSrchItem" align="left">
                    <telerik:RadComboBox ID="radCboBasinName" runat="server" Width="155px" MarkFirstMatch="true"
                        AllowCustomText="true" EmptyMessage="Enter a Basin Name" EnableLoadOnDemand="True"
                        ShowMoreResultsBox="true" EnableVirtualScrolling="true" EnableItemCaching="True"
                        OnClientItemsRequesting="HandleBasinRequestStart" OnItemsRequested="cmdRadCboBasinNameComboBox_ItemsRequested"
                        CssClass="AdvSearchRadcombo" Skin="Sitefinity">
                    </telerik:RadComboBox>
                    &nbsp;<input id="cmdBasinName" type="button" value="..." class="buttonAdvSrch" style="vertical-align: bottom"
                        runat="server" />
                    <asp:Image ID="imgBasinName" runat="server" ImageAlign="AbsBottom" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Operational Environment</td>
                <td class="tdAdvSrchItem" width="20%">
                    <asp:DropDownList ID="cboOperationalEnv" runat="server" CssClass="dropdownAdvSrch"
                        Width="125px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="10%" align="left" class="tdAdvSrchItem">
                    <asp:Image ID="imgOperationalEnv" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                </td>
            </tr>
            <tr style="display: block" id="trAdvFieldSearchGeology2">
                <td class="tdAdvSrchItem" width="20%">
                    Tectonic Classification</td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:RadioButtonList Width="150px" ID="rdblTectonicClassification" runat="server"
                        RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                        <asp:ListItem Selected="True">Bally</asp:ListItem>
                        <asp:ListItem>Klemme</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td class="tdAdvSrchItem" width="20%">
                    Tectonic Setting</td>
                <td class="tdAdvSrchItem" width="20%">
                    <asp:DropDownList ID="cboTectonicSetting" runat="server" CssClass="dropdownAdvSrch"
                        Width="125px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboTectonicSettingKle" runat="server" CssClass="dropdownAdvSrch"
                        Width="155px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="10%" align="left" class="tdAdvSrchItem">
                    <asp:Image ID="imgTectonicSetting" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                </td>
            </tr>
            <tr id="trAdvFieldSearchProductionSearch0" style="display: block">
                <td class="fieldSearchHeader" colspan="5">
                    <input type="CHECKBOX" name="chkGeology" onclick="RowToggler(this,'trAdvFieldSearchProductionSearch',5);"
                        checked="checked">
                    <b>Production Search</b></td>
            </tr>
            <tr id="trAdvFieldSearchProductionSearch1" style="display: block">
                <td class="tdAdvSrchItem" width="20%">
                    Reserve Magnitude Oil</td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:DropDownList ID="cboReserveMagOil" runat="server" CssClass="dropdownAdvSrch"
                        Width="155px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    Mmbbl
                    <asp:Image ID="imgReserveMagnitudeOil" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%" nowrap="nowrap">
                    Oil In Place*
                </td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtOilInPlace" runat="server" CssClass="queryfieldmini" Width="120px"></asp:TextBox>
                    Mmbbl
                    <asp:Image ID="imgOilInPlace" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr id="trAdvFieldSearchProductionSearch2" style="display: block">
                <td class="tdAdvSrchItem" width="20%">
                    Reserve Magnitude Gas
                </td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:DropDownList ID="cboReserveMagGas" runat="server" CssClass="dropdownAdvSrch"
                        Width="155px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    MMScf</td>
                <td class="tdAdvSrchItem" width="20%">
                    Gas Initially In Place(GIIP)*
                </td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtGasInitiallyInPlace" runat="server" CssClass="queryfieldmini"
                        Width="120px"></asp:TextBox>
                    MMScf
                    <asp:Image ID="imgGasInitiallyInPlace" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr id="trAdvFieldSearchProductionSearch3" style="display: block">
                <td class="tdAdvSrchItem" width="20%">
                    Condensate Recovery Factor*
                </td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:TextBox ID="txtCondensateRecovery" runat="server" CssClass="queryfieldmini"
                        Width="150px"></asp:TextBox>
                    %
                    <asp:Image ID="imgCondensateRecoveryFactor" runat="server" ImageAlign="AbsMiddle"
                        ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Condensate In Place*
                </td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtCondensateInPlace" runat="server" CssClass="queryfieldmini" Width="120px"></asp:TextBox>
                    Mmbbl
                    <asp:Image ID="imgCondensateInPlace" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr id="trAdvFieldSearchProductionSearch4" style="display: block">
                <td class="tdAdvSrchItem" width="20%">
                    No. of Wells
                </td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:TextBox ID="txtNoOfWells" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>
                    <asp:Image ID="imgNoOfWells" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    No. of Producers</td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtNoOfProducers" runat="server" CssClass="queryfieldmini" Width="120px"></asp:TextBox>
                    <asp:Image ID="imgNoOfProducers" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr id="trAdvFieldSearchProductionSearch5" style="display: block">
                <td class="tdAdvSrchItem" width="20%">
                    &nbsp;</td>
                <td class="tdAdvSrchItem">
                    &nbsp;</td>
                <td class="tdAdvSrchItem" width="20%">
                    No. of Injectors</td>
                <td class="tdAdvSrchItem" width="30%" colspan="2">
                    <asp:TextBox ID="txtNoOfInjectors" runat="server" CssClass="queryfieldmini" Width="120px"></asp:TextBox>
                    <asp:Image ID="imgNoOfInjector" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr id="trAdvFieldSearchPetrophysicalSearch0" style="display: block">
                <td class="fieldSearchHeader" colspan="5">
                    <input type="CHECKBOX" name="chkGeology" onclick="RowToggler(this,'trAdvFieldSearchPetrophysicalSearch',2);"
                        checked="checked">
                    <b>Petrophysical Search</b></td>
            </tr>
            <tr style="display: block" id="trAdvFieldSearchPetrophysicalSearch1">
                <td class="tdAdvSrchItem" width="20%">
                    Porosity (Max)*
                </td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:TextBox ID="txtPorosity" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>&nbsp;
                    %
                    <asp:Image ID="imgPorosityMax" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="50%" align="left" colspan="3">
                    <asp:Label ID="lblSRPPorosityInfo" runat="server" Text="Porosity - Allowed range is 0% to 50%"></asp:Label>
                </td>
            </tr>
            <tr style="display: block" id="trAdvFieldSearchPetrophysicalSearch2">
                <td class="tdAdvSrchItem" width="20%">
                    Permeability (Max)*
                </td>
                <td class="tdAdvSrchItem" width="30%" align="left">
                    <asp:TextBox ID="txtPermeability" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>&nbsp;
                    md
                    <asp:Image ID="imgPermeabilityMax" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="50%" align="left" colspan="3">
                    <asp:Label ID="lblSRPPermeabilityInfo" runat="server" Text="Permeability - Allowed range is 0.0001md to 20000md"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" class="tableAdvSrchBorder" cellspacing="0" cellpadding="4">
            <tr>
                <td colspan="5" class="fieldSearchHeader" width="100%">
                    <b>Save Search Criteria</b>
                </td>
            </tr>
            <tr>
                <td width="50%" class="tdAdvSrchItem" valign="middle" align="right" colspan="2">
                    &nbsp;</td>
                <td width="100%" class="tdAdvSrchItem" valign="middle" align="left" colspan="3">
                    <asp:Label ID="lblSRPInfo" runat="server" Font-Bold="true" Text="* Search will be performed for +/- 50% of the entered value"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="20%" class="tdAdvSrchItem">
                    Search Name
                </td>
                <td width="30%" class="tdAdvSrchItem" align="left">
                    <asp:TextBox ID="txtSaveSearch" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgSearchName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                </td>
                <td width="20%" class="tdAdvSrchItem" colspan="2" align="left">
                    <asp:CheckBox Height="18px" ToolTip="Type of Save Search(Personal/Shared)" Text="Shared"
                        ID="chbShared" runat="server" />
                </td>
                <td align="left" class="tdAdvSrchItem" width="30%">
                    <input type="button" id="cmdSaveSearch" runat="server" value="Save Search" class="buttonAdvSrch"
                        onserverclick="cmdSaveSearch_Click" />
                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                        OnClick="cmdSearch_Click" />
                    <input type="button" id="cmdReset" runat="server" value="Reset" class="buttonAdvSrch"
                        onserverclick="cmdReset_Click" onclick="EnableButton();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<input type="hidden" id="hidWordContent" runat="server" />
<input type="hidden" id="hidRadContent" runat="server" />
<input type="hidden" id="hidTechContent" runat="server" />
<input type="hidden" id="hidSrpControlSectionId" runat="server" />

<script type="text/JavaScript">
    setWindowTitle('Advanced Search - Field');
    HideDisplaySRPControlSection('<%= hidSrpControlSectionId.ClientID %>');
//    SetListBoxSelectedValues('<%= lstCountry.ClientID %>');         
    if(document.forms[0].elements[GetObjectID("chbGeographicalSearch", "input")].checked == true)
    {
        showPARSLatLongTable('TR1','TR2','TR3','TR4','TR5','TR6');
    }          
</script>

