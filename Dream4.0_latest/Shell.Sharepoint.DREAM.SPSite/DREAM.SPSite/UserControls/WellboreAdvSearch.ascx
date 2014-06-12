<!--
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: WellboreAdvSearch.ascx
-->
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="WellboreAdvSearch.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.WellboreAdvSearch" %>
<asp:Panel ID="ExceptionBlock" runat="server" Visible="false" Width="100%">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td colspan="4" class="tdAdvSrchHeader" style="font-weight: bold" id="tdExceptionAdvSrchTitle">
                <b>&nbsp;Advanced Search - Wellbore</b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSOAPException" runat="server" Text="" Visible="false" CssClass="labelMessage" /></td>
        </tr>
    </table>
</asp:Panel>
<table class="tableAdvSrchBorder" cellspacing="0" style="display: none;" width="100%">
    <tr>
        <td>
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelMessage"></asp:Label>
            <input type="hidden" value="" id="hidTest" runat="server" />
            <input type="hidden" value="" id="hidSearchName" runat="server" />
        </td>
    </tr>
</table>
<asp:Panel ID="AdvancedSearchContent" runat="server" DefaultButton="cmdSearch" Width="100%">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="4" class="tdAdvSrchHeader" id="tdAdvSrchTitle" style="font-weight: bold">
                <b>&nbsp;Advanced Search - Wellbore</b>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="GeneralExceptionBlock" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <span id="spanLstBxSelectionErr" class="labelMessage" style="display: none">Some error(s)
        occurred while processing the request. Please see the fields marked in * for more
        details.<br />
        <br />
    </span><span style="height: 5px"></span>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="AdvancedSearchSaveColumn">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td width="20%" valign="top" class="tdAdvSrchItemNbrdr">
                            &nbsp;Saved Search</td>
                        <td width="30%" class="tdAdvSrchItemNbrdr">
                            <asp:DropDownList ID="cboSavedSearch" runat="server" CssClass="dropdownAdvSrch" Width="185px"
                                OnSelectedIndexChanged="cboSavedSearch_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem>---Select---</asp:ListItem>
                            </asp:DropDownList><asp:Image ID="imgSavedSearch" runat="server" ImageAlign="AbsMiddle"
                                ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        <td align="left" width="50%" class="tdAdvSrchItemNbrdr">
                            <asp:Button ID="cmdTopSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                                OnClientClick="if(!ValidateWellboreSearch(false,'Wellbore'))return false;" OnClick="cmdSearch_Click" />
                            <input type="button" id="cmdTopReset" runat="server" value="Reset" class="buttonAdvSrch"
                                onclick="if(!ValidateAdvSearchReset())return false;" onserverclick="cmdReset_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <span style="height: 5px"></span>
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="4">
                <b>Search By File</b>[<a href="javascript:ResetFileSearchCriteria()" class="LinkTxt">Reset File Search Criteria</a>]
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                Search By</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:DropDownList ID="cboSearchCriteria" runat="server" CssClass="dropdownAdvSrch"
                    Width="185px">
                    <asp:ListItem>---Select---</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                Select File to search</td>
            <td colspan="3" class="tdAdvSrchItem">
                <input type="file" id="fileUploader" class="button" contenteditable="false" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="4">
                <b>Search</b>
            </td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                <asp:Label ID="lblUniqueIdentifier" runat="server" Text="Unique Wellbore Identifier"
                    Width="165px"></asp:Label></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtUnique_Wellbore_Identifier" runat="server" Width="180px" CssClass="queryfieldmini"
                    Visible="true"></asp:TextBox>
                <asp:Image ID="imgUWBI" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif"
                    Visible="true" />
            </td>
            <td colspan="2" width="50%" class="tdAdvSrchItem">
                [Wildcard = * OR %]</td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                <asp:Label ID="lblWellboreName" runat="server" Text="Wellbore Name" Width="165px"></asp:Label></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtWellbore_Name" runat="server" Width="180px" CssClass="queryfieldmini"
                    Visible="true"></asp:TextBox>
                <asp:Image ID="imgWellboreName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif"
                    Visible="true" />
            </td>
            <td colspan="2" width="50%" class="tdAdvSrchItem">
                [Wildcard = * OR %]</td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                <asp:Label ID="lblCommonName" runat="server" Text="Wellbore Common Name" Width="165px"></asp:Label></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtWellbore_Common_Name" runat="server" Width="180px" CssClass="queryfieldmini"
                    Visible="true"></asp:TextBox>
                <asp:Image ID="imgCommonName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif"
                    Visible="true" />
            </td>
            <td colspan="2" width="50%" class="tdAdvSrchItem">
                [Wildcard = * OR %]</td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                <asp:Label ID="lblAliasName" runat="server" Text="Wellbore Alias Name" Width="165px"></asp:Label></td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtAlias_Name" runat="server" Width="180px" CssClass="queryfieldmini"
                    Visible="true"></asp:TextBox>
                <asp:Image ID="imgAliasName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif"
                    Visible="true" />
            </td>
            <td colspan="2" width="50%" class="tdAdvSrchItem">
                [Wildcard = * OR %]</td>
        </tr>
        <tr>
            <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                <sup id="spanBasinSup" class="labelMessage" style="display: none">*</sup>Basin Identifier</td>
            <td colspan="3" class="tdAdvSrchItem">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr valign="top">
                        <td>
                            <asp:ListBox ID="lstBasin" EnableViewState="true" runat="server" Width="185px" CssClass="dropdownAdvSrch"
                                SelectionMode="Multiple"></asp:ListBox>
                            <asp:Image ID="imgBasin" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;&nbsp;
                        </td>
                        <td valign="bottom">
                            <span id="spanBasinSelectionError" class="labelMessage" style="display: none">The maximum
                                number of items per selection criteria is 999.
                                <br />
                                You have exceeded this for "Basin". Please amend your
                                <br />
                                criteria and try again. </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                <sup id="spanCountrySup" class="labelMessage" style="display: none">*</sup>Country</td>
            <td colspan="3" class="tdAdvSrchItem">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr valign="top">
                        <td>
                            <asp:DropDownList ID="lstCountry" runat="server" EnableViewState="true" Width="185px"
                                CssClass="dropdownAdvSrch" OnSelectedIndexChanged="lstCountry_SelectedIndexChanged"
                                AutoPostBack="True"></asp:DropDownList>
                            <asp:Image ID="imgCountry" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCountryInfo" runat="server" />
                            <br />
                            <br />
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
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                <asp:CheckBox ID="chbState" OnCheckedChanged="chbState_Change" Enabled="false" AutoPostBack="true"
                    runat="server" />&nbsp;<sup id="spanStateSup" class="labelMessage" style="display: none">*</sup>State
                or Province</td>
            <td colspan="3" class="tdAdvSrchItem">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr valign="top">
                        <td>
                            <asp:ListBox ID="lstState_Or_Province" runat="server" EnableViewState="true" Width="185px"
                                CssClass="dropdownAdvSrch" OnSelectedIndexChanged="lstState_Or_Province_SelectedIndexChanged"
                                SelectionMode="Multiple" AutoPostBack="True"></asp:ListBox>
                            <asp:Image ID="imgStateOrProvince" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblStateInfo" runat="server" />
                            <br />
                            <asp:Label ID="lblStateMessage" runat="server" Visible="false" />
                            <br />
                            <span id="spanStateSelectionError" class="labelMessage" style="display: none">The maximum
                                number of items per selection criteria is 999.
                                <br />
                                You have exceeded this for "State or Province". Please amend your
                                <br />
                                criteria and try again.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                <asp:CheckBox ID="chbCounty" Enabled="false" OnCheckedChanged="chbCounty_Change"
                    AutoPostBack="true" runat="server" />&nbsp;<sup id="spanCountySup" class="labelMessage"
                        style="display: none">*</sup>County</td>
            <td colspan="3" class="tdAdvSrchItem">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr valign="top">
                        <td>
                            <asp:ListBox ID="lstCounty" runat="server" EnableViewState="true" Width="185px" CssClass="dropdownAdvSrch"
                                SelectionMode="Multiple"></asp:ListBox>
                            <asp:Image ID="imgCounty" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;&nbsp;
                        </td>
                        <td valign="bottom">
                            <asp:Label ID="lblCountyMessage" runat="server" Visible="false" />
                            <span id="spanCountySelectionError" class="labelMessage" style="display: none">The maximum
                                number of items per selection criteria is 999.
                                <br />
                                You have exceeded this for "County". Please amend your
                                <br />
                                criteria and try again.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                <asp:CheckBox ID="chbField_Identifier" Enabled="false" OnCheckedChanged="chbField_Change"
                    AutoPostBack="true" runat="server" />&nbsp;<sup id="spanFieldSup" class="labelMessage"
                        style="display: none">*</sup>Field Name</td>
            <td colspan="3" class="tdAdvSrchItem">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr valign="top">
                        <td>
                            <asp:ListBox ID="lstField_Identifier" EnableViewState="true" runat="server" Width="185px"
                                CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox>
                            <asp:Image ID="imgFieldName" runat="server" ImageAlign="Top" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;&nbsp;
                        </td>
                        <td valign="bottom">
                            <asp:Label ID="lblField_Identifier" runat="server" Visible="false" />
                            <span id="spanFieldSelectionError" class="labelMessage" style="display: none">The maximum
                                number of items per selection criteria is 999.
                                <br />
                                You have exceeded this for "Field Name". Please amend your
                                <br />
                                criteria and try again.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- added for WDA requirments -->
        <tr id="trAPIAreas_Identifier" style="display: none;">
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                <asp:CheckBox ID="chbAPIAreas_Identifier" Enabled="false" AutoPostBack="true" runat="server"
                    OnCheckedChanged="chbAPIAreas_Identifier_Change" />&nbsp;API Area Identifier</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:ListBox ID="lstAPIAreas_Identifier" EnableViewState="true" runat="server" Width="185px"
                    CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox>
                <asp:Image ID="imgAPIAreaIdentifier" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                <asp:Label ID="lblAPIArea_Identifier" runat="server" Visible="false" /></td>
        </tr>
        <!-- added for WDA requirments -->
        <tr id="trBlock" style="display: none;">
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                Block</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:TextBox ID="txtBlocks" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgBlock" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                <asp:Label ID="lblBlock" runat="server" Visible="false" /></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                Onshore or Offshore</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:ListBox ID="lstOnshore_Or_Offshore" EnableViewState="true" runat="server" Width="185px"
                    CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox>&nbsp;<asp:Image
                        ID="imgOnshoreOrOffshore" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
        </tr>
        <tr id="tdWellType" style="display: none;">
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                Well Type</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:ListBox ID="lstKind" runat="server" EnableViewState="true" Width="185px" CssClass="dropdownAdvSrch"
                    SelectionMode="Multiple"></asp:ListBox>&nbsp;<asp:Image ID="imgWellType" runat="server"
                        ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
        </tr>
        <tr id="trPicks" style="display: none;">
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                <asp:CheckBox ID="chbFormation" Enabled="false" OnCheckedChanged="chbFormation_Change"
                    AutoPostBack="true" runat="server" />
                &nbsp;Formation</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:ListBox ID="lstPick" runat="server" Width="185px" CssClass="dropdownAdvSrch"
                    SelectionMode="Multiple" EnableViewState="true"></asp:ListBox>
                <asp:Image ID="imgFormation" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                <asp:Label ID="lblFormation" runat="server" Visible="false" /></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                Operator</td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtCurrent_Operator" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgOperator" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            <td colspan="2" width="50%" class="tdAdvSrchItem">
                &nbsp;</td>
        </tr>
        <tr id="trLeaseIdentifier" style="display: none;">
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                Lease Identifier</td>
            <td class="tdAdvSrchItem">
                <asp:TextBox ID="txtLease_Identifier" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgLeaseIdentifier" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            <td colspan="2" width="50%" class="tdAdvSrchItem">
                &nbsp;</td>
        </tr>
        <tr>
            <td id="tdAssetStatus" class="tdAdvSrchItem" valign="top" style="width: 20%">
                Wellbore Status</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:ListBox ID="lstCurrent_Status" EnableViewState="true" runat="server" Width="185px"
                    CssClass="dropdownAdvSrch" SelectionMode="Multiple"></asp:ListBox>&nbsp;<asp:Image
                        ID="imgWellboreStatus" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif"
                        Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchSubHeader" valign="middle" colspan="4">
                <b>Dates</b> [<a href="javascript:resetWellWellboreDateTable()" class="LinkTxt">Reset
                    Date Criteria</a>]</td>
        </tr>
        <tr id="trDate1">
            <td colspan="4" class="tdAdvSrchItem">
                <asp:RadioButtonList ID="rdoRbDate" runat="server" CssClass="radiobutton" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Spud/Kickoff" Value="SpudKickoff" onclick="javascript:EnblDisablWellwellboreAdvSrchDates('inline');"></asp:ListItem>
                    <asp:ListItem Text="Completion" Value="Completion" onclick="javascript:EnblDisablWellwellboreAdvSrchDates('inline');"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="trDates" style="display: none;">
            <td class="tdAdvSrchItem" valign="top" colspan="1" style="width: 20%">
                From</td>
            <td class="tdAdvSrchItem" width="40%">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="queryfieldmini" Width="180px"></asp:TextBox>
                <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtFrom');"
                    style="cursor: hand;" />
            </td>
            <td class="tdAdvSrchItem" width="10%">
                To</td>
            <td class="tdAdvSrchItem" width="40%">
                <asp:TextBox ID="txtTo" runat="server" CssClass="queryfieldmini" Width="180px"></asp:TextBox>
                <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtTo');"
                    style="cursor: hand;" />
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="4" valign="middle">
                <asp:CheckBox Height="18px" ToolTip="Reset Geographical fields" ID="chbGeographicalSearch"
                    runat="server" OnClick="javascript:showLatLongTable('TR1','TR2','TR3','TR4','TR5','TR6','TR7','TR8');SetGeographicalDefaultValues(this)" /><b>Geographical
                        Search</b></td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" valign="top" style="width: 20%">
                &nbsp;</td>
            <td colspan="3" class="tdAdvSrchItem">
                <asp:RadioButton runat="server" ID="rdoLatLong" Text="Lat / Long" GroupName="GeoSearch" />
            </td>
        </tr>
        <tr id="TR1" style="display: none;">
            <td colspan="4" class="tdNewAdvSrchSubHeader">
                <b>Lat/Long Search</b></td>
        </tr>
        <tr id="TR2" style="display: none;">
            <td class="tdAdvSrchItem" colspan="1" style="width: 20%">
                Surface or Bottom</td>
            <td class="tdAdvSrchItem" colspan="3">
                <asp:RadioButtonList ID="rdoRbLatLon" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="Surface">Surface</asp:ListItem>
                    <asp:ListItem Value="Bottom">Bottom</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="TR3" style="display: none;">
            <td class="tdNewAdvSrchSubHeader" colspan="4">
                <b>Min</b></td>
        </tr>
        <tr id="TR4" style="display: none;">
            <td class="tdAdvSrchItem" height="19px" style="width: 20%">
                Latitude</td>
            <td class="tdAdvSrchItem" colspan="3" height="19px">
                <asp:TextBox ID="txtMinLatDeg" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                deg &nbsp;
                <asp:TextBox ID="txtMinLatMin" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                min &nbsp;
                <asp:TextBox ID="txtMinLatSec" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                sec&nbsp;
                <asp:TextBox ID="txtMinLatNS" runat="server" MaxLength="1" Width="50px" CssClass="queryfieldmini"></asp:TextBox>
                N/S&nbsp;<br />
            </td>
        </tr>
        <tr id="TR5" style="display: none;">
            <td class="tdAdvSrchItem" style="width: 20%">
                Longitude</td>
            <td class="tdAdvSrchItem" colspan="3">
                <asp:TextBox ID="txtMinLonDeg" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                deg &nbsp;
                <asp:TextBox ID="txtMinLonMin" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                min &nbsp;
                <asp:TextBox ID="txtMinLonSec" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                sec&nbsp;
                <asp:TextBox ID="txtMinLonEW" runat="server" MaxLength="1" Width="50px" CssClass="queryfieldmini"></asp:TextBox>
                E/W&nbsp;<br />
            </td>
        </tr>
        <tr id="TR6" style="display: none;">
            <td class="tdNewAdvSrchSubHeader" colspan="4">
                <b>Max</b></td>
        </tr>
        <tr id="TR7" style="display: none;">
            <td class="tdAdvSrchItem" height="32px" style="width: 20%">
                Latitude</td>
            <td class="tdAdvSrchItem" colspan="3" height="32px">
                <asp:TextBox ID="txtMaxLatDeg" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                deg &nbsp;
                <asp:TextBox ID="txtMaxLatMin" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                min &nbsp;
                <asp:TextBox ID="txtMaxLatSec" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                sec&nbsp;&nbsp;
                <asp:TextBox ID="txtMaxLatNS" runat="server" Width="50px" MaxLength="1" CssClass="queryfieldmini"></asp:TextBox>
                N/S&nbsp;<br />
            </td>
        </tr>
        <tr id="TR8" style="display: none;">
            <td class="tdAdvSrchItem" style="width: 20%">
                Longitude</td>
            <td class="tdAdvSrchItem" colspan="3">
                <asp:TextBox ID="txtMaxLonDeg" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                deg &nbsp;
                <asp:TextBox ID="txtMaxLonMin" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                min &nbsp;
                <asp:TextBox ID="txtMaxLonSec" runat="server" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');" CssClass="queryfieldmini"></asp:TextBox>
                sec&nbsp;
                <asp:TextBox ID="txtMaxLonEW" runat="server" MaxLength="1" Width="50px" CssClass="queryfieldmini"></asp:TextBox>
                E/W&nbsp;<br />
            </td>
        </tr>
        <tr>
            <td colspan="4" class="AdvSaveSearchCriteria">
                <b>Save Search Criteria</b>
            </td>
        </tr>
        <tr>
            <td height="25px" rowspan="" style="width: 20%" class="tdAdvSrchItemNbrdr">
                Search Name</td>
            <td height="25px" width="30%" class="tdAdvSrchItemNbrdr">
                <asp:TextBox ID="txtSaveSearch" runat="server" Width="180px" onkeydown="if(event.keyCode == 13){if(!ValidateWellboreSaveSearch('Wellbore'))return false;};"
                    CssClass="queryfieldmini"></asp:TextBox>
                <asp:Image ID="imgSearchName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            <td height="25px" width="10%" class="tdAdvSrchItemNbrdr">
                <asp:CheckBox Height="18px" ToolTip="Type of Save Search(Personal/Shared)" Text="Shared"
                    ID="chbShared" runat="server" />
            </td>
            <td align="left" colspan="1" width="50%" class="tdAdvSrchItemNbrdr">
                <asp:Button ID="cmdSaveSearch" runat="server" Text="Save Search" CssClass="buttonAdvSrch"
                    OnClick="cmdSaveSearch_Click" />
                <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                    OnClick="cmdSearch_Click" OnClientClick="if(!ValidateWellboreSearch(false,'Wellbore'))return false;" />
                <input type="button" id="cmdReset" runat="server" value="Reset" class="buttonAdvSrch"
                    onclick="if(!ValidateAdvSearchReset())return false;" onserverclick="cmdReset_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>

