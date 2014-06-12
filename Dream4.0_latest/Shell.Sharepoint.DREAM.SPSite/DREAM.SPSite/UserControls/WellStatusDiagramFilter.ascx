<%@ Control Language="C#" AutoEventWireup="true" Codebehind="WellStatusDiagramFilter.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.WellStatusDiagramFilter" %>

<script type="text/javascript" language="javascript" src="/_Layouts/DREAM/Javascript/WellStatusDiagram.js"></script>

<div id="message">
    <p>
        <span style="font-size: 1.2em; font-weight: bold">Generating Well Status Diagram.....
        </span>
        <br />
        Please be patient, it may take some time. If the preferences are not the ones you
        would like to use, wait until the diagram shows up. Then choose different preferences
        and press the (Re)generate button.
    </p>
</div>
<div id="prefs">
    <p>
        <span style="font-size: 1em; font-weight: bold">Your preferences </span>
    </p>
    <table>
        <tr>
            <td style="font-size: 10pt;">
                Datum
            </td>
            <td>
                <select style="font-size: 8pt;width: 140px;" id="ddlDatum" name="Datum">
                    <option value='DFE' selected="selected">DFE </option>
                    <option value='GLE'>GLE </option>
                    <option value='MSL'>MSL </option>
                    <option value='NAP'>NAP </option>
                    <option value='RDL'>RDL </option>
                    <option value='TBF'>TBF </option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="font-size: 10pt;">
                Template
            </td>
            <td>
                <select style="font-size: 8pt;width: 140px;" id="ddlTemplate" name="Template">
                    <option value='EP As Built.ppc' selected="selected">EP As Built.ppc </option>
                    <option value='EP As Built Info.ppc'>EP As Built Info.ppc </option>
                    <option value='EP Drilling.ppc'>EP Drilling.ppc </option>
                    <option value='EP General.ppc'>EP General.ppc </option>
                    <option value='EP Wireline.ppc'>EP Wireline.ppc </option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="font-size: 10pt;">
                Units
            </td>
            <td>
                <select style="font-size: 8pt;width: 140px;" id="ddlUnits" name="Units">
                    <option value='API'>API </option>
                    <option value='EPE'>EPE </option>
                    <option value='Shell Expro' selected="selected">Shell Expro </option>
                    <option value='SI'>SI </option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="font-size: 10pt;">
                Use cache (if available)
            </td>
            <td>
                <input type="checkbox" id="chbxUsecache" name="usecache" checked="checked" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input style="font-size: 8pt;" type="button" class="clsButton" name="btnSearch" onclick="OpenWellStatusDiagram();"
                    value="(Re)generate" />
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidWSDUrl" name="WSDUrl" value="http://sww-discovery.shell.com:500/ERO/WsdDynamic.aspx?DataTypeName=WSD_DYNAMIC&ID=11000080304101&template=EP%20As%20Built%20Info.ppc&datum=NAP&units=EPE&usecache=True" />
    <p>
        Please be aware that not every combination of preference may yield a valid (if any)
        diagram.
    </p>
</div>