<script type="text/JavaScript">
 setWindowTitle("Advanced Search - Wellbore");
    SetListBoxSelectedValues('<%= lstBasin.ClientID %>');  
    SetListBoxSelectedValues('<%= lstCountry.ClientID %>'); 
    SetListBoxSelectedValues('<%= lstState_Or_Province.ClientID %>');  
    SetListBoxSelectedValues('<%= lstCounty.ClientID %>');  
    SetListBoxSelectedValues('<%= lstField_Identifier.ClientID %>');  
    SetListBoxSelectedValues('<%= lstAPIAreas_Identifier.ClientID %>'); 
    SetListBoxSelectedValues('<%= lstOnshore_Or_Offshore.ClientID %>');      
    SetListBoxSelectedValues('<%= lstKind.ClientID %>');  
    SetListBoxSelectedValues('<%= lstPick.ClientID %>');  
    SetListBoxSelectedValues('<%= lstCurrent_Status.ClientID %>');  

    if(document.forms[0].elements[GetObjectID("chbDates", "input")].checked == true)
    {
        resetDateTable();
    }
    if(document.forms[0].elements[GetObjectID("chbGeographicalSearch", "input")].checked == true)
    {
        showLatLongTable('TR1','TR2','TR3','TR4','TR5','TR6','TR7','TR8');
    }
    
        
</script>

<input type="hidden" id="hidWordContent" runat="server" />